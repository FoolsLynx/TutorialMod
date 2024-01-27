using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using TutorialMod.Content.Items.Placeables;

namespace TutorialMod.Content.NPCs.Enemies
{
    internal class TutorialZombie : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

            NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.Skeleton;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Zombie);
            NPC.damage = 14;
            NPC.defense = 6;
            NPC.lifeMax = 200;
            NPC.value = 60f;
            NPC.knockBackResist = 0.5f;

            // NPC.aiStyle = 3;

            AIType = NPCID.Zombie;
            AnimationType = NPCID.Zombie;

            Banner = Item.NPCtoBanner(NPCID.Zombie);
            BannerItem = Item.BannerToItem(Banner);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var zombieDropRule = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Zombie, false);
            foreach(var rule in zombieDropRule)
            {
                npcLoot.Add(rule);
            }

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialBar>(), 10, 1, 3));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldNightMonster.Chance - 0.65f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,
                new FlavorTextBestiaryInfoElement("This is a zombie that has been modded into the game.")
            });
        }
    }
}
