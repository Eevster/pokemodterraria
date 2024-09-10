using Pokemod.Content;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using System;

namespace Pokemod.Content.Items
{
	public class UltraPortalItem : ModItem
	{
		public override void SetStaticDefaults() {
			// The text shown below some item names is called a tooltip. Tooltips are defined in the localization files. See en-US.hjson.

			// How many items are needed in order to research duplication of this item in Journey mode. See https://terraria.wiki.gg/wiki/Journey_Mode#Research for a list of commonly used research amounts depending on item type. This defaults to 1, which is what most items will use, so you can omit this for most ModItems.
			Item.ResearchUnlockCount = 100;

			// This item is a custom currency (registered in ExampleMod), so you might want to make it give "coin luck" to the player when thrown into shimmer. See https://terraria.wiki.gg/wiki/Luck#Coins
			// However, since this item is also used in other shimmer related examples, it's commented out to avoid the item disappearing
			//ItemID.Sets.CoinLuckValue[Type] = Item.value;
		}

		public override void SetDefaults() {
			Item.width = 20; // The item texture's width
			Item.height = 20; // The item texture's height
			Item.useTime = 20;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            // Only allow use if right-clicked
            return player.altFunctionUse == 2;
        }

		public override bool? UseItem(Player player)
		{
    // Check if this is the client
    if (!Main.dedServ)
    {
        // Check if this is the right-click event
        if (player.altFunctionUse == 2)
        {
            // Call the SubworldSystem.Enter<T>() method here
            // Replace T with your desired type
            Random rnd = new Random();
            int ultraSpaceType = rnd.Next(1, 3);
            if (ultraSpaceType == 1) {
                bool enteredSubworld = SubworldSystem.Enter<IceUltraSpaceSubworld>();
                if (enteredSubworld)
            {
                Main.NewText("You entered the Ultra Space!");
                // Consume the item if the subworld entrance is successful
                player.ConsumeItem(Item.type);
            }
            else
            {
                Main.NewText("Failed to enter the Ultra Space. Make sure you're in the right environment.");
            }

            // Return true if the subworld was entered successfully, otherwise return false
            return enteredSubworld;
            }
             if (ultraSpaceType == 2) {
                bool enteredSubworld = SubworldSystem.Enter<JungleUltraSpaceSubworld>();
                if (enteredSubworld)
            {
                Main.NewText("You entered the Jungle Ultra Space!");
                // Consume the item if the subworld entrance is successful
                player.ConsumeItem(Item.type);
            }
            else
            {
                Main.NewText("Failed to enter the Ultra Space. Make sure you're in the right environment.");
            }

            // Return true if the subworld was entered successfully, otherwise return false
            return enteredSubworld;
            }
            
        }
    }
    return base.UseItem(player);
}
	}
}