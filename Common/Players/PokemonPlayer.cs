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

namespace Pokemod.Common.Players
{
    public class PokemonPlayer : ModPlayer
	{
		public int attackMode;
		public enum AttackMode
        {
			Auto_Attack,
            No_Attack,
            Directed_Attack
        }
		public Vector2 attackPosition;
		public NPC targetNPC;
		private static Asset<Texture2D> targetTexture;

		public override void Load()
        { 
            targetTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/PokemonTarget");
        }

        public override void Unload()
        { 
            targetTexture = null;
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
            if(targetNPC != null){
				if(targetNPC.active){
					attackPosition = targetNPC.Center;
				}else{
					targetNPC = null;
				}
			}
        }

        public void ChangeAttackMode(int mode){
			attackMode = mode;
			SoundEngine.PlaySound(SoundID.Item1, Player.position);

			targetNPC = null;

			if(attackMode == (int)AttackMode.Directed_Attack){
				attackPosition = Main.MouseWorld;
				SearchNPCTarget(16);
			}
		}

		public void SearchNPCTarget(float maxDetectDistance){
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxNPCs; k++) {
				NPC target = Main.npc[k];

				if (target.CanBeChasedBy()) {
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, attackPosition);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance) {
						sqrMaxDetectDistance = sqrDistanceToTarget;
						targetNPC = target;
					}
				}
			}
		}

		public string GetAttackModeText(int mode){
			return (""+(AttackMode)mode).Replace("_"," ");
		}

        public override void PostBuyItem(NPC vendor, Item[] shopInventory, Item item)
        {
			if(item.type == ModContent.ItemType<CharmanderPetItem>() || item.type == ModContent.ItemType<BulbasaurPetItem>() || item.type == ModContent.ItemType<SquirtlePetItem>()){
				int ballItem;
				string pokemonName = "";
				
				if(item.type == ModContent.ItemType<CharmanderPetItem>()) pokemonName = "Charmander";
				if(item.type == ModContent.ItemType<BulbasaurPetItem>()) pokemonName = "Bulbasaur";
				if(item.type == ModContent.ItemType<SquirtlePetItem>()) pokemonName = "Squirtle";

				if (Main.netMode == NetmodeID.SinglePlayer){
					ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
					CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
					pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
				}else{
					if(Main.netMode == NetmodeID.Server){
						ballItem = Player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<CaughtPokemonItem>());
						CaughtPokemonItem pokeItem = (CaughtPokemonItem)Main.item[ballItem].ModItem;
						pokeItem.SetPokemonData(pokemonName, Shiny: false, BallType: "PokeballItem");
					}
				}

				item.TurnToAir();
			}
            base.PostBuyItem(vendor, shopInventory, item);
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
    }
}