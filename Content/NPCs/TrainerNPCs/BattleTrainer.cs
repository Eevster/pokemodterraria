using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Pokemod.Common.Systems;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.DataStructures;

namespace Pokemod.Content.NPCs.TrainerNPCs
{
	public abstract class BattleTrainer : ModNPC
	{
		private static Profiles.StackedNPCProfile NPCProfile;
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 26; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 10; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 0;
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.PrettySafe[Type] = 300;
			NPCID.Sets.AttackType[Type] = -1; // magic attack.
			NPCID.Sets.AttackTime[Type] = 0; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 0;
			NPCID.Sets.HatOffsetY[Type] = 0; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ActsLikeTownNPC[Type] = true;
			NPCID.Sets.NoTownNPCHappiness[Type] = true;
			NPCID.Sets.SpawnsWithCustomName[Type] = true;

			NPCID.Sets.AllowDoorInteraction[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = -1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, -1)
			// new Profiles.DefaultNPCProfile(Texture + "_Shimmer", -1)
			);
		}

		public override void SetDefaults()
		{
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 0;
			NPC.defense = 50;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.PlayerHit;
			NPC.DeathSound = SoundID.PlayerKilled;
			NPC.knockBackResist = 0f;

			AnimationType = NPCID.Demolitionist;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return NPCProfile;
		}

		public bool OnBattle = false;

		public virtual bool GymLeader => false;

		public virtual int nPokemon => 1;
		public virtual bool randomPokemon => false;
		public virtual bool canRepeat => false;
		public virtual string[] pokemonOptions => ["Magikarp"];
		public int trainerLevel = 5;
		public List<EnemyPokemonInfo> pokemonTeam;

		public Player opponent;

		public void StartBattle(Player player)
		{
			opponent = player;
			Main.NewText(Language.GetText("Mods.Pokemod.PokemonBattle.BattleStart").WithFormatArgs(NPC.FullName).Value); 

			if (opponent.GetModPlayer<PokemonPlayer>().SetBattle(true))
			{
				OnBattle = true;

            	Main.CloseNPCChatOrSign();
				Main.npcChatText = string.Empty;
				opponent.SetTalkNPC(0);

				LoadTeam();
				SendPokemon(opponent);
			}
		}

        public override void OnSpawn(IEntitySource source)
        {
			trainerLevel = Main.rand.Next(Math.Max(WorldLevel.MaxWorldLevel-15,5), Math.Min(100,WorldLevel.MaxWorldLevel+10)+1);
            base.OnSpawn(source);
        }

		public virtual void LoadTeam()
		{
			pokemonTeam = new List<EnemyPokemonInfo>();

			for(int i = 0; i < nPokemon; i++)
			{
				pokemonTeam.Add(new EnemyPokemonInfo(pokemonOptions[Main.rand.Next(pokemonOptions.Length)], Main.rand.Next(trainerLevel-2, trainerLevel+1)));
			}
		}

		private void SendPokemon(Player player)
		{
			int projIndex = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(3f*Math.Sign(player.Center.X-NPC.Center.X), -3f), ModContent.Find<ModProjectile>("Pokemod", pokemonTeam[0].name+"PetProjectile").Type, 0, 0, player.whoAmI, 10000);
			Projectile proj = Main.projectile[projIndex];

			PokemonPetProjectile PokemonProj = null;
			if(proj.ModProjectile is PokemonPetProjectile){
				PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
			}
			
			PokemonProj?.SetPokemonLvl(pokemonTeam[0].level, pokemonTeam[0].IVs, pokemonTeam[0].EVs, pokemonTeam[0].nature, pokemonTeam[0].happiness);
			PokemonProj?.SetAsEnemyPokemon(NPC, pokemonTeam[0].moveSet);
		}

		public void FaintedPokemon()
		{
			if(pokemonTeam.Count > 0)
			{
				Main.NewText(Language.GetText("Mods.Pokemod.PokemonBattle.EnemyPokemonFainted").WithFormatArgs(pokemonTeam[0].name).Value, 237, 143, 2);
				var opponentPokemon = Main.projectile[opponent.GetModPlayer<PokemonPlayer>().currentActivePokemon[0]];
				if(opponentPokemon.ModProjectile is PokemonPetProjectile activePokemon)
				{
					activePokemon.SetGainedExp((int)(100f * pokemonTeam[0].level / 7f));
				}
				pokemonTeam.RemoveAt(0);

				if(pokemonTeam.Count > 0)SendPokemon(opponent);
				else
				{
					Main.NewText(Language.GetText("Mods.Pokemod.PokemonBattle.BattleWin").WithFormatArgs(NPC.FullName).Value, 237, 206, 2);
					opponent.GetModPlayer<PokemonPlayer>().SetBattle(false);
					NPC.active = false;
					NPC.netSkip = -1;
					NPC.life = 0;
				}
			}
		}

        public override void PostAI()
        {
            base.PostAI();
			if(opponent != null)
			{
				if(opponent.dead || !opponent.GetModPlayer<PokemonPlayer>().onBattle)
				{
					opponent = null;
					OnBattle = false;
				}
			}

			if(OnBattle) NPC.velocity.X = 0;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
			if(OnBattle) return false;

            return base.CanBeHitByItem(player, item);
        }

        public override bool CanBeHitByNPC(NPC attacker)
        {
			if(OnBattle) return false;

            return base.CanBeHitByNPC(attacker);
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
			if(OnBattle) return false;

            return base.CanBeHitByProjectile(projectile);
        }

        public override bool CanChat()
        {
			if(OnBattle) return false;

            return true;
        }
	}

	public class EnemyPokemonInfo
    {
        public string name;
		public int level;
		public string[] moveSet;
		public int[] IVs;
		public int[] EVs;
		public int nature;
		public int happiness;

        public EnemyPokemonInfo(string name, int level)
        {
            this.name = name;
			this.level = level;
			moveSet = GetPokemonMoves(name, level);
			IVs = PokemonNPCData.GenerateIVs();
			EVs = [0,0,0,0,0,0];
			nature = 10 * Main.rand.Next(5) + Main.rand.Next(5);
			happiness = 100;
        }

		public EnemyPokemonInfo(string name, int level, string[] moveSet)
        {
            this.name = name;
			this.level = level;
			this.moveSet = moveSet;
			IVs = PokemonNPCData.GenerateIVs();
			EVs = [0,0,0,0,0,0];
			nature = 10 * Main.rand.Next(5) + Main.rand.Next(5);
			happiness = 100;
        }

		public EnemyPokemonInfo(string name, int level, int[] IVs, int[] EVs, int nature, int happiness)
        {
            this.name = name;
			this.level = level;
			moveSet = GetPokemonMoves(name, level);
			this.IVs = IVs;
			this.EVs = EVs;
			this.nature = nature;
			this.happiness = happiness;
        }

		public EnemyPokemonInfo(string name, int level, string[] moveSet, int[] IVs, int[] EVs, int nature, int happiness)
        {
            this.name = name;
			this.level = level;
			this.moveSet = moveSet;
			this.IVs = IVs;
			this.EVs = EVs;
			this.nature = nature;
			this.happiness = happiness;
        }

		private string[] GetPokemonMoves(string PokemonName, int level)
		{
			List<MoveLvl> newMoveList = PokemonData.pokemonInfo[PokemonName].movePool.ToList();

			List<string> moveSet = [];

			while (newMoveList.Count > 0)
			{
				if (newMoveList[0].levelToLearn > level) break;

				if (!moveSet.Contains(newMoveList[0].moveName))
				{
                    if (moveSet.Count >= 4)
					{
						newMoveList.RemoveAt(Main.rand.Next(newMoveList.Count));
					}
					moveSet.Add(newMoveList[0].moveName);
				}
				newMoveList.RemoveAt(0);
			}

			return moveSet.ToArray();
		}
    }
}
