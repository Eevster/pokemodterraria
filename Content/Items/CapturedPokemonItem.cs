using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Content.Items
{
	public class CapturedPokemonItem : ModItem
	{
        public string PokemonName;
		public bool Shiny;

		public override string Texture => "Pokemod/Content/Items/PokeballItem";

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish);
			Item.useTime = 30;
			Item.useAnimation = 30;
            Item.width = 14;
            Item.height = 14;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.None;
			Item.buffType = 0;
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				player.AddBuff(Item.buffType, 3600);
			}
   			return true;
		}

		public override ModItem Clone(Item item) {
			CapturedPokemonItem clone = (CapturedPokemonItem)base.Clone(item);
			clone.PokemonName = (string)PokemonName?.Clone(); // note the ? here is important, colors may be null if spawned from other mods which don't call OnCreate
			return clone;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (PokemonName == null) //colors may be null if spawned from other mods which don't call OnCreate
				return;

			foreach (TooltipLine line in tooltips) {
				if (line.Mod == "Terraria" && line.Name == "ItemName") {
					line.Text = PokemonName;
					if(Shiny) line.OverrideColor = Main.DiscoColor;
				}
			}
			/*TooltipLine tooltipLine = new TooltipLine(Mod, "EntityName", PokemonName);
            tooltips.Add(tooltipLine);*/
		}

        public void SetPokemonData(string PokemonName, bool Shiny){
            this.PokemonName = PokemonName;
			this.Shiny = Shiny;
			
			SetPetInfo();
        }

        public override void UpdateInventory(Player player)
        {
			SetPetInfo();
            base.UpdateInventory(player);
        }

        public override void UpdateEquip(Player player)
        {
			SetPetInfo();
            base.UpdateEquip(player);
        }

        private void SetPetInfo(){
			if(PokemonName != null && PokemonName != ""){
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;
			}
		}

        // NOTE: The tag instance provided here is always empty by default.
        // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
        public override void SaveData(TagCompound tag) {
			tag["PokemonName"] = PokemonName;
			tag["Shiny"] = Shiny;
		}

		public override void LoadData(TagCompound tag) {
			PokemonName = tag.GetString("PokemonName");
			Shiny = tag.GetBool("Shiny");
			SetPetInfo();
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if(PokemonName != null && PokemonName != ""){
				Asset<Texture2D> texture = ModContent.Request<Texture2D>("Pokemod/Content/Pets/"+PokemonName+(Shiny?"PetShiny":"Pet")+"/"+PokemonName+(Shiny?"PetItemShiny":"PetItem"));

				spriteBatch.Draw(texture.Value,
					position: position-new Vector2(texture.Value.Width/4, texture.Value.Height/4),
					sourceRectangle: texture.Value.Bounds,
					drawColor,
					rotation: 0f,
					origin: Vector2.Zero,
					scale: 1f,
					SpriteEffects.None,
					layerDepth: 0f);
			}
				
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}