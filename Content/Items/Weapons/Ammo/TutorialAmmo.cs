using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using TutorialMod.Content.Projectiles.Weapons;

namespace TutorialMod.Content.Items.Weapons.Ammo
{
    internal class TutorialAmmo : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.width = 8;
            Item.height = 8;

            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1.25f;

            Item.maxStack = 999;
            Item.consumable = true;

            Item.ammo = AmmoID.Bullet; // Adds this item to the Bullet Ammo Type
            Item.shoot = ModContent.ProjectileType<TutorialAmmoProjectile>();
        }
    }
}
