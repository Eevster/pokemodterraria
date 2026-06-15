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
	public class Bonemerang : PokemonAttack
	{
        const float projSpeed = 25f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
        }

		public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 300;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 4;

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
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, projSpeed*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<Bonemerang>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner, distanceFromTarget/projSpeed)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
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
			Vector2 drawOrigin = texture.Size() / 2f;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, texture.Bounds, color, Projectile.oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void AI()
        {
            if(++Projectile.frameCounter > Projectile.ai[0])
            {
                Projectile.velocity += 2f*(pokemonProj.Center-Projectile.Center).SafeNormalize(Vector2.Zero);

                if(Projectile.velocity.Length() > projSpeed)
                {
                    Projectile.velocity = projSpeed*Projectile.velocity.SafeNormalize(Vector2.Zero);
                }

                if(Vector2.Distance(Projectile.Center, pokemonProj.Center) < Math.Max(Projectile.velocity.Length(),20))
                {
                    Projectile.Kill();
                }
            }

            if(Projectile.frameCounter%30 == 0) SoundEngine.PlaySound(SoundID.Item1, Projectile.position);

            Projectile.rotation += MathHelper.ToRadians(30);

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item54, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Projectile.Center-0.5f*new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.DirtSpray, Main.rand.NextFloat(-2,2), Main.rand.NextFloat(-2,2), 0, default(Color), 1f);
            }
        }
    }
}
