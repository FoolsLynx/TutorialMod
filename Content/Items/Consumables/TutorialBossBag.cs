using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TutorialMod.Content.Items.Armours.Vanity;
using TutorialMod.Content.Items.Placeables;
using TutorialMod.Content.Items.Weapons;
using TutorialMod.Content.NPCs.Bosses;

namespace TutorialMod.Content.Items.Consumables
{
    public class TutorialBossBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.BossBag[Type] = true;
            ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;

            Item.ResearchUnlockCount = 3;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;

            Item.maxStack = Item.CommonMaxStack;
            Item.consumable = true;

            Item.rare = ItemRarityID.Purple;
            Item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            // Drop Tutorial Bar 100% of Time - 2 - 5 bars
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialBar>(), 1, 2, 5));

            // Drop Rare Bar 20% of Time - 2 - 5 bars
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialRareBar>(), 5, 2, 5));

            // If hardMode
            if (Main.hardMode)
            {
                // 25% chance to drop summon item in hardmode
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialSummonStaff>(), 4));
            }

            // Add Expert Only items
            itemLoot.Add(ItemDropRule.NotScalingWithLuck(ModContent.ItemType<TutorialVanityShirt>(), 3));

            // Add Money
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<TutorialBoss>()));
        }
    }
}
