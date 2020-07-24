using HaloAchievementTracker.Adapters;
using HaloAchievementTracker.Models;
using HtmlAgilityPack;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker.Services
{
    public class SteamService
    {
        public SteamService()
        {
        }

        public virtual async Task<ISet<SteamAchievement>> GetAchievementsByApiAsync(ISteamWebInterfaceFactory webInterfaceFactory, uint appId, ulong steamId)
        {
            var steamInterface = webInterfaceFactory.CreateSteamWebInterface<SteamUserStatsAdapter>(new HttpClient());
            var stats = await steamInterface.GetPlayerAchievementsAsync(appId, steamId);
            return stats.Data.Achievements
                .Select(a =>
                    new SteamAchievement
                    {
                        Name = a.Name,
                        Description = a.Description,
                        IsUnlocked = Convert.ToBoolean(a.Achieved)
                    })
                .ToHashSet();
        }

        public virtual async Task<ISet<SteamAchievement>> GetAchievementsByScrapingAsync(uint appId, ulong steamId)
        {
            var url = $"https://steamcommunity.com/profiles/{steamId}/stats/appid/{appId}/achievements";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(responseBody);

            HtmlNodeCollection achieveRowsNodes = document.DocumentNode.SelectNodes($"//div[@class='{Constants.STEAM_ACHIEVE_ROW_DIV}']");

            ISet <SteamAchievement> achievements = new HashSet<SteamAchievement>();
            foreach (HtmlNode achieveRowsNode in achieveRowsNodes)
            {
                SteamAchievement steamAchievement = new SteamAchievement();

                var achieveTxtNode = achieveRowsNode.SelectSingleNode($".//div[@class='{Constants.STEAM_ACHIEVE_TXT_DIV}']");

                steamAchievement.Name = achieveTxtNode.SelectSingleNode($".//h3").InnerText;
                steamAchievement.Description = achieveTxtNode.SelectSingleNode($".//h5").InnerText;
                steamAchievement.IsUnlocked = achieveTxtNode.Descendants("div").Any(d => d.GetAttributeValue("class", string.Empty).Equals("achieveUnlockTime"));

                achievements.Add(steamAchievement);
            }
            return achievements;
        }

    }
}
