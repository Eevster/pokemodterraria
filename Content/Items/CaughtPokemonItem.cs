using Humanizer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Configs;
using Pokemod.Common.Players;
using Pokemod.Common.UI.MoveLearnUI;
using Pokemod.Common.UI.MoveTutorUI;
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
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
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
		public bool didMegaEvolve;

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
            for (int i = 0; i < 6; i++){
				writer.Write((double)IVs[i]);
			}
            for (int i = 0; i < 6; i++){
				writer.Write((double)EVs[i]);
			}
			writer.Write((double)moves.Length);
            for (int i = 0; i < moves.Length; i++)
            {
				writer.Write(moves[i]);
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
			for(int i = 0; i < 6; i++){
				IVs[i] = (int)reader.ReadDouble();
			}
			for(int i = 0; i < 6; i++){
				EVs[i] = (int)reader.ReadDouble();
			}
			int nMoves = (int)reader.ReadDouble();
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
				if (proj.ModProjectile is PokemonPetProjectile petProj)
				{
					petProj.variant = variant;
				}
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
						player.itemTime = 10;
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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (PokemonName == null || PokemonName == "") //colors may be null if spawned from other mods which don't call OnCreate
				return;

			int[] fullStats = GetPokemonStats(Main.player[Main.myPlayer]);
			foreach (TooltipLine line in tooltips)
			{
				if (line.Mod == "Terraria" && line.Name == "ItemName")
				{
					line.Text = Language.GetTextValue("Mods.Pokemod.NPCs." + PokemonName + "CritterNPC.DisplayName");
					if (Shiny) line.OverrideColor = Main.DiscoColor;
				}
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					int clampedLevel = Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>().GetClampedLevel(level);

					string attackString = "[c/" + GetStatColor(1) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonStats.Attack") + ":] " + fullStats[1];
					string defenseString = " [c/" + GetStatColor(2) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonStats.Defense") + ":] " + fullStats[2];
					string spAtkString = "[c/" + GetStatColor(3) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonStats.SpecialAttack") + ":] " + fullStats[3];
					string spDefString = " [c/" + GetStatColor(4) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonStats.SpecialDefense") + ":] " + fullStats[4];
					string speedString = "[c/" + GetStatColor(5) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonStats.Speed") + ":] " + fullStats[5];

					line.Text = (variant != null ? (variant != "" ? ("[c/23a462:" + variant + " variant]\n") : "") : "") +
					((OriginalTrainerID != null && OriginalTrainerID != "") ? Language.GetText("Mods.Pokemod.PokemonInfo.CaughtInBy").WithFormatArgs(Language.GetTextValue("Mods.Pokemod.Items." + BallType + ".DisplayName"), OriginalTrainerID.Remove(OriginalTrainerID.Length - 9)).Value : Language.GetText("Mods.Pokemod.PokemonInfo.CaughtIn").WithFormatArgs(BallType.Replace("Item", "")).Value) +
					PokemonTypeToString() +
					"\n[c/FFE270:Lvl] " + level + (clampedLevel < level ? (" (Capped to " + clampedLevel + ")") : "") + "  [c/FFE270:Exp:] " + (exp - GetExpToLevel(level)) + "/" + (expToNextLevel - GetExpToLevel(level)) +
					"\n" + (currentHP >= 0 ? "[c/FFE270:HP:] " + (currentHP > 0 ? (currentHP + "/" + fullStats[0] + " " + GetTextHPBar((float)currentHP / fullStats[0])) : "[c/fa3e42:" + Language.GetTextValue("Mods.Pokemod.PokemonInfo.Fainted") + "] ") : "") +
					"\n[c/fa8140:" + Language.GetTextValue("Mods.Pokemod.PokemonNatures.Nature") + ": " + Language.GetTextValue("Mods.Pokemod.PokemonNatures." + PokemonData.PokemonNatures[nature / 10][nature % 10]) + "]" +
					"\n" + attackString + defenseString +
					"\n" + spAtkString + spDefString +
					"\n" + speedString +

					"\n\nMoves: (Right Click to switch)";
					foreach (string move in moves)
					{
						line.Text += "\n- [c/" + PokemonNPCData.GetTypeColor(PokemonData.pokemonAttacks[move].attackType) + ":" + Language.GetText("Mods.Pokemod.Projectiles." + move + ".DisplayName") + "]";
						if (move == moves[moveIndex]) line.Text += " <<<";
					}
				}
			}

			/*TooltipLine tooltipLine = new TooltipLine(Mod, "BallType", "Caught in a "+BallType.Replace("Item", ""));
			tooltips.Add(tooltipLine);*/
		}
		
		private string GetTextHPBar(float barFill)
		{
			string fillChar = "█";
			int nfillChars = (int)Math.Round(barFill * 10f, 0, MidpointRounding.AwayFromZero);

			string colorCode = "[c/1AFF4B:";
			if (barFill <= 0.2f) colorCode = "[c/FF221A:";
			else if (barFill <= 0.5f) colorCode = "[c/FFF41A:";

			string textHPBar = "[" + ((nfillChars>0)?(colorCode + String.Concat(Enumerable.Repeat(fillChar, nfillChars)) + "]"):"")+((10-nfillChars > 0)?("[c/4A4A4A:" + String.Concat(Enumerable.Repeat(fillChar, 10-nfillChars)) + "]"):"")+"]";

			return textHPBar;
        }

		private string GetStatColor(int statIndex){
			statIndex = Math.Clamp(statIndex-1, 0, 4);
			int result = 0;

			if(statIndex == nature/10) result++;
			if(statIndex == nature%10) result--;

			if(result > 0) return "8BF97C";
			else if(result < 0) return "FF596F";

			return "FFE270";
		}

		private string PokemonTypeToString(){
			string typeString = "\n[";
			int[] types = PokemonData.pokemonInfo[PokemonName].pokemonTypes;

			if(types.Length > 0){
				if(types[0] >= 0) typeString += "[c/"+PokemonNPCData.GetTypeColor(types[0])+":"+Language.GetTextValue("Mods.Pokemod.PokemonTypes."+(TypeIndex)types[0])+"]";
				if(types[1] >= 0) typeString += "/[c/"+PokemonNPCData.GetTypeColor(types[1])+":"+Language.GetTextValue("Mods.Pokemod.PokemonTypes."+(TypeIndex)types[1])+"]";
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
			GetPokemonMoves(notLearn: true); // Pokemon caught in the wild shouldn't prompt the player to choose between moves. 
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

				if(moves.Length <= 0) GetPokemonMoves(notLearn: true); //If in this situation (going from 0 moves to more than 4) the player should not also have the luxury of choice.

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
							PokemonProj.ClearOldMoves();
							PokemonProj.ballType = BallType;
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

		private void GetPokemonMoves(bool notLearn = false)
		{
			List<MoveLvl> newMoveList = PokemonData.pokemonInfo[PokemonName].movePool.ToList();

			int oldMoveCount = moves.Length;
			List<string> newMoves = moves.ToList();
			List<string> learningMoves = toLearnMoves.ToList();

			while (newMoveList.Count > 0)
			{
				if (newMoveList[0].levelToLearn > level) break;

				if (!newMoves.Contains(newMoveList[0].moveName))
				{
                    if (newMoves.Count >= 4)
					{
						if (oldMoveCount >= 4 || !notLearn) // Pokemon shouldn't naturally get the opportunity to learn lower level moves (except for moves intended for learning on evolution).
						{
                            if (newMoveList[0].levelToLearn == level || newMoveList[0].levelToLearn == 0) learningMoves.Add(newMoveList[0].moveName);
                            newMoveList.RemoveAt(0);
							continue;
                        }
						else // In the case of caught pokemon, or error pokemon (with no move pool), the pokemon should select the 4 moves in its pool with the highest level requirement possible for its level. If players want the earlier learnset, they will need to catch weaker pokemon or breed (when we add it).
						{
                            newMoves.RemoveAt(oldMoveCount);
                        }
					}
					newMoves.Add(newMoveList[0].moveName);
					if (!notLearn) MoveLearnEffects(newMoveList[0].moveName);
				}
				newMoveList.RemoveAt(0);
			}
			moves = newMoves.ToArray();
			if (!notLearn) toLearnMoves = learningMoves.ToArray();
		}

		private void GetPokemonLvlMoves(int lvl)
		{
			List<MoveLvl> newMoveList = PokemonData.pokemonInfo[PokemonName].movePool.ToList();

			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);

			if (PokemonProj != null)
			{
				if (PokemonProj.isMega && PokemonProj.megaEvolutionBase.Length > 0)
				{
					newMoveList = PokemonData.pokemonInfo[PokemonProj.megaEvolutionBase[0]].movePool.ToList();
				}
			}

			if (newMoveList.Count > 0)
			{
				List<string> newToLearnMoves = toLearnMoves.ToList();
				foreach (MoveLvl move in newMoveList)
				{
					if (move.levelToLearn == lvl) {
						newToLearnMoves.Add(move.moveName);
					}
				}
				toLearnMoves = newToLearnMoves.ToArray();
			}
		}

		private void GetUsedItems(Player player = null)
		{
			PokemonPetProjectile PokemonProj = SafeGetPokemonProj(proj);

			if (PokemonProj != null)
			{
				if (PokemonProj.itemEvolve)
				{
					GetCanEvolve(player);
				}
				if (PokemonProj.GetRareCandy())
				{
					exp = expToNextLevel;
				}
			}
		}

		private PokemonPetProjectile SafeGetPokemonProj(Projectile proj){
			if(!(proj != null && proj.active)) return null;

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

					UnlockBestiary(PokemonName);
					RegisterPokemon(player);

					GetPokemonMoves(); //When evolving the player can choose from any evolution moves (Learned at level "0") or any move learned at the current level.
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

		private void UnlockBestiary(string pokemonName)
		{
            // Manually Adds the pokemon to the Bestiary when obtained
            string persistentId = "Pokemod/" + pokemonName + "CritterNPC" + (Shiny ? "Shiny" : "");
            NPCKillsTracker tracker = Main.BestiaryTracker.Kills;
            int currentCount = tracker.GetKillCount(persistentId);
            tracker.SetKillCountDirectly(persistentId, currentCount + 1);
        }

		private void RegisterPokemon(Player player)
		{
			if (player != null)
			{
				PokemonPlayer trainer = player.GetModPlayer<PokemonPlayer>();

				if (trainer.TrainerID == CurrentTrainerID)
				{
					trainer.RegisterPokemon(PokemonName, true);
				}
			}
		}

		private bool GetEvolutionRestricted(Player player = null)
		{
			if (player != null)
			{
				if (player.GetModPlayer<PokemonPlayer>().HasEverstone > 0)
				{
					if (Item.favorited) return true;
					if (player.miscEquips[0] != null && !player.miscEquips[0].IsAir)
					{
						if (player.miscEquips[0].ModItem is CaughtPokemonItem)
						{
							if (player.miscEquips[0].ModItem == Item.ModItem)
							{
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
					RegisterPokemon(player);
					if (!didMegaEvolve) GetPokemonMoves(); //Like any evolution or level up, a pokemon should only ever get this opportunity once (previously it could theoretically re-learn a move from mega evolving if it somehow removed it and had less than 4 moves).
					didMegaEvolve = true;
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
					GetPokemonLvlMoves(level);
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
							PokemonProj.SetPokemonLvl(level, IVs, EVs, nature, happiness);
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

		public void LearnMove(string newMove, int itemUsedType = -1, int itemUsedAmount = 1)
		{
			if (moves.Length >= 4)
			{
				MoveLearnUISystem uiSystem = ModContent.GetInstance<MoveLearnUISystem>();
				uiSystem.ShowMyUI(this, newMove, itemUsedType, itemUsedAmount);
            }
			else
			{
				List<string> existentMoves = moves.ToList();
				if (!existentMoves.Contains(newMove))
				{
					existentMoves.Add(newMove);
				}
				
				moves = existentMoves.ToArray();
				MoveLearnEffects(newMove);
			}
		}

		public void MoveLearnEffects(string move)
		{
			Player player = Main.LocalPlayer;
			if (player == Main.player[Item.playerIndexTheItemIsReservedFor]) {
				Color typeColor = ColorConverter.HexToXnaColor(PokemonNPCData.GetTypeColor(PokemonData.pokemonAttacks[move].attackType));
                Main.NewText(Language.GetText("Mods.Pokemod.MoveLearnUI.Success").WithFormatArgs(PokemonName, move).ToString(), color: typeColor);
				
				Vector2 effectPosition = proj != null && proj.active ? proj.Center : player.Center;

				SoundEngine.PlaySound(SoundID.Item28 with { Pitch = -1f , Volume = 0.5f }, effectPosition);

				for (int i = 0; i < 10; i++)
				{
					int dust = Dust.NewDust(effectPosition, 0, 0, DustID.PortalBolt, Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2), Scale: 2f);
					Main.dust[dust].noGravity = true;
                    Main.dust[dust].noLight = true;
                    Main.dust[dust].color = typeColor;
                }
            }
			MoveTutorUISystem tutorUI = ModContent.GetInstance<MoveTutorUISystem>();
		    tutorUI.RefreshUI();
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
			currentHP = Math.Max(tag.GetInt("CurrentHP"),0);
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

        public override bool OnPickup(Player player)
        {
			RegisterPokemon(player);

            return base.OnPickup(player);
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
					(currentHP <= 0)?Color.DarkGray:drawColor,
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

				if (currentHP > 0)
				{
					float quotient = (float)currentHP / GetPokemonStats(Main.player[Main.myPlayer])[0];;
					quotient = Utils.Clamp(quotient, 0f, 1f);

					int barWidth = 4;
					int barHeight = 28;

					int up = (int)(-scale*0.5f*barHeight);
					int down = (int)(scale*0.5f*barHeight);
					int steps = (int)((down - up) * quotient);

					for (int i = 0; i < steps; i += 1)
					{
						spriteBatch.Draw(TextureAssets.MagicPixel.Value, position + new Vector2((int)(-scale*0.5f*barHeight), down-i), new Rectangle(0, 0, (int)(scale * barWidth), 1), GetHPBarColor(quotient), 0, new Rectangle(0, 0, (int)(scale * barWidth), 1).Size() * 0.5f, 1, SpriteEffects.None, 0);
					}
				}
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

		public Color GetHPBarColor(float percent)
		{
			if(percent > 0.5f) return new Color(26, 255, 75);
			else if(percent > 0.2f) return new Color(255, 244, 26);
			else return new Color(255, 34, 26);
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
					proj = null;
				}
				else
				{
					proj = null;
				}
			}
		}
    }
}