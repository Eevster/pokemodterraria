using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Pets;
using Terraria.GameContent;
using Pokemod.Content.NPCs;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class EnergyBall : PokemonAttack
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2; // The recording mode
            
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = true;  
            Projectile.penetrate = 1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 20f*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<EnergyBall>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item105, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(20);

            int dustIndex = Dust.NewDust(Projectile.Center - new Vector2(10, 10), 20, 20, DustID.PoisonStaff, 0.5f * Projectile.velocity.X, 0.5f * Projectile.velocity.Y, 50, default(Color), 1f);
            Main.dust[dustIndex].noGravity = true;

            UpdateAnimation();
        }
        
        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >=  Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                    SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack with {Volume = 0.5f}, Projectile.position);
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 4;
            height = 4;
            fallThrough = true;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center-19f*Projectile.scale*new Vector2(0,-1).RotatedBy(Projectile.rotation);
            Vector2 end = Projectile.Center+19f*Projectile.scale*new Vector2(0,-1).RotatedBy(Projectile.rotation);
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 24f*Projectile.scale, ref collisionPoint);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, 20, 20, DustID.PoisonStaff, 0f, 0f, 50, default(Color), 2f);
                
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3f;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = texture.Frame(1, 5).Size() / 2f;
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, texture.Frame(1, 5, 0, Projectile.frame), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}

        public override void OnHitPokemonPet(PokemonPetProjectile target, int damageDone)
        {
            if (Main.rand.NextBool(10))
            {
                target.ApplyStatMod(3, -1); //Special Attack Down
            }
            base.OnHitPokemonPet(target, damageDone);
        }
    }
}