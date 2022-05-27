using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TutorialMod.Content.Projectiles.Weapons;

namespace TutorialMod.Content.Items.Weapons
{
    internal class TutorialWand : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.DamageType = DamageClass.Magic;
            Item.noMelee = true;
            Item.mana = 8;
            Item.damage = 24;
            Item.knockBack = 3.2f;

            Item.useTime = 20;
            Item.useAnimation = 15;

            Item.UseSound = SoundID.Item71;

            Item.shoot = ModContent.ProjectileType<TutorialWandProjectile>();
            Item.shootSpeed = 1f;
        }
    }
}
