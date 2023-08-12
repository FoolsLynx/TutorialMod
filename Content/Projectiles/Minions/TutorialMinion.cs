using Microsoft.Xna.Framework;
using rail;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TutorialMod.Content.Buffs;

namespace TutorialMod.Content.Projectiles.Minions
{
    public class TutorialMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;

            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 28;

            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if(owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<TutorialMinionBuff>());
                return;
            }

            if(owner.HasBuff(ModContent.BuffType<TutorialMinionBuff>())) {
                Projectile.timeLeft = 2;
            }

            AIGeneral(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            AISearchForTarget(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            AIMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);

            AIUpdateAnimation();
        }

        private void AIGeneral(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 48f;

            float minionPositionOffset = (10 + Projectile.minionPos * 40) * -owner.direction;
            idlePosition.X += minionPositionOffset;

            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if(Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            float overlapVelocity = 0.04f;

            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if(
                    i != Projectile.whoAmI && 
                    other.active && 
                    other.owner == Projectile.owner && 
                    Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width
                )
                {
                    if(Projectile.position.X < other.position.X )
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    } else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if(Projectile.position.Y < other.position.Y)
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    } else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }
   
        private void AISearchForTarget(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if(owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);
                if(between < 1200f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if(!foundTarget)
            {
                for(int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if(npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

                        bool closeThroughWall = between < 100f;

                        if(((closest && inRange) || ! foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }

            //Projectile.friendly = foundTarget;
        }

        private void AIMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            float speed = 8f;
            float inertia = 20f;

            if(foundTarget)
            {
                if(distanceFromTarget > 40f)
                {
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
                return;
            }

            if(distanceToIdlePosition > 600f)
            {
                speed = 12f;
                inertia = 60f;
            } else
            {
                speed = 4f;
                inertia = 80f;
            }

            if(distanceToIdlePosition > 20f)
            {
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;

                Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            } else if(Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity.X = -0.15f;
                Projectile.velocity.Y = -0.05f;
            }
        }

        private void AIUpdateAnimation()
        {
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            int frameSpeed = 5;
            Projectile.frameCounter++;
            if(Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if(Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.65f);
        }
    }
}
