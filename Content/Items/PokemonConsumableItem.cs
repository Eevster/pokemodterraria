
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;

namespace Pokemod.Content.Items
{
	public abstract class PokemonConsumableItem : ModItem
	{
		public override void SetStaticDefaults() {
			// The text shown below some item names is called a tooltip. Tooltips are defined in the localization files. See en-US.hjson.

			// How many items are needed in order to research duplication of this item in Journey mode. See https://terraria.wiki.gg/wiki/Journey_Mode#Research for a list of commonly used research amounts depending on item type. This defaults to 1, which is what most items will use, so you can omit this for most ModItems.
			Item.ResearchUnlockCount = 100;

			// This item is a custom currency (registered in ExampleMod), so you might want to make it give "coin luck" to the player when thrown into shimmer. See https://terraria.wiki.gg/wiki/Luck#Coins
			// However, since this item is also used in other shimmer related examples, it's commented out to avoid the item disappearing
			//ItemID.Sets.CoinLuckValue[Type] = Item.value;
		}

        public override bool? UseItem(Player player)
        {
			if(player.whoAmI == Main.myPlayer){
				foreach(Projectile proj in Main.projectile){
					if(proj.owner == player.whoAmI){
						if(proj.ModProjectile != null){
							if(proj.active){
								if(proj.ModProjectile.GetType().IsSubclassOf(typeof(PokemonPetProjectile))){
									Vector2 mousePosition = Main.MouseWorld;
									if(Collision.CheckAABBvAABBCollision(proj.Hitbox.TopLeft(), proj.Hitbox.Size(), mousePosition - new Vector2(1f,1f), new Vector2(2f,2f))){
										OnItemUse(proj);
										return true;
									}
								}
							}
						}
					}
				}
			}
			Item.consumable = false;
            return false;
        }

        public virtual void OnItemUse(Projectile proj){

		}
	}
}
