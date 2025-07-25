using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Buffs;
using Pokemod.Content.Pets;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class DragonBreath : PokemonAttack
	{
        public override void SetDefaults()
        {
            Projectile.width = 90;
            Projectile.height = 38;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;

            Projectile.tileCollide = true; 

            Projectile.timeLeft = 45;
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

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer%4==0){
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){

                            Vector2 spray = new Vector2(Main.rand.NextFloat(2f)-1f, Main.rand.NextFloat(2f)-1f) * 0.2f * Math.Clamp((pokemonOwner.timer / 90f - 0.1f), 0f, 1f);
                            float speed = 15f * (1.2f - (float)Math.Pow(pokemonOwner.timer / 90f, 2f));

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, speed*(Vector2.Normalize(targetCenter-pokemon.Center) + spray), ModContent.ProjectileType<DragonBreath>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 4f, pokemon.owner, Main.rand.NextBool()? 1: 0)];
							if(pokemonOwner.timer > 85){
								SoundEngine.PlaySound(SoundID.Item119, pokemon.position);
							}
							break;
						}
					} 
				}
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture).Value;
            base.PreDraw(ref lightColor);
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                    texture.Frame(1, 1, 0, 0), Color.White, Projectile.rotation,
                    texture.Frame(1, 1).Size() / 2f, Projectile.scale, Projectile.ai[0]==0 ? SpriteEffects.None: SpriteEffects.FlipVertically);

                return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = (int)(Projectile.damage*0.25f); 
            Projectile.Opacity = 1f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            base.OnSpawn(source);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();

            int dustindex = Dust.NewDust(Projectile.Center, 40, 40, Main.rand.NextBool() ? DustID.YellowTorch: DustID.PurpleTorch, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 0, default, 2f);
            Main.dust[dustindex].noGravity = true;

            if(Projectile.velocity.Length() < 3f && Projectile.timeLeft > 5)
            {
                Projectile.timeLeft = 5;
            }


            if (Projectile.timeLeft < 5)
            {
                Projectile.Opacity -= 0.1f;
            }

            if (Main.myPlayer == Projectile.owner){
                Projectile.netUpdate = true;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.Y = 0;

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = 0;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = 0;
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<ParalizedDebuff>(), (target.boss ? 2 : 3) * 60);
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Main.rand.NextBool(5))
            {
                target.AddBuff(ModContent.BuffType<ParalizedDebuff>(), 2 * 60);
            }
            base.OnHitPlayer(target, info);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            fallThrough = true;
			
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
    }
}
