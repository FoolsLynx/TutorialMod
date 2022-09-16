using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TutorialMod.Content.Items.Accessories
{
    internal class TutorialStatAccessory : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Generic) += 0.25f; // Increase all damage by 25% for all weapons

            player.GetCritChance(DamageClass.Melee) += 20f; // Increases the crit chance

            player.GetKnockback(DamageClass.Melee) += 0.5f; // Increases melee knockback by 50%

            player.GetAttackSpeed(DamageClass.Ranged) += 0.5f; // Increases ranged attack speed by 50%

            player.GetArmorPenetration(DamageClass.Magic) += 5f; // Increases Armour Penetration by 5 points

        }
    }
}
