using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Content.NPCs.TrainerNPCs
{
	public abstract class BattleTrainer : ModNPC
	{
		public virtual int nPokemon => 3;
		public virtual string[] pokemonOptions => ["Magikarp", "Poliwag", "Shellder"];
		public List<string> pokemonTeam;

		public void StartBattle()
		{
			Player player = Main.player[Main.myPlayer];

			if (player.GetModPlayer<PokemonPlayer>().SetBattle(true))
			{
				Main.NewText("Starting Battle!");

            	Main.CloseNPCChatOrSign();
				Main.npcChatText = string.Empty;

				LoadTeam();
				SendPokemon(player);
			}
		}

		private void LoadTeam()
		{
			pokemonTeam = new List<string>();

			for(int i = 0; i < nPokemon; i++)
			{
				pokemonTeam.Add(pokemonOptions[Main.rand.Next(pokemonOptions.Length)]);
			}
		}

		private void SendPokemon(Player player)
		{
			int projIndex = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(3f*Math.Sign(player.Center.X-NPC.Center.X), -3f), ModContent.Find<ModProjectile>("Pokemod", pokemonTeam[0]+"PetProjectile").Type, 0, 0, player.whoAmI, 10000);
			Projectile proj = Main.projectile[projIndex];

			PokemonPetProjectile PokemonProj = null;
			if(proj.ModProjectile is PokemonPetProjectile){
				PokemonProj = (PokemonPetProjectile)proj?.ModProjectile;
			}
			
			PokemonProj?.SetPokemonLvl(10, [0,0,0,0,0,0], [0,0,0,0,0,0], 0, 0);
			PokemonProj?.SetAsEnemyPokemon(NPC);
		}

		public void FaintedPokemon()
		{
			Player player = Main.player[Main.myPlayer];

			if(pokemonTeam.Count > 0)
			{
				Main.NewText("Enemy " + pokemonTeam[0] + " fainted!");
				pokemonTeam.RemoveAt(0);

				if(pokemonTeam.Count > 0)SendPokemon(player);
				else
				{
					Main.NewText(NPC.FullName + " was defeated! You won!");
					player.GetModPlayer<PokemonPlayer>().SetBattle(false);
					NPC.EncourageDespawn(0);
				}
			}
		}
	}
}
