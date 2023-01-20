using Terraria;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod.Content.Tiles
{
    internal class TutorialWorkbench : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true; // Tells Terraria that there is tileobjectdata that is used for rendering
            Main.tileSolidTop[Type] = true; // The tile is solid on top
            Main.tileNoAttach[Type] = true; // Doesn't attach to other tiles
            Main.tileLavaDeath[Type] = true; // This tile is killed by Lava
            Main.tileTable[Type] = true; // This tile acts as a table

            TileID.Sets.DisableSmartCursor[Type] = true; // Disables smart cursor interaction with this tile
            TileID.Sets.IgnoredByNpcStepUp[Type] = true; // Prevents NPCs from standing on top of this tile

            AdjTiles = new int[] { TileID.WorkBenches }; // Tells Terraria this tile is part of the Workbenches Tiles

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1); // Copying the existing style of 2x1
            TileObjectData.newTile.CoordinateHeights = new[] { 18 }; // Telling Terraria that each "cell" is 18 pixels
            TileObjectData.addTile(Type); // Adding the tile type to this style

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

            AddMapEntry(new Microsoft.Xna.Framework.Color(200, 200, 200), CreateMapEntryName());
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int itemType = ModContent.ItemType<Content.Items.Placeables.TutorialWorkbench>();

            Item.NewItem(
                new EntitySource_TileBreak(i, j),
                i * 16,
                j * 16,
                32,
                16,
                itemType
            );
        }
    }
}
