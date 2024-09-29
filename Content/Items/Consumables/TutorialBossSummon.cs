using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TutorialMod.Content.Items.Placeables;
using TutorialMod.Content.NPCs.Bosses;

namespace TutorialMod.Content.Items.Consumables
{
    public class TutorialBossSummon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 20;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
        }

        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossSpawners;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<TutorialBoss>());
        }

        public override bool? UseItem(Player player)
        {
            if(player.whoAmI == Main.myPlayer)
            {
                // Play Boss Roar
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                // Get NPC Type
                int type = ModContent.NPCType<TutorialBoss>();

                // Spawn on Single Player
                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                    return true;
                }

                // Send Data to Spawn
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            }
            return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<TutorialBar>(20).AddTile(TileID.DemonAltar).Register();
        }
    }
}
