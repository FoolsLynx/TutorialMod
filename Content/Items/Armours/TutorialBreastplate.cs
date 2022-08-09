using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TutorialMod.Content.Items.Armours
{
    [AutoloadEquip(EquipType.Body)]
    internal class TutorialBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;

            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;

            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            // Give Buff Immunity
            player.buffImmune[BuffID.Frozen] = true;

            // Increase Health of Mana
            player.statLifeMax2 += 20;
            player.statManaMax2 += 20;

            // Increase Max Minions
            player.maxMinions += 2;

            // Increase Movement Speed
            player.moveSpeed += 0.07f;
        }
    }
}
