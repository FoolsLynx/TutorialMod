using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;

namespace TutorialMod.Content.Items
{
    internal class TutorialItem : ModItem
    {
        // SetStaticDefaults sets values that cannot be changed during gameplay. These include things such as:
        // Display Name, Tooltips, Creative Menu and many other things
        public override void SetStaticDefaults()
        {
            // This accesses the creative catalog
            // Setting the research number to 100 before it can be fully accessed
            //CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
            Item.ResearchUnlockCount = 100;
            
        }

        // SetDefaults sets the values for the item that in some cases cam be changed during gameplay.
        public override void SetDefaults()
        {
            Item.width = 16;    // Hitbox Width from Bottom Center
            Item.height = 16;   // Hitbox Height form Bottom Center

            Item.value = Item.buyPrice(copper: 5); // Value of the Item 120 = Silver: 1, Cooper: 20
            Item.maxStack = 999;
        }

    }
}
