using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Common.Players;
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
	public class Screech : PokemonAttack
	{
		float animOffset = 0;
		float initialScale = 0.4f;
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetPosition);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPosition = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 80;
            Projectile.penetrate = 1;
            Projectile.Opacity = 0.8f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 14f*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<Screech>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item20, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = 0.5f*texture.Size();

			float finalScale = 0.6f;
			float currentScale = initialScale+animOffset;

			float nWaves = 4f;
			float separation = 1f/nWaves;

            while (currentScale <= finalScale)
            {
                Color color = Projectile.GetAlpha(lightColor)*Projectile.Opacity;

                color *= 0.4f+0.6f*(finalScale-currentScale)/(finalScale-initialScale);

                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition - new Vector2(60f*(currentScale-initialScale)/(finalScale-initialScale), 0).RotatedBy(Projectile.velocity.ToRotation()), texture.Bounds, color, Projectile.rotation, drawOrigin, currentScale*Projectile.scale, SpriteEffects.None, 0);
                currentScale += separation*(finalScale-initialScale);
            }

			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, texture.Bounds, Projectile.GetAlpha(lightColor)*Projectile.Opacity, Projectile.rotation, drawOrigin, initialScale*Projectile.scale, SpriteEffects.None, 0);

            animOffset += 0.02f*(finalScale-initialScale);
			if(animOffset > separation * (finalScale - initialScale))
			{
				animOffset -= separation * (finalScale - initialScale);
			}

			return false;
		}

        public override void AI()
        {
			Projectile.rotation = Projectile.velocity.ToRotation();

			if(Projectile.timeLeft < 10){
				Projectile.Opacity = 0.8f*Projectile.timeLeft*0.1f;
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override void OnHitPokemonPet(PokemonPetProjectile target, int damageDone)
        {
			target.ApplyStatMod(1, 2);
            base.OnHitPokemonPet(target, damageDone);
        }

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center + Projectile.scale*new Vector2(Projectile.width,0).RotatedBy(Projectile.velocity.ToRotation());
			Vector2 end = Projectile.Center - Projectile.scale*new Vector2(60,0).RotatedBy(Projectile.velocity.ToRotation());
			float collisionPoint = 0f; // Don't need that variable, but required as parameter

			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, initialScale*Projectile.scale*Projectile.height, ref collisionPoint);
		}
    }
}
