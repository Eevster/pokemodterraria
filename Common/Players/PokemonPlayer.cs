using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.Items;
using Pokemod.Content.Pets.CharmanderPet;
using Pokemod.Content.Pets.BulbasaurPet;
using Pokemod.Content.Pets.SquirtlePet;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.IO;
using Pokemod.Content.Pets.ChikoritaPet;
using Pokemod.Content.Pets.CyndaquilPet;
using Pokemod.Content.Pets.TotodilePet;
using Pokemod.Content.Tiles;
using Pokemod.Content.NPCs;
using Pokemod.Content.Buffs;
using Pokemod.Common.Configs;
using Terraria.Localization;

namespace Pokemod.Common.Players
{
	public class PokemonPlayer : ModPlayer
	{
		public bool HasStarter;
		//Accessories
		public int CanMegaEvolve;
		public string MegaStone;
		public int HasMegaStone;
		public bool HasShinyCharm;
		public int HasEverstone;
		public int HasAirBalloon;

		public float ExpMult = 1f;
		public int LeftoversTimer;

		//Multipliers
		public float[] typeMult = Enumerable.Repeat(1f,18).ToArray();

		//Trainer Glove
		public int attackMode;
		public enum AttackMode
		{
			Auto_Attack,
			No_Attack,
			Directed_Attack
		}
		public Vector2 attackPosition;
		private int directedEmptyTimer;
		private const int directedEmptyMaxTime = 3 * 60;
		public NPC targetNPC;
		public Player targetPlayer;

		//Registered Pokemon
		public Dictionary<string, int> registeredPokemon = new Dictionary<string, int>();

		//Active Pokemon
		public int maxPokemon;
		public const int defaultMaxPokemon = 1;
		public List<int> currentActivePokemon = new List<int>();

		//Level Cap
		public int levelCap;
		public const int defaultLevelCap = 10;

		//Manual Control
		public bool manualControl;

		//Battle Mode

		public bool onBattle = false;
		CaughtPokemonItem[] currentPokemonTeam;

		//Player data
		public string TrainerID { get; internal set; }

		//Textures
		private static Asset<Texture2D> targetTexture;

		private static Asset<Texture2D> noAttackTexture;
		private static Asset<Texture2D> autoAttackTexture;
		private static Asset<Texture2D> directedAttackTexture;

		public override void Load()
		{
			targetTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/PokemonTarget");

			noAttackTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/CursorNoAttack");
			autoAttackTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/CursorAutoAttack");
			directedAttackTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/CursorDirectedAttack");
		}

		public override void Unload()
		{
			targetTexture = null;

			noAttackTexture = null;
			autoAttackTexture = null;
			directedAttackTexture = null;
		}

		public override void SaveData(TagCompound tag)
		{
			tag["TrainerID"] = TrainerID;
			tag["HasStarter"] = HasStarter;
			tag["CanMegaEvolve"] = CanMegaEvolve;

			tag["RegisteredPKKeys"] = registeredPokemon.Keys.ToList();
			tag["RegisteredPKValues"] = registeredPokemon.Values.ToList();
		}

		public override void LoadData(TagCompound tag)
		{
			TrainerID = tag.GetString("TrainerID");
			HasStarter = tag.GetBool("HasStarter");
			CanMegaEvolve = tag.GetInt("CanMegaEvolve");

			List<string> auxKeys = tag.GetList<string>("RegisteredPKKeys").ToList();
			List<int> auxValues = tag.GetList<int>("RegisteredPKValues").ToList();

			registeredPokemon = new Dictionary<string, int>();

			for (int i = 0; i < auxKeys.Count; i++)
			{
				registeredPokemon.Add(auxKeys[i], i < auxValues.Count ? auxValues[i] : 0);
			}
		}

		public override void Initialize()
		{
			SetTrainerID();
			base.Initialize();
		}

		private void SetTrainerID()
		{
			if (Player.name == null)
			{
				return;
			}
			else
			{
				if (Player.name == "") return;
			}

			string id = Player.name;
			for (int i = 0; i < 9; i++)
			{
				id += "" + Main.rand.Next(10);
			}

			TrainerID = id;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)PokemodMessageType.PokemonPlayerSync);
			packet.Write((byte)Player.whoAmI);

			packet.Write((byte)attackMode);
			packet.WriteVector2(attackPosition);

