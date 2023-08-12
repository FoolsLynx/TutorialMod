using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TutorialMod.Content.Buffs;
using TutorialMod.Content.Projectiles.Minions;

namespace TutorialMod.Content.Items.Weapons
{
    public class TutorialSummonStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;

            Item.DamageType = DamageClass.Summon;
            Item.damage = 30;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.noMelee = true;

            Item.value = Item.buyPrice(gold: 1);

            Item.shoot = ModContent.ProjectileType<TutorialMinion>();
            Item.buffType = ModContent.BuffType<TutorialMinionBuff>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, Main.myPlayer);
            projectile.originalDamage = Item.damage;

            return false;
        }
    }
}
