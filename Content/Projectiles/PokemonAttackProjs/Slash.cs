using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Dusts;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Slash : PokemonAttack
	{
        private bool mirrored;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 7;
        }

		public override void SetDefaults()
        {
            Projectile.width = 70;
            Projectile.height = 70;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 60;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;

            Projectile.hide = false;

            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Slash>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 0f, pokemon.owner)];
						pokemon.velocity = 30*Vector2.Normalize(targetCenter-pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
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
            Slash proj = (Slash)pokemonOwner.attackProjs[i].ModProjectile;

            if (pokemonOwner.attackProjs[i].ai[0] == 0)
            {
                pokemonOwner.attackProjs[i].Center = pokemon.Center + pokemon.velocity * 5f;
                if (pokemon.velocity.Length() > float.Epsilon)
                {
                    proj.mirrored = pokemon.velocity.X < 0f;
                    pokemonOwner.attackProjs[i].rotation = pokemon.velocity.ToRotation();
                }
                if (pokemon.velocity.Length() < 1f)
                {
                    pokemonOwner.attackProjs[i].Kill();
                    if (!pokemonOwner.canAttack)
                    {
                        pokemonOwner.timer = 0;
                    }
                }
            }
        }

        public override void UpdateNoAttackProjs(Projectile pokemon, int i)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            Slash proj = (Slash)pokemonOwner.attackProjs[i].ModProjectile;

            if (pokemonOwner.attackProjs[i].ai[0] == 0)
            {
                pokemonOwner.attackProjs[i].Center = pokemon.Center + pokemon.velocity * 5f;
                if (pokemon.velocity.Length() > float.Epsilon)
                {
                    proj.mirrored = pokemon.velocity.X < 0f;
                    pokemonOwner.attackProjs[i].rotation = pokemon.velocity.ToRotation();
                }
                if (pokemon.velocity.Length() < 1f)
                {
                    pokemonOwner.attackProjs[i].Kill();
                    if (!pokemonOwner.canAttack)
                    {
                        pokemonOwner.timer = 0;
                    }
                }
            }
        }

        public override void ExtraChanges(Projectile pokemon)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

            if (!pokemonOwner.canAttack && pokemonOwner.timer > 0)
            {
                pokemonOwner.immune = true;
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
                new Color(lightColor.ToVector3() + Color.White.ToVector3() * 0.3f), 
                Projectile.rotation, 
                texture.Frame(1, Main.projFrames[Projectile.type]).Size() / 2f, 
                Projectile.scale, 
                mirrored? SpriteEffects.FlipVertically: SpriteEffects.None, 0
                );

            return false;
        }

        public override void AI()
        {
            if (Projectile.ai[0] == 0 && Projectile.timeLeft < 10)
            {
                Projectile.Opacity += 0.1f;
            }

            UpdateAnimation();

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            StartCut();
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            StartCut();
            base.OnHitPlayer(target, info);
        }

        public void StartCut()
        {
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;
                Projectile.velocity = Vector2.Zero;

                Projectile.hide = false;
                Projectile.frameCounter = 0;
                Projectile.frame = 0;

                Projectile.penetrate = 2;
                Projectile.timeLeft = 21;

                Projectile.Opacity = 1f;
                SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
            }
        }

        private void UpdateAnimation()
        {
            if (Projectile.ai[0] == 1f)
            {
                if (++Projectile.frameCounter >= 3)
                {
                    Projectile.frameCounter = 0;
                    if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    {
                        Projectile.Kill();
                    }
                    if (Projectile.frame == 1)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Dust.NewDust(Projectile.Center, 4, 4, DustID.YellowTorch, Main.rand.Next(2, 15) * (mirrored? -1: 1), Main.rand.Next(-7, -3));
                        }
                    }
                }
            }
        }
    }
}
