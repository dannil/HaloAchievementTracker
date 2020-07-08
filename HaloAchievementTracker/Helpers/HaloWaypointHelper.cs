﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace HaloAchievementTracker.Helpers
{
    public class HaloWaypointHelper
    {
        private readonly HtmlDocument document;

        public HaloWaypointHelper(HtmlDocument document)
        {
            this.document = document;
        }

        public ISet<HaloWaypointAchievement> GetAchievements()
        {
            HtmlNode serviceRecordAchievementsNode = document.DocumentNode.SelectSingleNode($"//div[@class='{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_DIV}']");
            HtmlNodeCollection achievementCollections = serviceRecordAchievementsNode.SelectNodes($"//div[@class='{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_COLLECTION_DIV}']");

            ISet<HaloWaypointAchievement> haloWaypointAchievements = new HashSet<HaloWaypointAchievement>();
            foreach (HtmlNode achievementCollection in achievementCollections)
            {
                HtmlNodeCollection achievements = achievementCollection.SelectNodes($".//ul[contains(@class,'{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_LIST_CLASS}')]/li");
                foreach (HtmlNode achievement in achievements)
                {
                    HaloWaypointAchievement haloWaypointAchievement = new HaloWaypointAchievement();

                    var titleNode = achievement.SelectSingleNode(".//p[@class='text--medium title']");
                    haloWaypointAchievement.Name = HttpUtility.HtmlDecode(titleNode.InnerText);

                    string gameId = achievementCollection.GetAttributeValue("data-game-id", string.Empty);
                    haloWaypointAchievement.GameId = gameId;

                    bool isUnlocked = achievement.GetAttributeValue("class", string.Empty).Contains("unlocked");
                    haloWaypointAchievement.IsUnlocked = isUnlocked;

                    haloWaypointAchievements.Add(haloWaypointAchievement);
                }
            }
            return haloWaypointAchievements;
        }

    }
}
