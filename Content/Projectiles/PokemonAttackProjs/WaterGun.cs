using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Pokemod.Content.Pets;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class WaterGun : PokemonAttack
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        private static Asset<Texture2D> trailTexture;
        
        public override void Load()
        { 
            trailTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/WaterGunTrail");
        }

        public override void Unload()
        { 
            trailTexture = null;
        }

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 48;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 300;

            Projectile.tileCollide = true;  
            Projectile.penetrate = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 2;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 18f*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<WaterGun>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item21, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
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
            Vector2 start = Projectile.Center-25f*Projectile.scale*new Vector2(0,-1).RotatedBy(Projectile.rotation);
            Vector2 end = Projectile.Center+25f*Projectile.scale*new Vector2(0,-1).RotatedBy(Projectile.rotation);
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 10f*Projectile.scale, ref collisionPoint);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item21, Projectile.position);

            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, 20, 20, DustID.Water, 0f, 0f, 100, default(Color), 3f);
                
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3f;
            }
        }

        public override bool PreDraw(ref Color lightColor) {
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = trailTexture.Value;

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
    }
}