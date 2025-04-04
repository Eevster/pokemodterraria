using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace Pokemod.Common.Players
{
    public class PokemonPlayer : ModPlayer
	{
		public bool HasStarter;
		//Accessories
		public string MegaStone;
		public int HasMegaStone;
		public bool HasShinyCharm;
		public int HasEverstone;

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
		private const int directedEmptyMaxTime = 3*60;
		public NPC targetNPC;
		public Player targetPlayer;

		//Player data
		public string TrainerID { get; internal set; }

		private static Asset<Texture2D> targetTexture;

		public override void Load()
        { 
            targetTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/PokemonTarget");
        }

        public override void Unload()
        { 
            targetTexture = null;
        }

		public override void SaveData(TagCompound tag)
        {
			tag["TrainerID"] = TrainerID;
			tag["HasStarter"] = HasStarter;
        }

        public override void LoadData(TagCompound tag)
        {
			TrainerID = tag.GetString("TrainerID");
			HasStarter = tag.GetBool("HasStarter");
        }

        public override void Initialize()
        {
			SetTrainerID();
            base.Initialize();
        }

		private void SetTrainerID(){
			if(Player.name == null){
				return;
			}else{
				if(Player.name == "") return;
			}

			string id = Player.name;
			for(int i = 0; i < 9; i++){
				id += ""+Main.rand.Next(10);
			}

			TrainerID = id;
		}

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer) {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PokemodMessageType.PokemonPlayerSync);
			packet.Write((byte)Player.whoAmI);
            
            packet.Write((byte)attackMode);
			packet.WriteVector2(attackPosition);
            
			packet.Send(toWho, fromWho);
		}

        public void ReceivePlayerSync(BinaryReader reader) {
			attackMode = reader.ReadByte();
            attackPosition = reader.ReadVector2();
		}

		public override void CopyClientState(ModPlayer targetCopy) {
            PokemonPlayer clone = (PokemonPlayer)targetCopy;
            clone.attackMode = attackMode;
            clone.attackPosition = attackPosition;
		}

		public override void SendClientChanges(ModPlayer clientPlayer) {
            PokemonPlayer clone = (PokemonPlayer)clientPlayer;

			if (clone.attackMode != attackMode || clone.attackPosition != attackPosition)
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}

        public override void ResetEffects()
        {
			if(TrainerID == null){
				SetTrainerID();
			}else{
				if(TrainerID == ""){
					SetTrainerID();
				}
			}

            if(targetNPC != null){
				if(targetNPC.active){
					if(attackMode==(int)AttackMode.Directed_Attack) directedEmptyTimer = 0;
					attackPosition = targetNPC.Center;
				}else{
					targetNPC = null;
				}
			}

			if(targetPlayer != null){
				if(targetPlayer.active){
					if(attackMode==(int)AttackMode.Directed_Attack) directedEmptyTimer = 0;
					attackPosition = targetPlayer.Center;
				}else{
					targetPlayer = null;
				}
			}
			if(attackMode==(int)AttackMode.Directed_Attack){
				if(directedEmptyTimer > 0) directedEmptyTimer--;
				if(targetPlayer == null && targetNPC == null){
					if(directedEmptyTimer <= 0) ChangeAttackMode((int)AttackMode.Auto_Attack);
				}
			}

			HasShinyCharm = false;
			if(HasEverstone > 0) HasEverstone--;
			if(HasMegaStone > 0) HasMegaStone--;
			else MegaStone = "";
        }

        public void ChangeAttackMode(int mode){
			directedEmptyTimer = 0;
			attackMode = mode;
			SoundEngine.PlaySound(SoundID.Item1, Player.position);

			targetNPC = null;
			targetPlayer = null;

			if(attackMode == (int)AttackMode.Directed_Attack){
				attackPosition = Main.MouseWorld;
				SearchNPCTarget(32);
				directedEmptyTimer = directedEmptyMaxTime;
			}
		}

		public void SearchNPCTarget(float maxDetectDistance){
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			targetNPC = null;
			targetPlayer = null;

			for (int k = 0; k < Main.maxPlayers; k++) {
				if(Main.player[k] != null){
					Player target = Main.player[k];
					if(target.whoAmI != Player.whoAmI){
						if(target.active && !target.dead){
							float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, attackPosition);

							// Check if it is within the radius
							if (sqrDistanceToTarget < sqrMaxDetectDistance) {
								if(target.hostile){
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
			if(targetPlayer == null){
				for (int k = 0; k < Main.maxNPCs; k++) {
					NPC target = Main.npc[k];

					if (target.CanBeChasedBy()) {
						// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
						float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, attackPosition);

						// Check if it is within the radius
						if (sqrDistanceToTarget < sqrMaxDetectDistance) {
							sqrMaxDetectDistance = sqrDistanceToTarget;
							targetNPC = target;
							targetPlayer = null;
						}
					}
				}
			}
		}

		public string GetAttackModeText(int mode){
			return (""+(AttackMode)mode).Replace("_"," ");
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
                else
                {
                    if (Main.netMode == NetmodeID.Server)
                    {
                        ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
                        CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
                        pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
                    }
                }

                item.TurnToAir();
            }
            base.PostBuyItem(vendor, shopInventory, item);
        }

		public void GenerateCaughtPokemon(string pokemonName){
			int ballItem;
			bool shiny = Main.rand.NextBool(4096);

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
				CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
				pokeItem.SetPokemonData(pokemonName, Shiny: shiny, BallType: "PokeballItem");
			}
			else
			{
				if (Main.netMode == NetmodeID.Server)
				{
					ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: shiny, BallType: "PokeballItem");
				}
			}
		}

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
			if(Player.whoAmI == Main.myPlayer){
				if(Vector2.Distance(Player.Center,attackPosition) < 3000 && attackMode == (int)AttackMode.Directed_Attack){
					Main.EntitySpriteDraw(targetTexture.Value, attackPosition - Main.screenPosition, targetTexture.Value.Bounds, Color.White*0.8f, 0, targetTexture.Size() * 0.5f, 1, SpriteEffects.None, 0);
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
			if(Player.miscEquips[0] != null && !Player.miscEquips[0].IsAir){
				if (Player.miscEquips[0].ModItem is CaughtPokemonItem pokeItem)
				{
					PokeHealerTile.HealPokemon(pokeItem, 1, ref healed);
				}
			}
            base.UpdateDead();
        }
    }
}