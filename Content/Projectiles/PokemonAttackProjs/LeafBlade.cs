using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Pokemod.Content.Dusts;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class LeafBlade : PokemonAttack
	{
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }
		public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;
            Projectile.CritChance = 13;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 12;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<LeafBlade>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 0f, pokemon.owner)];
						pokemon.velocity = 36*Vector2.Normalize(targetCenter-pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].ai[0] == 0) pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 0.1f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemonOwner.attackProjs[i].ai[0] == 0) pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 0.1f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}
        
        public override void ExtraChanges(Projectile pokemon){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(!pokemonOwner.canAttack && pokemonOwner.timer > 0){
				if(!Main.player[pokemon.owner].GetModPlayer<PokemonPlayer>().onBattle) pokemonOwner.immune = true;
                pokemon.velocity.Y *= 0.95f;
            }
        }

        public override void AI()
        {
            UpdateAnimation();

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = 1f;
            base.OnHitNPC(target, hit, damageDone);
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if(Projectile.frame < Main.projFrames[Projectile.type]) Projectile.frame++;
                if (Projectile.frame == 3)
                {
                    Projectile.ai[0] = 1;
                    SoundEngine.PlaySound(SoundID.DD2_SonicBoomBladeSlash with { Volume = 0.5f }, Projectile.position);
                    for (int i = 0; i < 20; i++)
                    {
                        int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(2, 2), 4, 4, ModContent.DustType<LeafDust>(), 0, 0, 50, default(Color), 1f);
                        Main.dust[dustIndex].velocity = new Vector2(Main.rand.NextFloat(5, 10), 0).RotatedByRandom(MathHelper.TwoPi);
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
            }
        }
    }
}
