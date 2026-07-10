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
	public class QuickAttack : PokemonAttack
	{
		public override bool CanExistIfNotActualMove => false;

		public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/DoubleTeam";

		public bool didHit = false;

		Vector2 pokeVel;

		public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

		public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;
            
            Projectile.knockBack = 4f;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;

			Projectile.Opacity = 0.6f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			Projectile.hide = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<QuickAttack>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 2, 0f, pokemon.owner)];
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

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 0.2f){
				pokemonOwner.attackProjs[i].Kill();
				if(!pokemonOwner.canAttack){
					pokemonOwner.timer = 0;
				}
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			if(pokemon.velocity.Length() < 0.2f){
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

		public override bool PreDraw(ref Color lightColor) {
			if(pokeVel.Length() > 5f){
				Main.instance.LoadProjectile(Projectile.type);
				Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

				// Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = texture.Size() / 2f;
				for (int k = 2; k < Projectile.oldPos.Length; k++) {
					Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * Projectile.Opacity;
					Main.EntitySpriteDraw(texture, drawPos, texture.Bounds, color, pokeVel.ToRotation(), drawOrigin, Projectile.scale, SpriteEffects.None, 0);
				}
			}

			return false;
		}

        public override void AI()
        {
            if(Main.rand.NextBool())
            {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Firework_Blue, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 100, default(Color), 0f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = Main.rand.NextFloat(0.5f,1.5f);
			}

			if(pokemonProj != null && pokemonProj.active && pokemonProj.velocity.Length() > 0.2f)
			{
				pokeVel = pokemonProj.velocity;
			}

            if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override void AfterHitTarget(Entity target, int damageDone)
        {
			if (!didHit)
			{
				if(pokemonProj != null && pokemonProj.active && pokemonProj.ModProjectile is PokemonPetProjectile)
                {
                    pokemonProj.velocity *= -0.5f;
                }
				didHit = true;
			}
            base.AfterHitTarget(target, damageDone);
        }

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
        }
    }
}
