using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TutorialMod.Content.Items.Weapons
{
    internal class TutorialSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tutorial Sword");
            Tooltip.SetDefault("This is a modded Broadsword");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Hitbox
            Item.width = 32;
            Item.height = 32;

            // Use and Animation Style
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;
            
            // Damage Values
            Item.DamageType = DamageClass.Melee;
            Item.damage = 20;
            Item.knockBack = 3.5f;
            Item.crit = 5;

            // Misc
            Item.value = Item.buyPrice(silver: 80, copper: 50);
            Item.rare = ItemRarityID.Blue;

            // Sound
            Item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<TutorialItem>(), 8) // AddIngredient takes ItemID, then Quantity
                .AddTile(TileID.Anvils) // AddTile takes the TileID
                .Register(); // Register registers the item
        }
    }
}
