using System;
using System.Collections.Generic;
using System.Text;

namespace HaloAchievementTracker.Common.Models
{
    public class Game
    {
        public string Name { get; set; }

        public Game(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class GameFactory
    {
        private static readonly Game CrossGame = new Game("Cross Game");
        private static Game HaloReach = new Game("Halo: Reach");
        private static Game HaloCE = new Game("Halo: CE");
        private static Game Halo2 = new Game("Halo 2");
        private static Game Halo3 = new Game("Halo 3");
        private static Game Halo3ODST = new Game("Halo 3: ODST");
        private static Game Halo4 = new Game("Halo 4");

        private static readonly Dictionary<string, Game> gameMappings = new Dictionary<string, Game>
        {
            { "CrossGame", new Game("Cross Game") },
            { "Cross Game", CrossGame },
            { "HaloReach", HaloReach },
            { "Halo: Reach", HaloReach },
            { "HaloCombatEvolved", HaloCE },
            { "Halo: CE", HaloCE },
            { "Halo CE", HaloCE },
            { "Halo2", Halo2 },
            { "Halo 2", Halo2 },
            { "Halo 2 MP", Halo2 },
            { "Halo 2A MP", Halo2 },
            { "Halo3", Halo3 },
            { "Halo 3", Halo3 },
            { "Halo3Odst", Halo3ODST },
            { "H3: ODST", Halo3ODST },
            { "Halo4", Halo4 },
            { "Halo 4", Halo4 },
        };

        public static Game Get(string name)
        {
            return gameMappings[name];
        }

        public static bool ContainsKey(string key)
        {
            return gameMappings.ContainsKey(key);
        }
    }
}
