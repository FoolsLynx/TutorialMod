using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TutorialMod.Content.Projectiles.Enemies.Boss
{
    internal class TutorialBossProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 48;

            Projectile.aiStyle = 0;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.hostile = true;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Player player = Main.player[(int)Projectile.ai[0]];
            if(player.dead || !player.active)
            {
                return;
            }

            Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 6f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
