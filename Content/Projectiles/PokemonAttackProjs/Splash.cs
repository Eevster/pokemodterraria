using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Splash : PokemonAttack
	{
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";

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
                            float splashDirection = 5f * Vector2.Normalize(targetCenter - pokemon.Center).X;
                            SplashEffect(pokemon.Bottom, splashDirection);
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

        private void SplashEffect(Vector2 position, float direction)
        {
            if (!Main.dedServ)
            {
                SoundEngine.PlaySound(SoundID.Splash, position);

                for (int j = 0; j < 32 / 2; j++)
                {
                    Dust.NewDust(position, 32, 1, DustID.BreatheBubble, direction, -6);
                    Dust.NewDust(position, 32, 1, DustID.Water, direction, -6);
                    Dust.NewDust(position, 32, 1, DustID.Water_Snow, direction, -6);
                }
            }

        }
    }
}
