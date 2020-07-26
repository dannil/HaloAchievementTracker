using HaloAchievementTracker.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HaloAchievementTracker
{
    public class Program2
    {
        public static async Task Main(string[] args)
        {
            OpenXBLService service = new OpenXBLService("8cgo0k0c0wo484ok0gwwckw4w4kg0so8gko");

            var res = await service.GetFriendsByGamertagAsync("PolyPropGod");

            string id = res.ProfileUsers[0].Id;

            var res2 = await service.GetAchievementsAsync(id, "1144039928");

            int a = 2;

            Console.WriteLine(res2.Achievements[13].IsUnlocked);
        }

    }
}
