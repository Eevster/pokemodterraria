using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Pokemod.Common.Systems;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.NPCs.TrainerNPCs
{
	public abstract class BattleTrainer : ModNPC
	{
		public bool OnBattle = false;

		public virtual int nPokemon => 1;
		public virtual bool randomPokemon => false;
		public virtual bool canRepeat => false;
		public virtual string[] pokemonOptions => ["Magikarp"];
		public int trainerLevel = 5;
		private List<EnemyPokemonInfo> pokemonTeam;

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
				pokemonTeam.RemoveAt(0);

				if(pokemonTeam.Count > 0)SendPokemon(opponent);
				else
				{
					Main.NewText(Language.GetText("Mods.Pokemod.PokemonBattle.BattleWin").WithFormatArgs(NPC.FullName).Value, 237, 206, 2); 
					opponent.GetModPlayer<PokemonPlayer>().SetBattle(false);
					NPC.EncourageDespawn(0);
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

            return base.CanChat();
        }
	}

	internal class EnemyPokemonInfo
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

		public EnemyPokemonInfo(string name, int level, int[] IVs, int[] EVs, int nature, int happiness)
        {
            this.name = name;
			this.level = level;
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
