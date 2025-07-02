using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Splash : PokemonAttack
	{
        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						pokemonOwner.canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20){
					int remainProjs = 1;
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
                            
							//Splash effect on pokemon
                            SoundEngine.PlaySound(SoundID.Splash, pokemon.Center);
                            float splashDirection = 5f * Vector2.Normalize(targetCenter - pokemon.Center).X;
                            for (int j = 0; j < pokemon.width / 2; j++)
							{
                                Dust.NewDust(pokemon.Bottom, pokemon.width, 0, DustID.BreatheBubble, splashDirection, -pokemon.height / 4);
                                Dust.NewDust(pokemon.Bottom, pokemon.width, 0, DustID.Water, splashDirection, -pokemon.height / 4);
                                Dust.NewDust(pokemon.Bottom, pokemon.width, 0, DustID.Water_Snow, splashDirection, -pokemon.height / 4);
                            }

							remainProjs--;
							pokemonOwner.canAttackOutTimer = false;
							if(remainProjs <= 0){
								break;
							}
						}
					} 
				}
			}
		}
    }
}
