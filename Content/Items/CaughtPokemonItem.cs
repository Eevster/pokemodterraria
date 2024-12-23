using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
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
		public int currentHP;
		public int[] IVs = [0,0,0,0,0,0];
		public int[] EVs = [0,0,0,0,0,0];
		public bool Shiny;
		public string BallType;
		public string variant;
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
			Item.consumable = false;
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write(PokemonName ?? "");
			writer.Write(Shiny);
			writer.Write(variant ?? "");
			writer.Write(BallType ?? "");
			writer.Write((double)level);
			writer.Write((double)exp);
			foreach(int IV in IVs){
				writer.Write((double)IV);
			}
			foreach(int EV in EVs){
				writer.Write((double)EV);
			}
			writer.Write((double)currentHP);
		}

		public override void NetReceive(BinaryReader reader) {
			PokemonName = reader.ReadString();
			Shiny = reader.ReadBoolean();
			variant = reader.ReadString();
			BallType = reader.ReadString();
			level = (int)reader.ReadDouble();
			exp = (int)reader.ReadDouble();
			for(int i = 0; i < IVs.Length; i++){
				IVs[i] = (int)reader.ReadDouble();
			}
			for(int i = 0; i < EVs.Length; i++){
				EVs[i] = (int)reader.ReadDouble();
			}
			currentHP = (int)reader.ReadDouble();
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
			if(player.HasBuff(Item.buffType) && currentHP != 0){
				int projIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, currentHP);
				proj = Main.projectile[projIndex];
			}

            return false;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
			if(player.ItemTimeIsZero){
				if(player.HeldItem.ModItem is PokemonConsumableItem item){
					if(item.OnItemInvUse(this, player)){
						player.itemTime = 20;
					}
				}
			}
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override ModItem Clone(Item item) {
			CaughtPokemonItem clone = (CaughtPokemonItem)base.Clone(item);
			clone.PokemonName = (string)PokemonName?.Clone(); // note the ? here is important, colors may be null if spawned from other mods which don't call OnCreate
			clone.BallType = (string)BallType?.Clone();
			return clone;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
			if (PokemonName == null || PokemonName == "") //colors may be null if spawned from other mods which don't call OnCreate
				return;

			int[] fullStats = GetPokemonStats();
			foreach (TooltipLine line in tooltips) {
				if (line.Mod == "Terraria" && line.Name == "ItemName") {
					line.Text = PokemonName;
					if(Shiny) line.OverrideColor = Main.DiscoColor;
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") {
					line.Text = (variant!=""?("[c/23a462:"+variant+" variant]\n"):"")+"Caught in a "+BallType.Replace("Item", "") + "\n"+
					"[c/ffd51c:Lvl] "+level+"  [c/ffd51c:Exp:] "+(exp-GetExpToLevel(level))+"/"+(expToNextLevel-GetExpToLevel(level))+
					"\n"+(currentHP>=0?"[c/ffd51c:HP:] "+(currentHP>0?(currentHP+"/"+fullStats[0]+" "):"[c/fa3e42:Fainted] "):"")+
					"\n[c/ffd51c:Atk:] "+fullStats[1]+"  [c/ffd51c:Def:] "+fullStats[2]+
					"\n[c/ffd51c:SpAtk:] "+fullStats[3]+"  [c/ffd51c:SpDef:] "+fullStats[4]+"  [c/ffd51c:Vel:] "+fullStats[5];
				}
			}

			/*TooltipLine tooltipLine = new TooltipLine(Mod, "BallType", "Caught in a "+BallType.Replace("Item", ""));
			tooltips.Add(tooltipLine);*/
		}

        public void SetPokemonData(string PokemonName, bool Shiny, string BallType, int level = 5, int[] IVs = null, string variant = ""){
            this.PokemonName = PokemonName;
			this.Shiny = Shiny;
			this.BallType = BallType;
			this.level = level;
			this.exp = GetExpToLevel(level);
			if(IVs != null){
				this.IVs = IVs;
			}
			currentHP = GetPokemonStats()[0];
			this.variant = variant;
			
			SetPetInfo();
        }

		public int[] GetPokemonStats(){
			return PokemonNPCData.CalcAllStats(level, PokemonNPCData.pokemonStats[PokemonName], IVs, EVs);
		}

        public override void UpdateInventory(Player player)
        {
			if(PokemonName != null && PokemonName != ""){
				GetProjInfo(player);
			}
			if(proj != null){
				if(((!player.HasBuff(Item.buffType) && Item.buffType != 0) || currentHP == 0) && proj.active){
					player.ClearBuff(Item.buffType);
					proj?.Kill();
					proj = null;
				}
			}
			if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
				if(player.miscEquips[0] != Item){
					proj?.Kill();
					proj = null;
				}
			}
			SetPetInfo(player);
            base.UpdateInventory(player);
        }

        public override void HoldItem(Player player)
        {
			if(PokemonName != null && PokemonName != ""){
				GetProjInfo(player);
				if(Main.mouseItem != null){
					if(Main.mouseItem.ModItem is CaughtPokemonItem ball){
						ball.GetProjInfo(player);
					}
				}
			}
			if(proj != null){
				if(((!player.HasBuff(Item.buffType) && Item.buffType != 0) || currentHP == 0) && proj.active){
					player.ClearBuff(Item.buffType);
					proj?.Kill();
					proj = null;
				}
			}
			if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
				if(player.miscEquips[0] != Item){
					proj?.Kill();
					proj = null;
				}
			}
			SetPetInfo(player);
			if(Main.mouseItem != null){
				if(Main.mouseItem.ModItem is CaughtPokemonItem ball){
					ball.SetPetInfo(player);
				}
			}

            base.HoldItem(player);
        }

        public void SetPetInfo(Player player = null){
			if(PokemonName != null && PokemonName != ""){
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;

				UpdateLevel(player);
				GetProjInfo(player);
				if(currentHP == 0){
					if(Item.buffType != 0 && player != null){
						player.ClearBuff(Item.buffType);
					}
					Item.shoot = ProjectileID.None;
					Item.buffType = 0;
				}
				if(variant != null){
					if(variant != ""){
						if(proj != null){
							if(proj.active){
								PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
								PokemonProj.variant = variant;
							}
						}
					}
				}
			}
		}

		private void GetProjInfo(Player player = null){
			if(proj != null){
				if(proj.active){
					GetProjExp(player);
					GetUsedItems(player);
					GetProjHP();
				}else{
					proj = null;
				}
			}
		}

		private void GetProjExp(Player player = null){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
			if(PokemonProj != null){
				AddExp(PokemonProj.GetExpGained(), player);
			}
		}

		private void GetProjHP(){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
			if(PokemonProj != null){
				currentHP = PokemonProj.currentHp;
			}
		}

		private void GetUsedItems(Player player = null){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
			if(PokemonProj != null){
				if(PokemonProj.itemEvolve){
					GetCanEvolve(player);
				}
				if(PokemonProj.GetRareCandy()){
					exp = expToNextLevel;
				}
			}
		}

		private void GetCanEvolve(Player player = null){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;
			string newPokemonName = PokemonProj.GetCanEvolve();
			if(newPokemonName != ""){
				Vector2 pokePosition = proj.Center - new Vector2(0,(player.height-proj.height)/2);
				proj.Kill();
				if(player != null){
					player.ClearBuff(Item.buffType);
				}
				PokemonName = newPokemonName;
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;
				if(player != null){
					player.AddBuff(Item.buffType, 3600);
					int projIndex = Projectile.NewProjectile(Item.GetSource_FromThis(), pokePosition, Vector2.Zero, Item.shoot, 0, 0, player.whoAmI, currentHP);
					proj = Main.projectile[projIndex];
				}
			}
		}

		public void AddExp(int amount, Player player = null){
			exp += amount;
			UpdateLevel(player);
		}

		public void UpdateLevel(Player player = null){
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
					UpdateProjLevel(true, player);
				}
				if(level >= 100 && exp> expToNextLevel){
					exp = expToNextLevel;
				}
			}
			UpdateProjLevel(false, player);
		}

		private void UpdateProjLevel(bool canEvolve = false, Player player = null){
			if(proj != null){
				if(proj.active){
					if(level > 0){
						PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;
						PokemonProj.SetPokemonLvl(level, IVs, EVs);
						if(canEvolve){
							PokemonProj.SetCanEvolve();
						}
						GetCanEvolve(player);
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
			tag["Variant"] = variant;
			tag["BallType"] = BallType;
			tag["CurrentHP"] = currentHP;
			tag["IVs"] = IVs.ToList();
			tag["EVs"] = EVs.ToList();
		}

		public override void LoadData(TagCompound tag) {
			PokemonName = tag.GetString("PokemonName");
			level = tag.GetInt("Level");
			exp = tag.GetInt("Exp");
			Shiny = tag.GetBool("Shiny");
			variant = tag.GetString("Variant");
			BallType = tag.GetString("BallType");
			currentHP = tag.GetInt("CurrentHP");
			if(tag.ContainsKey("IVs")){
				IVs = [.. tag.GetList<int>("IVs")];
			}
			if(tag.ContainsKey("EVs")){
				EVs = [.. tag.GetList<int>("EVs")];
			}

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
					scale: scale,
					SpriteEffects.None,
					layerDepth: 0f);
			}

			if(proj != null){
				if(proj.active){
					GetProjHP();
					proj.Kill();
				}else{
					proj = null;
				}
			}

            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if(BallType != null && BallType != ""){
				Asset<Texture2D> ballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokeballs/"+BallType+"Big");

				spriteBatch.Draw(ballTexture.Value,
					position: position-scale*new Vector2(ballTexture.Value.Width/2, ballTexture.Value.Height/2),
					sourceRectangle: ballTexture.Value.Bounds,
					drawColor,
					rotation: 0f,
					origin: Vector2.Zero,
					scale: scale,
					SpriteEffects.None,
					layerDepth: 0f);
			}
			if(PokemonName != null && PokemonName != ""){
				Asset<Texture2D> texture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/"+PokemonName+(Shiny?"Shiny":""));

				spriteBatch.Draw(texture.Value,
					position: position-scale*new Vector2(texture.Value.Width/4, texture.Value.Height/4),
					sourceRectangle: texture.Value.Bounds,
					drawColor,
					rotation: 0f,
					origin: Vector2.Zero,
					scale: scale,
					SpriteEffects.None,
					layerDepth: 0f);
			}
				
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}