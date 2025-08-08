using Microsoft.Xna.Framework;
using Pokemod.Common.GlobalNPCs;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Splash : PokemonAttack
	{
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hide = true;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.knockBack = 0f;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            base.SetDefaults();
        }

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

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if (pokemon.owner == Main.myPlayer)
			{
				if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20)
				{
					for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
					{
						if (pokemonOwner.attackProjs[i] == null)
						{
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter, Vector2.Zero, ModContent.ProjectileType<Splash>(), 0, 0f, pokemon.owner)];
                            float splashDirection = 5f * (targetCenter - pokemon.Center).SafeNormalize(Vector2.Zero).X;
                            //Splash effect on pokemon
							SplashEffect(pokemon.Bottom, splashDirection);
							pokemonOwner.canAttackOutTimer = false;
							break;
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

        public bool SetExpTarget(out NPC target)
        {
            target = null;
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc != null)
                    {
                        if (npc.CanBeChasedBy() || npc.CountsAsACritter || npc.ModNPC is PokemonWildNPC)
                        {
                            if (Projectile.Hitbox.Intersects(npc.getRect()))
                            {
                                target = npc;
                                break;
                            }
                        }
                    }
                }
                if (target != null)
                {
                    if (pokemonProj != null)
                    {
                        if (pokemonProj.active)
                        {
                            if (target.life <= 0 && target.GetGlobalNPC<HitByPokemonNPC>().pokemonProj != pokemonProj)
                            {
                                PokemonPetProjectile pokemonMainProj = (PokemonPetProjectile)pokemonProj?.ModProjectile;
                                pokemonMainProj?.SetExtraExp(HitByPokemonNPC.SetExpGained(target));
                            }
                            target.GetGlobalNPC<HitByPokemonNPC>().pokemonProj = pokemonProj;

                            return true;
                        }
                    }
                }   
            }
            return false;
        }

        public override void AI()
        {
            if (SetExpTarget(out NPC target))
            {
                float splashDirection = 5f * (target.Center - pokemonProj.Center).SafeNormalize(Vector2.Zero).X;
                SplashEffect(target.Bottom, splashDirection);
            }

            Projectile.Kill();

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }
    }
}
