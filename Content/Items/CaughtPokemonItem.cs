using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using Pokemod.Common.UI.MoveLearnUI;
using Pokemod.Content.Items.Consumables;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using Pokemod.Content.Projectiles;
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
		public string PokemonNick;
        public int level;
		public int exp;
		public int expToNextLevel;
		public int currentHP;
		public int[] IVs = [0,0,0,0,0,0];
		public int[] EVs = [0,0,0,0,0,0];
		public string[] moves = [];
		private string[] toLearnMoves = [];
		public int moveIndex;
		public bool Shiny;
		public string BallType;
		public string variant;

		public int nature;
		public int happiness;
		public string pokeHeldItem;

		public int shouldDespawn = 10;
		const int despawnTime = 10;
		public bool canShoot = false;
		public Projectile proj;

		private int timer;

		public override string Texture => "Pokemod/Assets/Textures/Pokeballs/PokeballItem";

        public override void SetDefaults() {
			Item.useTime = 30;
			Item.useAnimation = 30;
            Item.width = 14;
            Item.height = 14;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.None;
			Item.shootSpeed = 10;
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
			writer.Write(PokemonNick ?? "");
			writer.Write(Shiny);
			writer.Write(variant ?? "");
			writer.Write(BallType ?? "");
			writer.Write(pokeHeldItem ?? "");
			writer.Write((double)level);
			writer.Write((double)exp);
			writer.Write((double)nature);
			writer.Write((double)happiness);
			foreach(int IV in IVs){
				writer.Write((double)IV);
			}
			foreach(int EV in EVs){
				writer.Write((double)EV);
			}
			writer.Write(moves.Length);
			foreach(string move in moves){
				writer.Write(move);
			}
			writer.Write((double)moveIndex);
			writer.Write((double)currentHP);
			writer.Write(OriginalTrainerID ?? "");
		}

		public override void NetReceive(BinaryReader reader) {
			PokemonName = reader.ReadString();
			PokemonNick = reader.ReadString();
			Shiny = reader.ReadBoolean();
			variant = reader.ReadString();
			BallType = reader.ReadString();
			pokeHeldItem = reader.ReadString();
			level = (int)reader.ReadDouble();
			exp = (int)reader.ReadDouble();
			nature = (int)reader.ReadDouble();
			happiness = (int)reader.ReadDouble();
			for(int i = 0; i < IVs.Length; i++){
				IVs[i] = (int)reader.ReadDouble();
			}
			for(int i = 0; i < EVs.Length; i++){
				EVs[i] = (int)reader.ReadDouble();
			}
			int nMoves = reader.ReadInt32();
			moves = new string[nMoves];
			for(int i = 0; i < nMoves; i++){
				moves[i] = reader.ReadString();
			}
			moveIndex = (int)reader.ReadDouble();
			currentHP = (int)reader.ReadDouble();
			OriginalTrainerID = reader.ReadString();
		}

		public override bool? UseItem(Player player)
        {
			if (player.whoAmI == Main.myPlayer) {
				//if(player.GetModPlayer<PokemonPlayer>().FreePokemonSlots() > 0) return true;

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
				if(Main.mouseItem.IsAir){
					moveIndex = (moveIndex+1)%moves.Length;
				}else if(player.HeldItem.ModItem is PokemonConsumableItem item){
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

			int[] fullStats = GetPokemonStats(Main.player[Main.myPlayer]);
			foreach (TooltipLine line in tooltips) {
				if (line.Mod == "Terraria" && line.Name == "ItemName") {
					line.Text = Language.GetTextValue("Mods.Pokemod.NPCs."+PokemonName+"CritterNPC.DisplayName");
					if(Shiny) line.OverrideColor = Main.DiscoColor;
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") {
					int clampedLevel = Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().GetClampedLevel(level);
					line.Text = (variant != null?(variant!=""?("[c/23a462:"+variant+" variant]\n"):""):"")+
					((OriginalTrainerID != null && OriginalTrainerID != "")?Language.GetText("Mods.Pokemod.PokemonInfo.CaughtInBy").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.Items."+BallType+".DisplayName"), OriginalTrainerID.Remove(OriginalTrainerID.Length-9)).Value:Language.GetText("Mods.Pokemod.PokemonInfo.CaughtIn").WithFormatArgs(BallType.Replace("Item", "")).Value) +
					PokemonTypeToString()+
					"\n[c/ffd51c:Lvl] "+level+(clampedLevel < level?(" (Capped to "+clampedLevel+")"):"")+"  [c/ffd51c:Exp:] "+(exp-GetExpToLevel(level))+"/"+(expToNextLevel-GetExpToLevel(level))+
					"\n"+(currentHP>=0?"[c/ffd51c:HP:] "+(currentHP>0?(currentHP+"/"+fullStats[0]+" "):"[c/fa3e42:"+Language.GetTextValue("Mods.Pokemod.PokemonInfo.Fainted")+"] "):"")+
					"\n[c/fa8140:"+Language.GetTextValue("Mods.Pokemod.PokemonNatures.Nature")+": "+Language.GetTextValue("Mods.Pokemod.PokemonNatures."+PokemonData.PokemonNatures[nature/10][nature%10])+"]"+
					"\n[c/"+GetStatColor(1)+":Atk:] "+fullStats[1]+"  [c/"+GetStatColor(2)+":Def:] "+fullStats[2]+
					"\n[c/"+GetStatColor(3)+":SpAtk:] "+fullStats[3]+"  [c/"+GetStatColor(4)+":SpDef:] "+fullStats[4]+"  [c/"+GetStatColor(5)+":Speed:] "+fullStats[5]+
					"\n\nMoves: (Right Click to switch)";
					foreach(string move in moves){
						line.Text += "\n- [c/"+PokemonNPCData.GetTypeColor(PokemonData.pokemonAttacks[move].attackType)+":"+Language.GetText("Mods.Pokemod.Projectiles."+move+".DisplayName")+"]";
						if(move == moves[moveIndex]) line.Text += " <<<";
					}
				}
			}

			/*TooltipLine tooltipLine = new TooltipLine(Mod, "BallType", "Caught in a "+BallType.Replace("Item", ""));
			tooltips.Add(tooltipLine);*/
		}

		private string GetStatColor(int statIndex){
			statIndex = Math.Clamp(statIndex-1, 0, 4);
			int result = 0;

			if(statIndex == nature/10) result++;
			if(statIndex == nature%10) result--;

			if(result > 0) return "53fa40";
			else if(result < 0) return "ff253f";

			return "ffd51c";
		}

		private string PokemonTypeToString(){
			string typeString = "\n[";
			int[] types = PokemonData.pokemonInfo[PokemonName].pokemonTypes;

			if(types.Length > 0){
				if(types[0] >= 0) typeString += "[c/"+PokemonNPCData.GetTypeColor(types[0])+":"+(TypeIndex)types[0]+"]";
				if(types[1] >= 0) typeString += "/[c/"+PokemonNPCData.GetTypeColor(types[1])+":"+(TypeIndex)types[1]+"]";
			}
			
			typeString += "]";

			return typeString;
		}

        public void SetPokemonData(string PokemonName, bool Shiny, string BallType, int level = 5, int[] IVs = null, int nature = -1, string variant = ""){
            this.PokemonName = PokemonName;
			this.Shiny = Shiny;
			this.BallType = BallType;
			this.level = level;
			this.exp = GetExpToLevel(level);
			if(IVs != null){
				this.IVs = IVs;
			}
			if(nature < 0) nature = 10*Main.rand.Next(5)+Main.rand.Next(5);
            this.nature = nature;
			currentHP = GetPokemonStats()[0];
			this.variant = variant;
			
			SetPetInfo();
			GetPokemonMoves();
        }

		public int[] GetPokemonStats(Player player = null){
			return PokemonNPCData.CalcAllStats(player != null?player.GetModPlayer<PokemonPlayer>().GetClampedLevel(level):level, PokemonData.pokemonInfo[PokemonName].pokemonStats, IVs, EVs, nature);
		}

        public override void UpdateInventory(Player player)
        {
			if(OriginalTrainerID == null || OriginalTrainerID == "") OriginalTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;
			if(CurrentTrainerID == null || CurrentTrainerID == "") CurrentTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;

			shouldDespawn = despawnTime;
			if (Main.netMode != NetmodeID.SinglePlayer && proj != null) proj.timeLeft = 10;

			if (exp < GetExpToLevel(level))
			{
				exp = GetExpToLevel(level);
			}

			if (PokemonName != null && PokemonName != "")
			{
				GetProjInfo(player);
			}
			if(proj != null){
				if(currentHP == 0 && proj.active){
					proj?.Kill();
					proj = null;
				}
			}

			SetPetInfo(player);

			if (toLearnMoves.Length > 0)
			{
				if (!ModContent.GetInstance<MoveLearnUISystem>().IsActive())
				{
					List<string> toLearnMovesAux = toLearnMoves.ToList();
					if (!moves.Contains(toLearnMovesAux[0]))
					{
						LearnMove(toLearnMovesAux[0]);
					}
					toLearnMovesAux.RemoveAt(0);
					toLearnMoves = toLearnMovesAux.ToArray();
				}
			}

            base.UpdateInventory(player);

			if(ModContent.GetInstance<GameplayConfig>().ReleaseFaintedPokemon){
				if(currentHP <= 0) DeletePokemon();
			}
        }

        public override void HoldItem(Player player)
        {
			if(OriginalTrainerID == null || OriginalTrainerID == "") OriginalTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;
			if(CurrentTrainerID == null || CurrentTrainerID == "") CurrentTrainerID = player.GetModPlayer<PokemonPlayer>()?.TrainerID;

			shouldDespawn = despawnTime;
			if (Main.netMode != NetmodeID.SinglePlayer && proj != null) proj.timeLeft = 10;

			bool isHolding = false;

			if(Main.mouseItem != null){
				if(Main.mouseItem.ModItem is CaughtPokemonItem ball){
					if(ball.ComparePokemon(this)){
						if(ball.PokemonName != null && ball.PokemonName != ""){
							ball.GetProjInfo(player);
						}
						if(ball.proj != null){
							if(ball.currentHP == 0 && ball.proj.active){
								ball.proj?.Kill();
								ball.proj = null;
							}
						}
						ball.SetPetInfo(player);
						ball.shouldDespawn = despawnTime;
						if (Main.netMode != NetmodeID.SinglePlayer && ball.proj != null) ball.proj.timeLeft = 10;
						isHolding = true;
					}
				}
			}

			if(!isHolding){
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
			}

            base.HoldItem(player);

			if(ModContent.GetInstance<GameplayConfig>().ReleaseFaintedPokemon){
				if(currentHP <= 0) DeletePokemon();
			}
        }

		private void DeletePokemon(){
			Item.TurnToAir();
		}

        public void SetPetInfo(Player player = null){
			if(PokemonName != null && PokemonName != ""){
				Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
				//Item.buffType = ModContent.Find<ModBuff>("Pokemod", PokemonName+(Shiny?"PetBuffShiny":"PetBuff")).Type;

				if(moves.Length <= 0) GetPokemonMoves();

				UpdateLevel(player);
				GetProjInfo(player);
				
				if(currentHP == 0){
					Item.shoot = ProjectileID.None;
				}
				if(proj != null){
					if(proj.active){
						PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);
						moveIndex = Math.Clamp(moveIndex, 0, moves.Length);
						if(PokemonProj != null){
							if (moves[moveIndex] == "SmokeScreen") moves[moveIndex] = "Smokescreen";
							PokemonProj.currentAttack = moves[moveIndex];
							if(variant != null){
								if(variant != ""){
									PokemonProj.variant = variant;
								}
							}
						}
					}
				}
			}
		}

		private void GetProjInfo(Player player = null){
			if(proj != null){
				if(proj.active){
					GetActiveHappiness(player);
					GetProjExp(player);
					GetUsedItems(player);
					GetProjHP();
				}else{
					proj = null;
				}
			}
		}

		private void GetProjExp(Player player = null){
			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);
			if(PokemonProj != null){
				AddExp(PokemonProj.GetExpGained(), player);
			}
		}

		private void GetProjHP(){
			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);
			if(PokemonProj != null){
				if(currentHP != 0 && PokemonProj.currentHp == 0){
					AddHappiness(-3, -3, -5);
				}
				currentHP = PokemonProj.currentHp;
			}
		}

		private void GetActiveHappiness(Player player = null){
			if(player != null){
				if(timer > 30*60){
					AddHappiness(+1, +1, +0);
					timer = 0;
				}else{
					if(player.velocity.Length() > float.Epsilon){
						timer++;
					}
				}
			}
		}

		private void GetPokemonMoves(bool notLearn = false){
			List<string> newMoveList = PokemonData.pokemonInfo[PokemonName].movePool.ToList();

			if (moves.Length < 4) {
				List<string> newMoves = moves.ToList();

				while (newMoveList.Count > 0)
				{
					if (!newMoves.Contains(newMoveList[0]))
					{
						newMoves.Add(newMoveList[0]);
					}
					newMoveList.RemoveAt(0);
					if (newMoves.Count >= 4) break;
				}
				moves = newMoves.ToArray();
			}

			if (notLearn) newMoveList = new List<string>();

			if (newMoveList.Count > 0)
			{
				List<string> newToLearnMoves = toLearnMoves.ToList();
				foreach (string move in newMoveList)
				{
					newToLearnMoves.Add(move);
				}
				toLearnMoves = newToLearnMoves.ToArray();
			}
		}

		private void GetUsedItems(Player player = null){
			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);

			if(PokemonProj != null){
				if(PokemonProj.itemEvolve){
					GetCanEvolve(player);
				}
				if(PokemonProj.GetRareCandy()){
					exp = expToNextLevel;
				}
			}
		}

		private PokemonPetProjectile SafeGetPokemonProj(Projectile proj){
			PokemonPetProjectile PokemonProj;
			if(proj.ModProjectile is PokemonPetProjectile){
				PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
			}else{
				PokemonProj = null;
			}

			return PokemonProj;
		}

		private void GetCanEvolve(Player player = null){
			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);
			if(PokemonProj != null){
				string newPokemonName = PokemonProj.GetCanEvolve();
				if(newPokemonName != ""){
					Vector2 pokePosition = proj.Center - new Vector2(0,(player.height-proj.height)/2);
					proj.Kill();
					PokemonName = newPokemonName;
					GetPokemonMoves();
					Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
					if (player != null)
					{
						int projIndex = Projectile.NewProjectile(Item.GetSource_FromThis(), pokePosition, Vector2.Zero, Item.shoot, 0, 0, player.whoAmI, currentHP);
						proj = Main.projectile[projIndex];
						PokemonProj = SafeGetPokemonProj(proj);
						PokemonProj.isOut = true;
					}
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
			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);
			if(PokemonProj != null){
				string newPokemonName = PokemonProj.GetCanMegaEvolve();
				if(newPokemonName != ""){
					Vector2 pokePosition = proj.Center - new Vector2(0,(player.height-proj.height)/2);
					proj.Kill();
					PokemonName = newPokemonName;
					GetPokemonMoves(true);
					Item.shoot = ModContent.Find<ModProjectile>("Pokemod", PokemonName+(Shiny?"PetProjectileShiny":"PetProjectile")).Type;
					if (player != null)
					{
						int projIndex = Projectile.NewProjectile(Item.GetSource_FromThis(), pokePosition, Vector2.Zero, Item.shoot, 0, 0, player.whoAmI, currentHP);
						proj = Main.projectile[projIndex];
						PokemonProj = SafeGetPokemonProj(proj);
						PokemonProj.isOut = true;
					}
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
					AddHappiness(+4, +3, +2);
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
						PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);
						if(PokemonProj != null){
							PokemonProj.SetPokemonLvl(level, IVs, EVs, nature);
							if(canEvolve){
								PokemonProj.SetCanEvolve();
							}
							PokemonProj.SetCanMegaEvolve();
						}
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
			switch(PokemonData.pokemonInfo[PokemonName].expType){
				case (int)ExpTypes.Slow:
					return (int)(1.25f*Math.Pow(lvl, 3));
				case (int)ExpTypes.MediumSlow:
					return (int)((1.2f*Math.Pow(lvl, 3))-(15*Math.Pow(lvl, 2))+(100*lvl)-140);
				case (int)ExpTypes.Fast:
					return (int)(0.8f*Math.Pow(lvl, 3));
				case (int)ExpTypes.Erratic:
					if(lvl<50){
						return (int)(Math.Pow(lvl, 3)*(100-lvl)/50);
					}else if(lvl<68){
						return (int)(Math.Pow(lvl, 3)*(150-lvl)/100);
					}else if(lvl<98){
						return (int)(Math.Pow(lvl, 3)*((1911-(10*lvl))/3)/500);
					}else if(lvl<=100){
						return (int)(Math.Pow(lvl, 3)*(160-lvl)/100);
					}else{
						return (int)(0.6f*Math.Pow(lvl, 3));
					}
				case (int)ExpTypes.Fluctuating:
					if(lvl<15){
						return (int)(Math.Pow(lvl, 3)*(((lvl+1)/3)+24)/50);
					}else if(lvl<36){
						return (int)(Math.Pow(lvl, 3)*(lvl+14)/50);
					}else if(lvl<=100){
						return (int)(Math.Pow(lvl, 3)*((lvl/2)+32)/50);
					}else{
						return (int)(1.64f*Math.Pow(lvl, 3));
					}
				default:
					return (int)Math.Pow(lvl, 3);
			}
		}

		public void LearnMove(string newMove)
		{
			if (moves.Length >= 4)
			{
				ModContent.GetInstance<MoveLearnUISystem>().ShowMyUI(this, newMove);
			}
			else
			{
				List<string> existentMoves = moves.ToList();
				if (!existentMoves.Contains(newMove))
				{
					existentMoves.Add(newMove);
				}
				
				moves = existentMoves.ToArray();
			}
		}

		public void AddHappiness(int add1, int add2, int add3){
			if(happiness < 100){
				happiness += add1;
			}else if(happiness < 160){
				happiness += add2;
			}else if(happiness < 255){
				happiness += add3;
			}else if(happiness > 255){
				happiness = 255;
			}

			//Main.NewText(PokemonName+" has "+happiness+" happiness");

			happiness = Math.Clamp(happiness, 0, 255);
		}

        // NOTE: The tag instance provided here is always empty by default.
        // Read https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound to better understand Saving and Loading data.
        public override void SaveData(TagCompound tag) {
			tag["OriginalTrainerID"] = OriginalTrainerID;
			tag["CurrentTrainerID"] = CurrentTrainerID;
			tag["PokemonName"] = PokemonName;
			tag["PokemonNick"] = PokemonNick;
			tag["Level"] = level;
			tag["Exp"] = exp;
			tag["Shiny"] = Shiny;
			tag["Variant"] = variant;
			tag["Nature"] = nature;
			tag["Happiness"] = happiness;
			tag["BallType"] = BallType;
			tag["PokeHeldItem"] = pokeHeldItem;
			tag["CurrentHP"] = currentHP;
			tag["IVs"] = IVs.ToList();
			tag["EVs"] = EVs.ToList();
			tag["Moves"] = moves.ToList();
			tag["MoveIndex"] = moveIndex;
		}

		public override void LoadData(TagCompound tag) {
			OriginalTrainerID = tag.GetString("OriginalTrainerID");
			CurrentTrainerID = tag.GetString("CurrentTrainerID");
			PokemonName = tag.GetString("PokemonName");
			PokemonNick = tag.GetString("PokemonNick");
			level = tag.GetInt("Level");
			exp = tag.GetInt("Exp");
			Shiny = tag.GetBool("Shiny");
			variant = tag.GetString("Variant");
			if(tag.TryGet<int>("Nature", out int natureAux)){
				nature = natureAux;
			}else{
				nature = 10*Main.rand.Next(5)+Main.rand.Next(5);
			}
			happiness = tag.GetInt("Happiness");
			BallType = tag.GetString("BallType");
			pokeHeldItem = tag.GetString("PokeHeldItem");
			currentHP = tag.GetInt("CurrentHP");
			if(tag.ContainsKey("IVs")){
				IVs = [.. tag.GetList<int>("IVs")];
			}
			if(tag.ContainsKey("EVs")){
				EVs = [.. tag.GetList<int>("EVs")];
			}
			if(tag.ContainsKey("Moves")){
				moves = [.. tag.GetList<string>("Moves")];
			}
			moveIndex = Math.Clamp(tag.GetInt("MoveIndex"), 0, moves.Length);

			SetPetInfo();
			SetExpToNextLevel();
		}

        /*public override void PostUpdate()
        {
			if (proj != null)
			{
				Main.NewText("PostUpdate: " +proj);
			}
			else
			{
				Main.NewText("PostUpdate: " +"Proj null");
			}
			if (shouldDespawn > 0) shouldDespawn--;
			if (shouldDespawn <= 0)
			{
				DespawnPokemon();
			}

            base.PostUpdate();
        }*/

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			DespawnPokemon();

            base.Update(ref gravity, ref maxFallSpeed);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (BallType != null && BallType != "")
			{
				Asset<Texture2D> ballTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokeballs/" + BallType);

				spriteBatch.Draw(ballTexture.Value,
					position: Item.position - Main.screenPosition,
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
				shouldDespawn = despawnTime;
			}
				
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

		public bool ComparePokemon(CaughtPokemonItem other){
			if(OriginalTrainerID == other.OriginalTrainerID && CurrentTrainerID == other.CurrentTrainerID
			&& PokemonName == other.PokemonName && PokemonNick == other.PokemonNick && exp == other.exp
			&& nature == other.nature && EVs == other.EVs && Shiny == other.Shiny && variant == other.variant
			&& BallType == other.BallType){
				return true;
			}

			return false;
		}

		public void DespawnPokemon(){
			if (proj != null && Main.netMode != NetmodeID.Server)
			{
				if (proj.active)
				{
					GetProjHP();
					proj.Kill();
				}
				else
				{
					proj = null;
				}
			}
		}
    }
}