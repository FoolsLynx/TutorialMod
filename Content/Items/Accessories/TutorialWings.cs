using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Terraria.DataStructures;

namespace TutorialMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    internal class TutorialWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(125, 4f, 1.25f);
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.accessory = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1.55f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1.15f;
            maxAscentMultiplier = 2.2f;
            constantAscend = 0.1f;
        }
    }
}
