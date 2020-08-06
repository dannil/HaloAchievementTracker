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
        private static readonly Game HaloReach = new Game("Halo: Reach");
        private static readonly Game HaloCE = new Game("Halo CE");
        private static readonly Game Halo2 = new Game("Halo 2");
        private static readonly Game Halo3 = new Game("Halo 3");
        private static readonly Game Halo3ODST = new Game("Halo 3: ODST");
        private static readonly Game Halo4 = new Game("Halo 4");

        private static readonly Dictionary<string, Game> gameMappings = new Dictionary<string, Game>
        {
            { "CrossGame", CrossGame },
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
            { "Halo 3: ODST", Halo3ODST },
            { "H3: ODST", Halo3ODST },
            { "Halo4", Halo4 },
            { "Halo 4", Halo4 },
        };

        public static Game Get(string name)
        {
            if (!gameMappings.ContainsKey(name))
            {
                throw new ArgumentException();
            }
            return gameMappings[name];
        }
    }
}
