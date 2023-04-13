using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.WorldBuilding;
using TutorialMod.Content.Tiles;
using Terraria.ID;

namespace TutorialMod.Common.Systems.GenPasses
{
    internal class TutorialOreGenPass : GenPass
    {
        public TutorialOreGenPass(string name, float weight) : base(name, weight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spawning Tutorial Ores";

            // TutorialOre
            int maxToSpawn = (int)(Main.maxTilesX * Main.maxTilesY * 6E-05);
            for(int i = 0; i < maxToSpawn; i++)
            {
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurface, Main.maxTilesY - 300);

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 5), ModContent.TileType<TutorialOre>());
            }

            // TutorialRareOre
            maxToSpawn = WorldGen.genRand.Next(100, 250);
            int numSpawned = 0;
            int attempts = 0;
            while(numSpawned < maxToSpawn)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(0, Main.maxTilesY);

                Tile tile = Framing.GetTileSafely(x, y);
                if(tile.TileType == TileID.SnowBlock || tile.TileType == TileID.IceBlock || tile.TileType == TileID.Slush)
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(2, 5), WorldGen.genRand.Next(1, 4), ModContent.TileType<TutorialRareOre>());
                    numSpawned++;
                }

                attempts++;
                if(attempts >= 100000)
                {
                    break;
                }
            }
        }
    }
}
