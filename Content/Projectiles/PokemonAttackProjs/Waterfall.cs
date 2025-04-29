using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Waterfall : PokemonAttack
	{
        Vector2 initialPosition;
		
		private static Asset<Texture2D> chainTexture;
        
        public override void Load()
        { 
            chainTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/WaterfallChain");
        }

        public override void Unload()
        { 
            chainTexture = null;
        }

		public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 80;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
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

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 5){
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
							pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter + new Vector2(0,22), new Vector2(0,-10), ModContent.ProjectileType<Waterfall>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 6f, pokemon.owner)];
							SoundEngine.PlaySound(SoundID.Item21, pokemon.position);
							pokemonOwner.canAttackOutTimer = false; 
							break;
						}
					}
				}
			}
		}

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 center = Projectile.Center;
            Vector2 directionToOrigin = initialPosition - center;

            float distanceToOrigin = directionToOrigin.Length();
            int frameAux = 0;

            while (distanceToOrigin > chainTexture.Width() && !float.IsNaN(distanceToOrigin))
            {
                directionToOrigin /= distanceToOrigin; 
                directionToOrigin *= chainTexture.Width(); 

                center += directionToOrigin; 
                directionToOrigin = initialPosition - center; 
                distanceToOrigin = directionToOrigin.Length();

                Main.EntitySpriteDraw(chainTexture.Value, center - Main.screenPosition,
                    chainTexture.Frame(1,3,0,2-((Projectile.timeLeft)/5)%3), lightColor*Projectile.Opacity, 0,
                    chainTexture.Frame(1,3).Size() / 2f, 1f, SpriteEffects.None, 0);
                
                frameAux++;
            }

            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Main.EntitySpriteDraw(texture, initialPosition - new Vector2(0,4)  - Main.screenPosition,
                texture.Bounds, lightColor*Projectile.Opacity, Projectile.rotation,
                texture.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

            base.PostDraw(lightColor);
        }

        public override void OnSpawn(IEntitySource source)
        {
            initialPosition = Projectile.Center + new Vector2(0,4);
            base.OnSpawn(source);
        }

        public override void AI()
        {
			if(Vector2.Distance(Projectile.Center, initialPosition) > 300){
                Projectile.velocity = Vector2.Zero;
            }

            if(Projectile.timeLeft < 20){
                Projectile.Opacity = Projectile.timeLeft*0.05f;
            }

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, initialPosition + new Vector2(0,18), 44f, ref collisionPoint);
		}
    }
}
