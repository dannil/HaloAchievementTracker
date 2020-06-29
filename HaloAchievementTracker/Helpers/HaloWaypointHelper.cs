using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace HaloAchievementTracker.Helpers
{
    public class HaloWaypointHelper
    {
        private readonly HtmlDocument document;

        public HaloWaypointHelper(string fileLocation)
        {
            document = new HtmlDocument();
            document.Load(fileLocation);
        }

        public ISet<HaloWaypointAchievement> GetAchievements()
        {
            ISet<HaloWaypointAchievement> haloWaypointAchievements = new HashSet<HaloWaypointAchievement>();

            HtmlNode serviceRecordAchievementsNode = document.DocumentNode.SelectSingleNode($"//div[@class='{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_DIV}']");
            HtmlNodeCollection achievements = serviceRecordAchievementsNode.SelectNodes($"//div[@class='{Constants.HALO_WAYPOINT_SERVICE_RECORDS_ACHIEVEMENT_COLLECTION_DIV}']/ul/li");

            foreach (HtmlNode achievement in achievements)
            {
                HaloWaypointAchievement model = new HaloWaypointAchievement();

                var achievementTitleNode = achievement.SelectSingleNode(".//p[@class='text--medium title']");
                model.Name = HttpUtility.HtmlDecode(achievementTitleNode.InnerText);

                bool isAchievementUnlocked = achievement.GetAttributeValue("class", string.Empty).Contains("unlocked");
                model.IsUnlocked = isAchievementUnlocked;

                haloWaypointAchievements.Add(model);
            }
            return haloWaypointAchievements;
        }

    }
}
