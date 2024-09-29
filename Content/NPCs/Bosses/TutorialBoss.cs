using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.ModLoader;
using TutorialMod.Common.Systems;
using TutorialMod.Content.Items.Placeables;
using TutorialMod.Content.Items.Weapons;
using TutorialMod.Content.Projectiles.Enemies.Boss;

namespace TutorialMod.Content.NPCs.Bosses
{
    [AutoloadBossHead]
    public class TutorialBoss : ModNPC
    {
        private int state
        {
            get => (int)NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        private int subState
        {
            get => (int)NPC.ai[1];
            set => NPC.ai[1] = value;
        }

        private float stateTimer
        {
            get => NPC.ai[2];
            set => NPC.ai[2] = value;
        }

        private float stateTimer2
        {
            get => NPC.ai[3];
            set => NPC.ai[3] = value;
        }

        private bool secondPhase => state == 1;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;

            NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers drawMods = new()
            {
                PortraitScale = 0.6f,
                PortraitPositionYOverride = 0f
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawMods);
        }

        public override void SetDefaults()
        {
            // Hitbox
            NPC.width = 64;
            NPC.height = 128;

            // Damage and Defence
            NPC.damage = 32;
            NPC.defense = 15;

            // Maximum Health
            NPC.lifeMax = 7500;

            // Knockback Resistance
            NPC.knockBackResist = 0f;

            // Sounds
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            // Collision
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            // Boss Settings
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f;

            // AI
            NPC.aiStyle = -1;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Drop Tutorial Bar 100% of Time - 2 - 5 bars
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialBar>(), 1, 2, 5));

