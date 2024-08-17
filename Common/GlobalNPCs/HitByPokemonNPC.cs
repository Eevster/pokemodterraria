using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.GlobalNPCs
{
    public class HitByPokemonNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public Projectile pokemonProj;

        /*public override void OnKill(NPC npc)
        {
            Main.NewText("OnKill");
            if(pokemonProj != null){
				if(pokemonProj.active){
					if(pokemonProj.ModProjectile is PokemonPetProjectile){
                        PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
                        pokemonMainProj?.SetExtraExp(SetExpGained(npc));
                        Main.NewText(pokemonMainProj.Name);
                    }
				}else{
                    Main.NewText("No Active");
                }
			}else{
                Main.NewText("Null");
            }
            base.OnKill(npc);
        }*/

        public override void HitEffect(NPC npc, NPC.HitInfo hit)
        {
            if(npc.life <= 0){
                if(pokemonProj != null){
                    if(pokemonProj.active){
                        if(pokemonProj.ModProjectile is PokemonPetProjectile){
                            PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
                            pokemonMainProj?.SetExtraExp(SetExpGained(npc));
                            Main.NewText(pokemonMainProj.Name);
                        }
                    }
                }
            }
            base.HitEffect(npc, hit);
        }

        public int SetExpGained(NPC npc){
            int exp = (int)Math.Sqrt(npc.value);
            if(exp < 1) exp = 1;

            return exp;
		}
    }
}