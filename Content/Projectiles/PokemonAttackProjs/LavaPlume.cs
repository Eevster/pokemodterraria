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
	public class LavaPlume : PokemonAttack
	{
		bool exploded = false;
		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
			Main.projFrames[Projectile.type] = 7;
        }
		public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i+=8){
					if(pokemonOwner.attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item14, pokemon.position);
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						for(int j = 0; j < 8; j++){
							pokemonOwner.attackProjs[j] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 15*Vector2.Normalize(targetCenter-pokemon.Center).RotatedBy(MathHelper.ToRadians(j*45f)), ModContent.ProjectileType<LavaPlume>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						}
					}
				} 
			}
			pokemonOwner.timer = pokemonOwner.attackDuration;
			pokemonOwner.canAttack = false;
		}

		public override bool PreDraw(ref Color lightColor) {
			base.PreDraw(ref lightColor);
			
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = texture.Frame(1, 7).Size() / 2f;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Color.Lerp(Projectile.GetAlpha(lightColor), new Color(100, 100, 100), (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length)*((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 7, 0, Projectile.frame), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
		
		public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = (int)(Projectile.damage*0.4f);
            base.OnSpawn(source);
        }

        public override void AI()
		{
			UpdateAnimation();

			Lighting.AddLight(Projectile.Center, new Vector3(0.5f, 0.2f, 0));

			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
			}
		}

		private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 7)
            {
                Projectile.frameCounter = 0;
				Projectile.frame++;
                if(!exploded){
					if (Projectile.frame >= 3)
					{
						Projectile.frame = 0;
					}
				}
				if (Projectile.frame >= Main.projFrames[Projectile.type]){
					Projectile.Kill();
				}
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.OnFire, 6*60);
			if(!exploded){
				if(target.CanBeChasedBy()){
					Projectile.frame = 3;
					exploded = true;
					Projectile.velocity = Vector2.Zero;
				}
			}
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.AddBuff(BuffID.OnFire, 6*60);
			if(!exploded){
				Projectile.frame = 3;
				exploded = true;
				Projectile.velocity = Vector2.Zero;
			}
            base.OnHitPlayer(target, info);
        }
    }
}
