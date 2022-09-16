using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace TutorialMod.Content.Items.Armours.Vanity
{
    [AutoloadEquip(EquipType.Body)]
    internal class TutorialVanityShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 14;

            Item.vanity = true;
        }
    }
}
