using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;

namespace TutorialMod.Content.Items.Armours
{
    [AutoloadEquip(EquipType.Head)]
    internal class TutorialHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            //ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Dont draw head
            //ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Example: Wizards Hat
            //ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Example: Masks
            //ArmorIDs.Head.Sets.DrawBackHair[Item.headSlot] = true;
            ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;

            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;

            Item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<TutorialBreastplate>() && legs.type == ModContent.ItemType<TutorialLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.TutorialMod.ItemSetBonus.TutorialSet");
            
            player.moveSpeed += 0.10f;
        }
    }
}
