
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;

namespace Pokemod.Content.Items.Consumables
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

        /*public override void HoldItem(Player player)
        {
			if(player.whoAmI == Main.myPlayer){
				if(Main.HoverItem.ModItem is CaughtPokemonItem){
					if(Main.mouseRight && player.ItemTimeIsZero){
						Item pokeItem = player.inventory[player.FindItem(Main.HoverItem.netID)];
						if(pokeItem.ModItem is CaughtPokemonItem item){
							if(OnItemInvUse(item)){
								Main.NewText("Use");
								Item.stack--;
								if(Item.stack <= 0){
									Item.TurnToAir();
								}
								player.itemTime = 20;
							}
						}
					}
				}
			}
            base.HoldItem(player);
        }*/

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
            return true;
        }

        public virtual bool OnItemUse(Projectile proj){
			return false;
		}

		public virtual bool OnItemInvUse(CaughtPokemonItem item, Player player){
			return false;
		}

		public void ReduceStack(Player player, int type){
			if(player != null){
				if(player.whoAmI == Main.myPlayer){
					if(Main.mouseItem != null){
						if(Main.mouseItem?.ModItem?.Type == type){
							Main.mouseItem.stack--;
							if(Main.mouseItem.IsAir){
								Main.mouseItem.TurnToAir();
							}
						}
					}else{
						Item.consumable = true;
						Item.stack--;
						if(Item.IsAir){
							Item.TurnToAir();
						}
					}
				}
			}
		}
	}
}
