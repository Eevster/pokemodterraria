using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using System;

namespace Pokemod.Content.Pets
{
	public abstract class PokemonPetBuff : ModBuff
	{
        public override string Texture => "Pokemod/Assets/Textures/Pokesprites/Buffs/"+GetType().Name;
        public virtual string PokeName => "";
        public virtual int ProjType => 0;
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool Shiny = false;
            bool noProj = false;

            if(player.whoAmI == Main.myPlayer){
                if(GetType().Name.Contains("PetBuffShiny")) Shiny = true;

                if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
                    if(player.miscEquips[0].ModItem is CaughtPokemonItem){
                        CaughtPokemonItem pokeItem = (CaughtPokemonItem)player.miscEquips[0].ModItem;
                        if(pokeItem.proj == null || player.ownedProjectileCounts[ProjType] <= 0){
                            if(pokeItem.PokemonName == PokeName && pokeItem.Shiny == Shiny && pokeItem.currentHP != 0){
                                int proj = Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ProjType, 0, 0f, player.whoAmI, pokeItem.currentHP);
                                pokeItem.proj = Main.projectile[proj];
                            }else{
                                noProj = true;
                            }
                        }
                    }
                }
                if(player.ownedProjectileCounts[ProjType] > 0){
                    if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
                        if(player.miscEquips[0].ModItem is CaughtPokemonItem){
                            CaughtPokemonItem pokeItem = (CaughtPokemonItem)player.miscEquips[0].ModItem;
                            if(pokeItem.proj != null){
                                if(pokeItem.PokemonName == PokeName && pokeItem.Shiny == Shiny){
                                    pokeItem.SetPetInfo();
                                    player.buffTime[buffIndex] = 2;
                                    if(pokeItem.currentHP == 0){
                                        noProj = true;
                                    }
                                }
                            }
                        }
                    }
                }
                
                if(player.ownedProjectileCounts[ProjType] <= 0){
                    noProj = true;
                }

                UpdateExtraChanges(player);

                if(noProj){
                    player.DelBuff(buffIndex);
                    buffIndex--;
                }
            }
        }

        public virtual void UpdateExtraChanges(Player player){
            
        }
	}
}