			packet.Send(toWho, fromWho);
		}

		public void ReceivePlayerSync(BinaryReader reader)
		{
			attackMode = reader.ReadByte();
			attackPosition = reader.ReadVector2();
		}

		public override void CopyClientState(ModPlayer targetCopy)
		{
			PokemonPlayer clone = (PokemonPlayer)targetCopy;
			clone.attackMode = attackMode;
			clone.attackPosition = attackPosition;
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			PokemonPlayer clone = (PokemonPlayer)clientPlayer;

			if (clone.attackMode != attackMode || clone.attackPosition != attackPosition)
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}

		public override void ResetEffects()
		{
			if (TrainerID == null)
			{
				SetTrainerID();
			}
			else
			{
				if (TrainerID == "")
				{
					SetTrainerID();
				}
			}

			if (targetNPC != null)
			{
				if (targetNPC.active)
				{
					if (attackMode == (int)AttackMode.Directed_Attack) directedEmptyTimer = 0;
					attackPosition = targetNPC.Center;
				}
				else
				{
					targetNPC = null;
				}
			}

			if (targetPlayer != null)
			{
				if (targetPlayer.active)
				{
					if (attackMode == (int)AttackMode.Directed_Attack) directedEmptyTimer = 0;
					attackPosition = targetPlayer.Center;
				}
				else
				{
					targetPlayer = null;
				}
			}
			if (attackMode == (int)AttackMode.Directed_Attack)
			{
				if (directedEmptyTimer > 0) directedEmptyTimer--;
				if (targetPlayer == null && targetNPC == null)
				{
					if (directedEmptyTimer <= 0) ChangeAttackMode((int)AttackMode.Auto_Attack);
				}
			}

			HasShinyCharm = false;
			if (HasEverstone > 0) HasEverstone--;
			if (HasMegaStone > 0) HasMegaStone--;
			else MegaStone = "";

			if (CanMegaEvolve == 2 && !Main.dayTime)
			{
				CanMegaEvolve = 1;
			}
			if (CanMegaEvolve == 1 && Main.dayTime)
			{
				CanMegaEvolve = 0;
			}

			maxPokemon = defaultMaxPokemon;
			currentActivePokemon ??= [];

			int i = 0;
			while (i < currentActivePokemon.Count)
			{
				if (!IsValidPokemon(Main.projectile[currentActivePokemon[i]]))
				{
					currentActivePokemon.Remove(currentActivePokemon[i]);
				}
				else
				{
					i++;
				}
			}

			levelCap = defaultLevelCap;

			if (manualControl && !onBattle)
			{
				if (currentActivePokemon.Count <= 0) manualControl = false;
				else
				{
					PokemonPetProjectile pokemonProj = GetPokemonProjectile(0);

					if (pokemonProj != null && !pokemonProj.manualControl) manualControl = false;
				}
			}

			if (onBattle)
			{
				Player.aggro -= 3000;
			}

			ExpMult = 1f;
			typeMult = Enumerable.Repeat(1f,18).ToArray();

			if (LeftoversTimer > 0) LeftoversTimer--;
			if (HasAirBalloon > 0) HasAirBalloon--;
		}

		public int FreePokemonSlots()
		{
			return maxPokemon - currentActivePokemon.Count;
		}

		public void ChangeAttackMode(int mode)
		{
			directedEmptyTimer = 0;
			attackMode = mode;
			SoundEngine.PlaySound(SoundID.Item1, Player.position);

			targetNPC = null;
			targetPlayer = null;

			if (attackMode == (int)AttackMode.Directed_Attack)
			{
				attackPosition = Main.MouseWorld;
				SearchNPCTarget(48);
				directedEmptyTimer = directedEmptyMaxTime;
			}
		}

		public void SearchNPCTarget(float maxDetectDistance)
		{
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			targetNPC = null;
			targetPlayer = null;

			for (int k = 0; k < Main.maxPlayers; k++)
			{
				if (Main.player[k] != null)
				{
					Player target = Main.player[k];
					if (target.whoAmI != Player.whoAmI)
					{
						if (target.active && !target.dead)
						{
							float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, attackPosition);

							// Check if it is within the radius
							if (sqrDistanceToTarget < sqrMaxDetectDistance)
							{
								if (target.hostile)
								{
									sqrMaxDetectDistance = sqrDistanceToTarget;
									targetPlayer = target;
									targetNPC = null;
								}
							}
						}
					}
				}
			}
			// This code is required either way, used for finding a target
			if (targetPlayer == null)
			{
				for (int k = 0; k < Main.maxNPCs; k++)
				{
					NPC target = Main.npc[k];

					if (target.CanBeChasedBy() || target.ModNPC is PokemonWildNPC)
					{
						// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
						float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, attackPosition);

						// Check if it is within the radius
						if (sqrDistanceToTarget < sqrMaxDetectDistance)
						{
							sqrMaxDetectDistance = sqrDistanceToTarget;
							targetNPC = target;
							targetPlayer = null;
						}
					}
				}
			}
		}

		public string GetAttackModeText(int mode)
		{
			return ("" + (AttackMode)mode).Replace("_", " ");
		}

		public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
		{
			if (item.type == ModContent.ItemType<CharmanderPetItem>() || item.type == ModContent.ItemType<BulbasaurPetItem>() || item.type == ModContent.ItemType<SquirtlePetItem>() ||
			item.type == ModContent.ItemType<ChikoritaPetItem>() || item.type == ModContent.ItemType<CyndaquilPetItem>() || item.type == ModContent.ItemType<TotodilePetItem>())
			{
				int ballItem;
				string pokemonName = "";

				if (item.type == ModContent.ItemType<CharmanderPetItem>()) pokemonName = "Charmander";
				if (item.type == ModContent.ItemType<BulbasaurPetItem>()) pokemonName = "Bulbasaur";
				if (item.type == ModContent.ItemType<SquirtlePetItem>()) pokemonName = "Squirtle";
				if (item.type == ModContent.ItemType<ChikoritaPetItem>()) pokemonName = "Chikorita";
				if (item.type == ModContent.ItemType<CyndaquilPetItem>()) pokemonName = "Cyndaquil";
				if (item.type == ModContent.ItemType<TotodilePetItem>()) pokemonName = "Totodile";

				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
				}
				else if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer == Player.whoAmI)
				{
					//ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
					ballItem = Item.NewItem(Player.GetSource_FromThis(), (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, ModContent.ItemType<CaughtPokemonItem>(), 1, noBroadcast: false, -1);
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
					NetMessage.SendData(MessageID.SyncItem, -1, -1, null, ballItem, 1f);
				}

				item.TurnToAir();
			}
			base.PostBuyItem(vendor, shopInventory, item);
		}

		public void GenerateCaughtPokemon(string pokemonName)
		{
			int ballItem;
			bool shiny = pokemonName.EndsWith("Shiny");
			if (shiny) pokemonName = pokemonName[..^5];

			RegisterPokemon(pokemonName, true);

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
				CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
				pokeItem.SetPokemonData(pokemonName, Shiny: shiny, BallType: shiny ? "PremierballItem" : "PokeballItem");
			}
			else if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer == Player.whoAmI)
			{
				//ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
				ballItem = Item.NewItem(Player.GetSource_FromThis(), (int)Player.position.X, (int)Player.position.Y, Player.width, Player.height, ModContent.ItemType<CaughtPokemonItem>(), 1, noBroadcast: false, -1);
				CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
				pokeItem.SetPokemonData(pokemonName, Shiny: shiny, BallType: shiny ? "PremierballItem" : "PokeballItem");
				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, ballItem, 1f);
			}
		}

		public void RegisterPokemon(string pokemonName, bool captured)
		{
			List<string> allForms = PokemonData.GetAllForms(pokemonName);

			foreach (string form in allForms)
			{
				if (!registeredPokemon.Keys.Contains(form))
				{
					registeredPokemon.Add(form, (captured && form == pokemonName) ? 1 : 0);
				}
				else if (captured && form == pokemonName) registeredPokemon[form] = 1;
			}
		}

		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			if (Player.whoAmI == Main.myPlayer)
			{
				if (Vector2.Distance(Player.Center, attackPosition) < 3000 && attackMode == (int)AttackMode.Directed_Attack)
				{
					Main.EntitySpriteDraw(targetTexture.Value, attackPosition - Main.screenPosition, targetTexture.Value.Bounds, Color.White * 0.8f, 0, targetTexture.Size() * 0.5f, 1, SpriteEffects.None, 0);
				}
				if (Player.HeldItem.ModItem is TrainerGlove)
				{
					Asset<Texture2D> gloveIconTexture;
					if (attackMode == (int)AttackMode.Directed_Attack)
					{
						gloveIconTexture = directedAttackTexture;
					}
					else if (attackMode == (int)AttackMode.Auto_Attack)
					{
						gloveIconTexture = autoAttackTexture;
					}
					else
					{
						gloveIconTexture = noAttackTexture;
					}
					float cursorScale = Main.cursorScale / Main.GameZoomTarget;
					Main.spriteBatch.Draw(gloveIconTexture.Value, Main.MouseWorld - Main.screenPosition + cursorScale * new Vector2(-gloveIconTexture.Width() / 2, 8 + gloveIconTexture.Height() / 2), gloveIconTexture.Value.Bounds, Color.White, 0, gloveIconTexture.Size() * 0.5f, cursorScale, SpriteEffects.None, 0);
				}

				foreach(Projectile proj in Main.projectile){
					if(proj.owner == Player.whoAmI){
						if(proj.ModProjectile != null){
							if(proj.active){
								if(proj.ModProjectile is PokemonPetProjectile pokemon){
									Vector2 mousePosition = Main.MouseWorld;
									if (Vector2.Distance(proj.Center, mousePosition) <= 32f){
										string PokemonInfo = pokemon.pokemonName + " Lvl " + pokemon.pokemonLvl;
										Main.instance.MouseText(PokemonInfo);
									}
								}
							}
						}
					}
				}
			}
			base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
		}

		public override void UpdateDead()
		{
			bool healed = false;
			foreach (Item item in Player.inventory)
			{
				if (item.ModItem is CaughtPokemonItem pokeItem)
				{
					PokeHealerTile.HealPokemon(pokeItem, 1, ref healed);
				}
			}
			if (Player.miscEquips[0] != null && !Player.miscEquips[0].IsAir)
			{
				if (Player.miscEquips[0].ModItem is CaughtPokemonItem pokeItem)
				{
					PokeHealerTile.HealPokemon(pokeItem, 1, ref healed);
				}
			}
			base.UpdateDead();
		}

		public void LeftoversEffect()
		{
			if (LeftoversTimer <= 0)
			{
				foreach (int index in currentActivePokemon)
				{
					if (Main.projectile[index].active)
					{
						PokemonPetProjectile PokemonProj;

						if (Main.projectile[index].ModProjectile is PokemonPetProjectile) PokemonProj = (PokemonPetProjectile)Main.projectile[index]?.ModProjectile;
						else PokemonProj = null;

						PokemonProj?.regenHP(3 * 5);
					}
				}
				LeftoversTimer = 10 * 60;
			}
		}

		public PokemonPetProjectile GetPokemonProjectile(int i)
		{
			if (currentActivePokemon.Count > 0 && i < currentActivePokemon.Count)
			{
				if (IsValidPokemon(Main.projectile[currentActivePokemon[i]]))
				{
					return (PokemonPetProjectile)Main.projectile[currentActivePokemon[i]].ModProjectile;
				}
			}

			return null;
		}

		public bool HasPokemonByName(string pokemonName)
		{
			for (int i = 0; i < currentActivePokemon.Count; i++)
			{
				PokemonPetProjectile currentPokemon = GetPokemonProjectile(i);

				if (currentPokemon != null && currentPokemon.pokemonName == pokemonName)
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsValidPokemon(Projectile proj)
		{
			if (!proj.active)
			{
				return false;
			}
			else if (proj.ModProjectile is not PokemonPetProjectile)
			{
				return false;
			}

			return true;
		}

		public void SetManualControl()
		{
			if(onBattle) return;

			if (!manualControl)
			{
				PokemonPetProjectile pokemonProj = GetPokemonProjectile(0);

				if (pokemonProj != null)
				{
					pokemonProj.manualControl = true;
					manualControl = true;
				}
			}
			else
			{
				manualControl = false;
			}
		}

		public void SetMegaEvolution(string megaName)
		{
			if (MegaStone != megaName) Player.ClearBuff(ModContent.BuffType<MegaEvolution>());

			MegaStone = megaName;
		}

		public int GetClampedLevel(int pokemonLvl)
		{
			if (ModContent.GetInstance<GameplayConfig>().LevelCapType == GameplayConfig.LevelCapOptions.LevelClamping)
			{
				return System.Math.Clamp(pokemonLvl, 0, levelCap);
			}

			return pokemonLvl;
		}

		public override void OnHitAnything(float x, float y, Entity victim)
		{
			if (victim.GetType().ToString() == "Terraria.NPC")
			{
				NPC target = (NPC)victim;
				if (target.ModNPC is PokemonWildNPC nPC)
				{
					RegisterPokemon(nPC.pokemonName, false);
				}
			}
			base.OnHitAnything(x, y, victim);
		}

		public override void PreUpdateMovement()
		{
			if (manualControl)
			{
				Player.velocity.X = 0;
				if (Player.velocity.Y < 0 || onBattle) Player.velocity.Y = 0;
			}
			base.PreUpdateMovement();
		}

		public override void ModifyScreenPosition()
		{
			if (manualControl)
			{
				PokemonPetProjectile pokemonProj = GetPokemonProjectile(0);
				if (pokemonProj != null)
				{
					Main.screenPosition = pokemonProj.Projectile.Center - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
				}
			}
		}

		public override void PostUpdateBuffs()
		{
			if (manualControl)
			{
				Player.controlUseTile = false;

				/*if (!Player.HeldItem.IsAir && Player.HeldItem.ModItem is not SynchroMachine)
				{
					Player.delayUseItem = true;
					Player.controlUseTile = false;
				}*/
			}

			if (onBattle)
			{
				Player.aggro -= 3000;
			}
		}

		public override bool CanUseItem(Item item)
		{
			if (manualControl)
			{
				if (item.ModItem is not SynchroMachine)
				{
					return false;
				}
			}

			return base.CanUseItem(item);
		}

		public bool SetBattle(bool active)
		{
			if (active)
			{
				currentPokemonTeam = GetPokemonTeam(6);

				if (currentPokemonTeam.Length > 0)
				{
					onBattle = true;

					DespawnAllPokemon();
					SendNextPokemon();

					return true;
				}
				else
				{
					Main.NewText(Language.GetTextValue("Mods.Pokemod.PokemonBattle.NoTeam"), 255, 130, 130);
				}
			}
			else
			{
				onBattle = false;
				manualControl = false;

				Player.SetImmuneTimeForAllTypes(Player.longInvince ? 120 : 80);

				return true;
			}

			return false;
		}

		public void SendNextPokemon()
		{
			foreach(CaughtPokemonItem nextPokemon in currentPokemonTeam)
			{
				if (nextPokemon.currentHP > 0 && !(nextPokemon.proj != null && nextPokemon.proj.ModProjectile is PokemonPetProjectile && ((PokemonPetProjectile)nextPokemon.proj.ModProjectile).currentHp <= 0))
				{
					if (nextPokemon.proj == null)
					{
						nextPokemon.UseItem(Player);
						nextPokemon.Shoot(Player, null, Player.Center, 10f * new Vector2(Player.direction > 0 ? 1 : -1, 0), nextPokemon.Item.shoot, nextPokemon.Item.damage, nextPokemon.Item.knockBack);
					}

					if (nextPokemon.proj.ModProjectile is PokemonPetProjectile pokemonProj)
					{
						manualControl = true;
						pokemonProj.manualControl = true;
					}

					Main.NewText(Language.GetText("Mods.Pokemod.PokemonBattle.NextPokemon").WithFormatArgs(nextPokemon.PokemonName).Value, 0, 191, 35); 

					return;
				}
			}

			Main.NewText(Language.GetTextValue("Mods.Pokemod.PokemonBattle.TeamDefeated"), 255, 130, 130);
			SetBattle(false);
		}
		public CaughtPokemonItem[] GetPokemonTeam(int amount) {
			List<Item> pokemonItems = Player.inventory.ToList().FindAll(item => item.type == ModContent.ItemType<CaughtPokemonItem>());
			List<CaughtPokemonItem> pokemonTeam = [];

			for (int i = 0; i < Math.Min(amount, pokemonItems.Count); i++)
			{
				pokemonTeam.Add(pokemonItems[i].ModItem as CaughtPokemonItem);
				//Main.NewText(((CaughtPokemonItem)pokemonItems[i].ModItem).PokemonName);
			}

			return pokemonTeam.ToArray();
		}

		public void DespawnAllPokemon()
		{
			List<Item> pokemonItems = Player.inventory.ToList().FindAll(item => item.type == ModContent.ItemType<CaughtPokemonItem>());

			for (int i = 0; i < pokemonItems.Count; i++)
			{
				if(pokemonItems[i].ModItem is CaughtPokemonItem pokemonItem)
				{
					pokemonItem.DespawnPokemon();
				}
			}

		}

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
			if(onBattle && !npc.boss) return false;

            return base.CanBeHitByNPC(npc, ref cooldownSlot);
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
			if(onBattle) return false;

            return base.CanBeHitByProjectile(proj);
        }
	}
}