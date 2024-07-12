using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Pets;
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
	public class CaughtPokemonItem : ModItem
	{
        public string PokemonName;
		public int level;
		public int exp;
		public int expToNextLevel;
		public bool Shiny;
		public string BallType;
		public Projectile proj;

		public override string Texture => "Pokemod/Assets/Textures/Pokeballs/PokeballItem";

		public override void SetDefaults() {
			Item.CloneDefaults(ItemID.ZephyrFish);
			Item.useTime = 30;
			Item.useAnimation = 30;
            Item.width = 14;
            Item.height = 14;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.None;
			Item.buffType = 0;
			Item.noUseGraphic = true;
		}

        public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				if(!player.HasBuff(Item.buffType)){
					player.AddBuff(Item.buffType, 3600);
				}else{
					player.ClearBuff(Item.buffType);
					proj?.Kill();
					proj = null;
				}
			}
   			return true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int projIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			proj = Main.projectile[projIndex];

            return false;
        }

        public override ModItem Clone(Item item) {
			CaughtPokemonItem clone = (CaughtPokemonItem)base.Clone(item);
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
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") {
					line.Text = "Caught in a "+BallType.Replace("Item", "") + "\nLvl "+level+"  Exp: "+(exp-GetExpToLevel(level))+"/"+(expToNextLevel-GetExpToLevel(level));
				}
			}

			/*TooltipLine tooltipLine = new TooltipLine(Mod, "BallType", "Caught in a "+BallType.Replace("Item", ""));
			tooltips.Add(tooltipLine);*/
		}

        public void SetPokemonData(string PokemonName, bool Shiny, string BallType, int level = 5){
            this.PokemonName = PokemonName;
			this.Shiny = Shiny;
			this.BallType = BallType;
			this.level = level;
			this.exp = GetExpToLevel(level); 
			
			SetPetInfo();
        }

        public override void UpdateInventory(Player player)
        {
			if(!player.HasBuff(Item.buffType)){
				proj?.Kill();
				proj = null;
			}
			if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
				if(player.miscEquips[0] != Item){
					proj?.Kill();
					proj = null;
				}
			}
			SetPetInfo();
            base.UpdateInventory(player);
        }

        public override void UpdateEquip(Player player)
        {
			SetPetInfo();
            base.UpdateEquip(player);
        }

        public void SetPetInfo(){
			if(PokemonName != null && PokemonName != ""){
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;

				UpdateLevel();
				GetProjInfo();
			}
		}

		private void GetProjInfo(){
			if(proj != null){
				if(proj.active){
					GetProjExp();
					GetUsedItems();
				}else{
					proj = null;
				}
			}
		}

		private void GetProjExp(){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			AddExp(PokemonProj.GetExpGained());
		}

		private void GetUsedItems(){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			if(PokemonProj.GetRareCandy()){
				exp = expToNextLevel;
			}
		}

		private void GetCanEvolve(){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			string newPokemonName = PokemonProj.GetCanEvolve();
			if(newPokemonName != ""){
				proj.Kill();
				PokemonName = newPokemonName;
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;
			}
		}

		public void AddExp(int amount){
			exp += amount;
			UpdateLevel();
		}

		public void UpdateLevel(){
			if(level == 0){
				level = 1;
			}
			SetExpToNextLevel();
			if(PokemonName != null && PokemonName != "" && level > 0){
				if(exp>= expToNextLevel && level < 100){
					level++;
					if(level < 100){
						SetExpToNextLevel();
					}
					UpdateProjLevel(true);
				}
			}
			UpdateProjLevel();
		}

		private void UpdateProjLevel(bool canEvolve = false){
			if(proj != null){
				if(proj.active){
					if(level > 0){
						PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;
						PokemonProj.SetPokemonLvl(level);
						if(canEvolve){
							PokemonProj.SetCanEvolve();
							GetCanEvolve();
						}
					}
				}else{
					proj = null;
				}
			}
		}

		private void SetExpToNextLevel(){
			expToNextLevel = GetExpToLevel(level+1);
		}

		private int GetExpToLevel(int lvl){
			return (int)Math.Pow(lvl, 3);
		}

        // NOTE: The tag instance provided here is always empty by default.
        // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
        public override void SaveData(TagCompound tag) {
			tag["PokemonName"] = PokemonName;
			tag["Level"] = level;
			tag["Exp"] = exp;
			tag["Shiny"] = Shiny;
			tag["BallType"] = BallType;
		}

		public override void LoadData(TagCompound tag) {
			PokemonName = tag.GetString("PokemonName");
			level = tag.GetInt("Level");
			exp = tag.GetInt("Exp");
			Shiny = tag.GetBool("Shiny");
			BallType = tag.GetString("BallType");
			SetPetInfo();
			SetExpToNextLevel();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			if(BallType != null && BallType != ""){
				Asset<Texture2D> ballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokeballs/"+BallType);

				spriteBatch.Draw(ballTexture.Value,
					position: Item.position-Main.screenPosition,
					sourceRectangle: ballTexture.Value.Bounds,
					lightColor,
					rotation: 0f,
					origin: Vector2.Zero,
					scale: 1f,
					SpriteEffects.None,
					layerDepth: 0f);
			}

            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if(BallType != null && BallType != ""){
				Asset<Texture2D> ballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokeballs/"+BallType);

				spriteBatch.Draw(ballTexture.Value,
					position: position-new Vector2(ballTexture.Value.Width/2, ballTexture.Value.Height/2),
					sourceRectangle: ballTexture.Value.Bounds,
					drawColor,
					rotation: 0f,
					origin: Vector2.Zero,
					scale: 1f,
					SpriteEffects.None,
					layerDepth: 0f);
			}
			if(PokemonName != null && PokemonName != ""){
				Asset<Texture2D> texture = ModContent.Request<Texture2D>("Pokemod/Content/Pets/"+PokemonName+(Shiny?"PetShiny":"Pet")+"/"+PokemonName+(Shiny?"PetBuffShiny":"PetBuff"));

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