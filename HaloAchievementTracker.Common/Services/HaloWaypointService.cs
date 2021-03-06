﻿using HaloAchievementTracker.Common.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace HaloAchievementTracker.Common.Services
{
    public class HaloWaypointService : IHaloWaypointService
    {
        private readonly HtmlDocument document;

        public HaloWaypointService(HtmlDocument document)
        {
            this.document = document;
        }

        public virtual IEnumerable<XboxLiveAchievement> GetAchievements()
        {
            HtmlNodeCollection achievementCollectionNodes = document.DocumentNode.SelectNodes($"//div[@class='{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_COLLECTION_DIV}']");

            ISet<XboxLiveAchievement> achievements = new HashSet<XboxLiveAchievement>();
            foreach (HtmlNode achievementCollectionNode in achievementCollectionNodes)
            {
                HtmlNodeCollection achievementNodes = achievementCollectionNode.SelectNodes($".//ul[contains(@class,'{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_LIST_CLASS}')]/li");
                foreach (HtmlNode achievementNode in achievementNodes)
                {
                    XboxLiveAchievement xboxLiveAchievement = new XboxLiveAchievement();

                    var titleNode = achievementNode.SelectSingleNode(".//p[@class='text--medium title']");
                    xboxLiveAchievement.Name = HttpUtility.HtmlDecode(titleNode.InnerText);

                    string gameId = achievementCollectionNode.GetAttributeValue("data-game-id", string.Empty);
                    xboxLiveAchievement.Game = GameFactory.Get(gameId);

                    bool isUnlocked = achievementNode.GetAttributeValue("class", string.Empty).Contains("unlocked");
                    xboxLiveAchievement.IsUnlocked = isUnlocked;

                    achievements.Add(xboxLiveAchievement);
                }
            }
            return achievements;
        }

    }

    public interface IHaloWaypointService
    {
        IEnumerable<XboxLiveAchievement> GetAchievements();
    }
}
