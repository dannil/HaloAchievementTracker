using HaloAchievementTracker.Common.Adapters;
using HaloAchievementTracker.Common.Helpers;
using HaloAchievementTracker.Common.Models;
using HtmlAgilityPack;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Common.Services
{
    public class SteamService : ISteamService
    {
        private readonly HttpClient _httpClient;

        public SteamService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual async Task<IEnumerable<SteamAchievement>> GetAchievementsByApiAsync(ISteamWebInterfaceFactory webInterfaceFactory, uint appId, ulong steamId)
        {
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUserStatsAdapter>(new HttpClient());
            var stats = await steamInterface.GetPlayerAchievementsAsync(appId, steamId);
            return stats.Data.Achievements
                .Select(a =>
                    new SteamAchievement
                    {
                        Name = a.Name,
                        Game = AchievementHelper.GetGameFromDescription(a.Description),
                        Description = a.Description,
                        IsUnlocked = Convert.ToBoolean(a.Achieved)
                    });
        }

        public virtual async Task<IEnumerable<SteamAchievement>> GetAchievementsByScrapingAsync(uint appId, ulong steamId)
        {
            //var url = $"https://steamcommunity.com/profiles/{steamId}/stats/appid/{appId}/achievements";
            //var httpClient = new HttpClient();
            var endpoint = $"{steamId}/stats/appid/{appId}/achievements";
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                var document = new HtmlDocument();
                document.LoadHtml(responseBody);

                HtmlNodeCollection achieveRowsNodes = document.DocumentNode.SelectNodes($"//div[@class='{Constants.STEAM_ACHIEVE_ROW_DIV}']");

                ISet<SteamAchievement> achievements = new HashSet<SteamAchievement>();
                foreach (HtmlNode achieveRowsNode in achieveRowsNodes)
                {
                    SteamAchievement steamAchievement = new SteamAchievement();

                    var achieveTxtNode = achieveRowsNode.SelectSingleNode($".//div[@class='{Constants.STEAM_ACHIEVE_TXT_DIV}']");

                    steamAchievement.Name = achieveTxtNode.SelectSingleNode($".//h3").InnerText;
                    steamAchievement.Description = achieveTxtNode.SelectSingleNode($".//h5").InnerText;

                    Game game = null;
                    if (SteamHelper.HasMissingGameIdentifier(steamAchievement.Name))
                    {
                        game = SteamHelper.Get(steamAchievement.Name);
                    }
                    else
                    {
                        game = AchievementHelper.GetGameFromDescription(steamAchievement.Description);
                    }

                    steamAchievement.Game = game;
                    steamAchievement.IsUnlocked = achieveTxtNode.Descendants("div").Any(d => d.GetAttributeValue("class", string.Empty).Equals("achieveUnlockTime"));

                    achievements.Add(steamAchievement);
                }
                return achievements;
            }
            throw new HttpRequestException();
        }

    }

    public interface ISteamService
    {
        Task<IEnumerable<SteamAchievement>> GetAchievementsByApiAsync(ISteamWebInterfaceFactory webInterfaceFactory, uint appId, ulong steamId);

        Task<IEnumerable<SteamAchievement>> GetAchievementsByScrapingAsync(uint appId, ulong steamId);
    }
}
