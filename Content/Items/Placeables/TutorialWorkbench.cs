using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace TutorialMod.Content.Items.Placeables
{
    internal class TutorialWorkbench : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.useStyle = ItemUseStyleID.Swing;

            Item.autoReuse = true;
            Item.useTurn = true;

            Item.maxStack = 999;
            Item.consumable = true;

            Item.createTile = ModContent.TileType<Tiles.TutorialWorkbench>();
            Item.placeStyle = 0;
        }
    }
}
