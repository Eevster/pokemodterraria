using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class DrillRun : PokemonAttack
	{
        public override bool CanExistIfNotActualMove => false;
        private bool mirrored;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }

		public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 80;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.Opacity = 0.8f;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<DrillRun>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 0f, pokemon.owner)];
						pokemon.velocity = 30*Vector2.Normalize(targetCenter-pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item22, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

        public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            DrillRun proj = (DrillRun)pokemonOwner.attackProjs[i].ModProjectile;

            pokemonOwner.attackProjs[i].Center = pokemon.Center;

            if (pokemonOwner.attackProjs[i].ai[0] == 0)
            {
                if (pokemon.velocity.Length() < 0.1f)
                {
                    pokemonOwner.attackProjs[i].Kill();
                    if (!pokemonOwner.canAttack)
                    {
                        pokemonOwner.timer = 0;
                    }
                }
                else
                {
                    proj.mirrored = pokemon.velocity.X < 0f;
                    pokemonOwner.attackProjs[i].rotation = pokemon.velocity.ToRotation();
                }
            }
        }

        public override void UpdateNoAttackProjs(Projectile pokemon, int i)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            DrillRun proj = (DrillRun)pokemonOwner.attackProjs[i].ModProjectile;

            pokemonOwner.attackProjs[i].Center = pokemon.Center;

            if (pokemonOwner.attackProjs[i].ai[0] == 0)
            {
                if (pokemon.velocity.Length() < 0.1f)
                {
                    pokemonOwner.attackProjs[i].Kill();
                    if (!pokemonOwner.canAttack)
                    {
                        pokemonOwner.timer = 0;
                    }
                }
                else
                {
                    proj.mirrored = pokemon.velocity.X < 0f;
                    pokemonOwner.attackProjs[i].rotation = pokemon.velocity.ToRotation();
                    proj.Projectile.Opacity = 0.8f*Math.Clamp(pokemon.velocity.Length() / 2f, 0f, 1f);
                }
            }
        }

        public override void ExtraChanges(Projectile pokemon)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

            if (!pokemonOwner.canAttack && pokemonOwner.timer > 0)
            {
                if(!Main.player[pokemon.owner].GetModPlayer<PokemonPlayer>().onBattle) pokemonOwner.immune = true;
                pokemon.velocity.Y *= 0.95f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture).Value;
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                texture.Frame(1, Main.projFrames[Projectile.type],
                0,
                Projectile.frame),
                Color.White*Projectile.Opacity,
                Projectile.rotation,
                texture.Frame(1, Main.projFrames[Projectile.type]).Size() / 2f,
                Projectile.scale,
                mirrored ? SpriteEffects.FlipVertically : SpriteEffects.None, 0
                );
            return false;
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
            StartCut(target.Center);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            StartCut(target.Center);
            base.OnHitPlayer(target, info);
        }

        public void StartCut(Vector2 position)
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;
                Projectile.velocity = Vector2.Zero;
                Projectile.Center = position;

                Projectile.hide = false;
                Projectile.frameCounter = 0;
                Projectile.frame = 0;

                Projectile.penetrate = 3;
                Projectile.timeLeft = 35;

                Projectile.Opacity = 0.8f;
                SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
            }

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.Center, 4, 4, DustID.Dirt, Main.rand.Next(2, 15) * (mirrored? -1: 1), Main.rand.Next(-7, -3));
                Dust.NewDust(Projectile.Center, 4, 4, DustID.FireworkFountain_Red, Main.rand.Next(2, 15) * (mirrored? -1: 1), Main.rand.Next(-7, -3));
            }
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Vector2 speedVector = Vector2.UnitX.RotatedBy(Projectile.rotation);
                for (int i = 0; i < 3; i++)
                {
                    Vector2 sparkDirection = Main.rand.Next(-7, -3)*speedVector.RotatedByRandom(MathHelper.ToRadians(45));
                    Vector2 originPos = Projectile.Center + new Vector2(Main.rand.Next(-8, 8), Main.rand.Next(-8, 8)) + 60f*speedVector;
                    int dust = Dust.NewDust(originPos, 4, 4, DustID.Pixie, sparkDirection.X, sparkDirection.Y);
                    Main.dust[dust].noGravity = true;
                    Dust.NewDust(originPos, 4, 4, DustID.Dirt, sparkDirection.X, sparkDirection.Y);
                }

                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    SoundEngine.PlaySound(SoundID.Item22, Projectile.Center);
                }
            }
        }
    }
}
