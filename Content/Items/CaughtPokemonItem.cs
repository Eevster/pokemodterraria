using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
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
		public string OriginalTrainerID;
		public string CurrentTrainerID;
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
		public int shouldDespawn = 3;
		public bool canShoot = false;
		public Projectile proj;

		public override string Texture => "Pokemod/Assets/Textures/Pokeballs/PokeballItem";

        public override void SetDefaults() {
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
			Item.maxStack = 1;
		}

        public override bool CanReforge()
        {
            return false;
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
				if(proj == null){
					canShoot = true;
				}else{
					canShoot = false;
					proj?.Kill();
					proj = null;
				}
			}
   			return true;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			if(canShoot && currentHP != 0){
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
					line.Text = Language.GetTextValue("Mods.Pokemod.NPCs."+PokemonName+"CritterNPC.DisplayName");
					if(Shiny) line.OverrideColor = Main.DiscoColor;
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") {
					line.Text = (variant != null?(variant!=""?("[c/23a462:"+variant+" variant]\n"):""):"")+
					((OriginalTrainerID != null && OriginalTrainerID != "")?Language.GetText("Mods.Pokemod.PokemonInfo.CaughtInBy").WithFormatArgs(BallType.Replace("Item", ""), OriginalTrainerID.Remove(OriginalTrainerID.Length-9)).Value:Language.GetText("Mods.Pokemod.PokemonInfo.CaughtIn").WithFormatArgs(BallType.Replace("Item", "")).Value) + "\n"+
					"[c/ffd51c:Lvl] "+level+"  [c/ffd51c:Exp:] "+(exp-GetExpToLevel(level))+"/"+(expToNextLevel-GetExpToLevel(level))+
					"\n"+(currentHP>=0?"[c/ffd51c:HP:] "+(currentHP>0?(currentHP+"/"+fullStats[0]+" "):"[c/fa3e42:"+Language.GetTextValue("Mods.Pokemod.PokemonInfo.Fainted")+"] "):"")+
					"\n[c/ffd51c:Atk:] "+fullStats[1]+"  [c/ffd51c:Def:] "+fullStats[2]+
					"\n[c/ffd51c:SpAtk:] "+fullStats[3]+"  [c/ffd51c:SpDef:] "+fullStats[4]+"  [c/ffd51c:Speed:] "+fullStats[5];
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
			if(OriginalTrainerID == null || OriginalTrainerID == "") OriginalTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;
			if(CurrentTrainerID == null || CurrentTrainerID == "") CurrentTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;

			shouldDespawn = 3;

			if(PokemonName != null && PokemonName != ""){
				GetProjInfo(player);
			}
			if(proj != null){
				if(currentHP == 0 && proj.active){
					proj?.Kill();
					proj = null;
				}
			}

			SetPetInfo(player);

            base.UpdateInventory(player);
        }

        public override void HoldItem(Player player)
        {
			if(OriginalTrainerID == null || OriginalTrainerID == "") OriginalTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;
			if(CurrentTrainerID == null || CurrentTrainerID == "") CurrentTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;

			shouldDespawn = 3;

			if(PokemonName != null && PokemonName != ""){
				GetProjInfo(player);
				if(Main.mouseItem != null){
					if(Main.mouseItem.ModItem is CaughtPokemonItem ball){
						ball.GetProjInfo(player);
					}
				}
			}

			if(proj != null){
				if(currentHP == 0 && proj.active){
					proj?.Kill();
					proj = null;
				}
			}
			if(Main.mouseItem != null){
				if(Main.mouseItem.ModItem is CaughtPokemonItem ball){
					if(ball.proj != null){
						if(ball.currentHP == 0 && ball.proj.active){
							ball.proj?.Kill();
							ball.proj = null;
						}
					}
				}
			}

			SetPetInfo(player);
			if(Main.mouseItem != null){
				if(Main.mouseItem.ModItem is CaughtPokemonItem ball){
					ball.SetPetInfo(player);
					ball.shouldDespawn = 3;
				}
			}

            base.HoldItem(player);
        }

        public void SetPetInfo(Player player = null){
			if(PokemonName != null && PokemonName != ""){
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				//Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;

				UpdateLevel(player);
				GetProjInfo(player);
				
				if(currentHP == 0){
					Item.shoot = ProjectileID.None;
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
				PokemonName = newPokemonName;
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				if(player != null){
					int projIndex = Projectile.NewProjectile(Item.GetSource_FromThis(), pokePosition, Vector2.Zero, Item.shoot, 0, 0, player.whoAmI, currentHP);
					proj = Main.projectile[projIndex];
				}
			}
		}
		private bool GetEvolutionRestricted(Player player = null){
			if(player != null){
				if(player.GetModPlayer<PokemonPlayer>().HasEverstone>0){
					if(Item.favorited) return true;
					if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
						if(player.miscEquips[0].ModItem is CaughtPokemonItem){
							if(player.miscEquips[0].ModItem == Item.ModItem){
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		private void GetCanMegaEvolve(Player player = null){
			PokemonPetProjectile PokemonProj = (PokemonPetProjectile)proj.ModProjectile;

			string newPokemonName = PokemonProj.GetCanMegaEvolve();
			if(newPokemonName != ""){
				Vector2 pokePosition = proj.Center - new Vector2(0,(player.height-proj.height)/2);
				proj.Kill();
				PokemonName = newPokemonName;
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				if(player != null){
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
					UpdateProjLevel(!GetEvolutionRestricted(player), player);
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
						PokemonProj.SetCanMegaEvolve();
						GetCanEvolve(player);
						GetCanMegaEvolve(player);
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
			tag["OriginalTrainerID"] = OriginalTrainerID;
			tag["CurrentTrainerID"] = CurrentTrainerID;
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
			OriginalTrainerID = tag.GetString("OriginalTrainerID");
			CurrentTrainerID = tag.GetString("CurrentTrainerID");
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

        public override void PostUpdate()
        {
			DespawnPokemon();
            base.PostUpdate();
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

			if(!Main.gamePaused){
				if(--shouldDespawn<=0){
					DespawnPokemon();
				}
			}else{
				shouldDespawn = 3;
			}
				
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

		public void DespawnPokemon(){
			if(proj != null){
				if(proj.active){
					GetProjHP();
					proj.Kill();
				}else{
					proj = null;
				}
			}
		}
    }
}