            // Drop Rare Bar 20% of Time - 2 - 5 bars
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialRareBar>(), 5, 2, 5));

            // If hardMode
            if(Main.hardMode)
            {
                // 25% chance to drop summon item in hardmode
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialSummonStaff>(), 4));
            }
        }

        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref BossDownedSystem.downedTutorialBoss, -1);
        }

        public override void AI()
        {
            // Handle Targetting
            if(NPC.target == 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            // Set Player Target
            Player player = Main.player[NPC.target];

            // Despawn Behaviour
            if(player.dead || !player.active)
            {
                NPC.velocity.Y -= 0.04f;
                NPC.EncourageDespawn(10);
                return;
            }

            // Handle State Machine
            switch(state)
            {
                case 0:
                    HandleFirstState(player);
                    break;
                case 1:
                    HandleSecondState(player);
                    break;
            }
        }

        private void HandleFirstState(Player player)
        {
            // Move towards player
            if(subState == 0)
            {
                // Setup Speed
                float baseMoveSpeed = 5f;
                float accelerationSpeed = 0.04f;

                // Expert Mode Adjustment
                if(Main.expertMode)
                {
                    baseMoveSpeed = 7f;
                    accelerationSpeed = 0.15f;
                }

                // Move Towards Target
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                // Increase State Timer
                stateTimer += 1f;

                // Check if change substate
                float threshold = 600f;
                if(Main.expertMode)
                {
                    threshold *= 0.3f;
                }

                // Change Substate if Conditions Met
                if(stateTimer >= threshold)
                {
                    subState = 1;
                    stateTimer = 0;
                    stateTimer2 = 0;
                    NPC.netUpdate = true;
                    return;
                }
            }

            // Charge at Player
            else if(subState == 1)
            {
                // Setup Speed
                float baseSpeed = 6f;
                if(Main.expertMode)
                {
                    baseSpeed = 7f;
                }

                // Handle Charge Velocity
                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                // Get Distance 
                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                // Calculate Velocity
                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                // Apply Velocity to NPC
                NPC.velocity = velocity;

                // Move to Post Charge State
                subState = 2;

                // Update Network
                NPC.netUpdate = true;
                if(NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }
            // Post Charge State
            else if(subState == 2)
            {
                // Increase Timer
                stateTimer += 1f;

                // Slow Down the Charge
                if(stateTimer >= 48f)
                {
                    // Slow Velocity
                    NPC.velocity *= 0.98f;

                    // Adjust Based on Difficulty
                    if(Main.expertMode)
                    {
                        NPC.velocity *= 0.985f;
                    }

                    // If Velocity close to 0
                    if (Math.Abs(NPC.velocity.X) < 0.05) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.05) NPC.velocity.Y = 0f;
                }

                // Handle State Changing
                int threshold = 150;
                if(Main.expertMode)
                {
                    threshold = 100;
                }

                // Handle State Change
                if(stateTimer >= threshold)
                {
                    // Increment Second Timer
                    stateTimer2 += 1f;

                    // Reset Main timer
                    stateTimer = 0f;

                    // Reset Target
                    NPC.target = 255;

                    // Change Sub State
                    if(stateTimer2 >= 2f)
                    {
                        subState = 0;
                        stateTimer2 = 0f;
                    } else
                    {
                        subState = 1;
                    }
                }
            }

            // Check Second State Switch
            float lowHealthThreshold = 0.35f;
            if((float)NPC.life < (float)NPC.lifeMax * lowHealthThreshold)
            {
                // Go to Second State
                state = 1;
                subState = 0;
                stateTimer = 0f;
                stateTimer2 = 0f;

                // Handle Network
                NPC.netUpdate = true;
                if(NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }
        }

        private void HandleSecondState(Player player)
        {
            // Move towards player
            if (subState == 0)
            {
                // Setup Speed
                float baseMoveSpeed = 6f;
                float accelerationSpeed = 0.07f;

                // Expert Mode Adjustment
                if (Main.expertMode)
                {
                    baseMoveSpeed = 8f;
                    accelerationSpeed = 0.2f;
                }

                // Move Towards Target
                MoveToTarget(player, baseMoveSpeed, accelerationSpeed, out float distanceToPlayer);

                // Increase State Timer
                stateTimer += 1f;

                // Check if change substate
                float threshold = 400f;
                if (Main.expertMode)
                {
                    threshold *= 0.3f;
                }

                // Change Substate if Conditions Met
                if (stateTimer >= threshold)
                {
                    subState = 1;
                    stateTimer = 0;
                    stateTimer2 = 0;
                    NPC.netUpdate = true;
                    return;
                }

                // Handle Secondary Action
                if(NPC.position.Y + NPC.height < player.Center.Y && distanceToPlayer < 500f)
                {
                    // Increase Secondary Timer
                    if(!player.dead)
                    {
                        stateTimer2 += 1f;
                    }

                    // Handle Projectile Threshold
                    float projThreshold = 120f;
                    if(Main.expertMode)
                    {
                        projThreshold *= 0.4f;
                    }

                    // Handle Shooting Projectile
                    if(stateTimer2 >= projThreshold)
                    {
                        // Reset Timer
                        stateTimer2 = 0;

                        // Set Move Speed
                        float projSpeed = 7f;
                        if(Main.expertMode)
                        {
                            projSpeed = 8.5f;
                        }

                        // Calculate Damage
                        int projDamage = (int)(NPC.damage * .5f);
                        float projKnockback = 3f;
                        if(Main.expertMode)
                        {
                            projDamage += (int)(NPC.damage * .15);
                            projKnockback += .5f;
                        }

                        // Shoot Projectile
                        ShootProjectile(player, ModContent.ProjectileType<TutorialBossProjectile>(), projSpeed, projDamage, projKnockback);
                    }
                }
            }

            // Charge at Player
            else if (subState == 1)
            {
                // Setup Speed
                float baseSpeed = 8f;
                if (Main.expertMode)
                {
                    baseSpeed = 9f;
                }

                // Handle Charge Velocity
                float deltaX = (player.Center.X - NPC.Center.X);
                float deltaY = (player.Center.Y - NPC.Center.Y);

                // Get Distance 
                float distanceToPlayer = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                // Calculate Velocity
                float movementSpeed = baseSpeed / distanceToPlayer;
                Vector2 velocity = new Vector2(deltaX, deltaY) * movementSpeed;

                // Apply Velocity to NPC
                NPC.velocity = velocity;

                // Move to Post Charge State
                subState = 2;

                // Update Network
                NPC.netUpdate = true;
                if (NPC.netSpam > 10)
                {
                    NPC.netSpam = 10;
                }
            }
            // Post Charge State
            else if (subState == 2)
            {
                // Increase Timer
                stateTimer += 1f;

                // Slow Down the Charge
                if (stateTimer >= 40f)
                {
                    // Slow Velocity
                    NPC.velocity *= 0.98f;

                    // Adjust Based on Difficulty
                    if (Main.expertMode)
                    {
                        NPC.velocity *= 0.985f;
                    }

                    // If Velocity close to 0
                    if (Math.Abs(NPC.velocity.X) < 0.05) NPC.velocity.X = 0f;
                    if (Math.Abs(NPC.velocity.Y) < 0.05) NPC.velocity.Y = 0f;
                }

                // Handle State Changing
                int threshold = 120;
                if (Main.expertMode)
                {
                    threshold = 85;
                }

                // Handle State Change
                if (stateTimer >= threshold)
                {
                    // Increment Second Timer
                    stateTimer2 += 1f;

                    // Reset Main timer
                    stateTimer = 0f;

                    // Reset Target
                    NPC.target = 255;

                    // Change Sub State
                    if (stateTimer2 >= 4f)
                    {
                        subState = 0;
                        stateTimer2 = 0f;
                    }
                    else
                    {
                        subState = 1;
                    }
                }
            }
        }

        private void MoveToTarget(Player player, float moveSpeed, float accelerationRate, out float distanceToPlayer)
        {
            // Set Distance to Player
            distanceToPlayer = Vector2.Distance(NPC.Center, player.Center);

            // Set Move Speeds
            float movementSpeed = moveSpeed / distanceToPlayer;

            float targetVelocityX = (player.Center.X - NPC.Center.X) * movementSpeed;
            float targetVelocityY = (player.Center.Y - NPC.Center.Y) * movementSpeed;

            // Apply Acceleration
            if(NPC.velocity.X < targetVelocityX)
            {
                // Increase Velocity by Acceleration
                NPC.velocity.X += accelerationRate;

                // Further increase velocity
                if(NPC.velocity.X < 0f && targetVelocityX > 0f)
                {
                    NPC.velocity.X += accelerationRate;
                }
            }

            if (NPC.velocity.X > targetVelocityX)
            {
                // Increase Velocity by Acceleration
                NPC.velocity.X -= accelerationRate;

                // Further increase velocity
                if (NPC.velocity.X > 0f && targetVelocityX < 0f)
                {
                    NPC.velocity.X -= accelerationRate;
                }
            }

            if (NPC.velocity.Y < targetVelocityY)
            {
                // Increase Velocity by Acceleration
                NPC.velocity.Y += accelerationRate;

                // Further increase velocity
                if (NPC.velocity.Y < 0f && targetVelocityY > 0f)
                {
                    NPC.velocity.Y += accelerationRate;
                }
            }

            if (NPC.velocity.Y > targetVelocityY)
            {
                // Increase Velocity by Acceleration
                NPC.velocity.Y -= accelerationRate;

                // Further increase velocity
                if (NPC.velocity.Y > 0f && targetVelocityY < 0f)
                {
                    NPC.velocity.Y -= accelerationRate;
                }
            }
        }

        private void ShootProjectile(Player player, int type, float speed, int damage, float knockback)
        {
            // Get Target Position
            Vector2 projTarget = new(player.Center.X - NPC.Center.X, player.Center.Y - NPC.Center.Y);
            float projDistance = (float)(projTarget.X * projTarget.X - projTarget.Y * projTarget.Y);
            float projTargetDistance = speed / projDistance;

            // Set Velocity
            Vector2 projVelocity = projTarget * projTargetDistance;

            // Get Spawn Position
            Vector2 projSpawm = NPC.Center + projVelocity * 10f;

            // Handle Network Logic
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                int projectileID = Projectile.NewProjectile(NPC.GetSource_FromAI(), projSpawm, projVelocity, type, damage, knockback);
                if(Main.netMode == NetmodeID.Server && projectileID < 200)
                {
                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, projectileID);
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            // First Phase
            int startFrame = 0;
            int endFrame = 3;

            // Adjust for Second Phase
            if(secondPhase)
            {
                startFrame = 4;
                endFrame = 7;

                // Ensure frame is within range
                if(NPC.frame.Y < startFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }

            // Handle Animation
            int frameSpeed = 5;

            // Increment Frame Counters
            NPC.frameCounter += 0.5f;
            NPC.frameCounter += NPC.velocity.Length() / 10f;

            // Adjust Frame
            if(NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                // Loop back to start
                if(NPC.frame.Y > endFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
    }
}
