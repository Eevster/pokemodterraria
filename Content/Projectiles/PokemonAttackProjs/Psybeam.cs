using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Dusts;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Psybeam : PokemonAttack
    {
        Vector2 initialPosition;
        int nParts = 0;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(initialPosition);
            writer.Write((double)Projectile.rotation);
            writer.Write((double)Projectile.Opacity);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            initialPosition = reader.ReadVector2();
            Projectile.rotation = (float)reader.ReadDouble();
            Projectile.Opacity = (float)reader.ReadDouble();
            base.ReceiveExtraAI(reader);
        }
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 90;

            Projectile.tileCollide = false;  
            Projectile.penetrate = 3;
            Projectile.Opacity = 1f;
            Projectile.light = 1f;

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
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center + 20f*Vector2.Normalize(targetCenter-pokemon.Center), 18f*Vector2.Normalize(targetCenter-pokemon.Center), ModContent.ProjectileType<Psybeam>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item46 with { Pitch = 0.3f, Volume = 0.7f}, pokemon.position);
                        SoundEngine.PlaySound(SoundID.Item75 with { Pitch = 0.4f, Volume = 0.8f }, pokemon.position);
                        pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

        public override void OnSpawn(IEntitySource source)
        {
            initialPosition = Projectile.Center;

            base.OnSpawn(source);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.Lerp(Color.Pink, Color.LightBlue, 0.5f*((float)Math.Sin(Projectile.frame*MathHelper.PiOver2)+1f)) * Projectile.Opacity;
        }

        public override bool PreDrawExtras()
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            if (Projectile.timeLeft < 90)
            {
                Vector2 center = Projectile.Center;
                if (Vector2.Distance(center, initialPosition) > texture.Width * 15) initialPosition = center + texture.Width * 15 * Vector2.Normalize(initialPosition - center);
                Vector2 directionToOrigin = initialPosition - Projectile.Center;

                nParts = 0;

                float distanceToOrigin = directionToOrigin.Length();

                while (distanceToOrigin > texture.Width && !float.IsNaN(distanceToOrigin))
                {
                    directionToOrigin /= distanceToOrigin;
                    directionToOrigin *= texture.Width;

                    center += directionToOrigin;
                    directionToOrigin = initialPosition - center;
                    distanceToOrigin = directionToOrigin.Length();

                    //Color drawColor = Lighting.GetColor((int)(center.X / 16), (int)(center.Y / 16));
                    Color drawColor = Color.Lerp(Color.Pink, Color.LightBlue, 0.5f * ((float)Math.Sin(((Projectile.frame + nParts) % 4) * MathHelper.PiOver2) + 1f));

                    nParts++;

                    Main.EntitySpriteDraw(texture, center - Main.screenPosition,
                        texture.Frame(1, 4, 0, (Projectile.frame + nParts) % 4), drawColor * Projectile.Opacity, Projectile.rotation,
                        texture.Frame(1, 4).Size() / 2f, 1f, SpriteEffects.None, 0);
                }
            }

            return false;
        }

        public override void AI()
        {
            if(Projectile.timeLeft <= 10){
				Projectile.Opacity = 1f*Projectile.timeLeft/10;
                Projectile.velocity *= 0.9f;
			}

            if(Projectile.timeLeft % 10 == 1)
            {
                SoundEngine.PlaySound(SoundID.Item46 with { Pitch = 0.2f, Volume = Projectile.timeLeft/140f }, Projectile.position);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            UpdateAnimation();

            if (Main.myPlayer == Projectile.owner){
                Projectile.netUpdate = true;
            }
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.Center - (40*nParts+20)*new Vector2(1,0).RotatedBy(Projectile.rotation);
			Vector2 end = Projectile.Center + 20*new Vector2(1,0).RotatedBy(Projectile.rotation);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 40, ref collisionPoint);
		}
    }
}
