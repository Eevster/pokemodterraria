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
	public class Blizzard : PokemonAttack
	{
		public override bool CanExistIfNotActualMove => false;
		int explosionSize = 400;

		public override void SetDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 300;

			Projectile.Opacity = 0.3f;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = false;  
            Projectile.penetrate = -1;

			Projectile.tileCollide = false;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if (pokemon.owner == Main.myPlayer)
			{
				for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
				{
					if (pokemonOwner.attackProjs[i] == null)
					{
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Blizzard>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
						SoundEngine.PlaySound(SoundID.Item122, pokemon.position);
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
			pokemonOwner.attackProjs[i].scale = 2f;
            pokemonOwner.attackProjs[i].Center = pokemon.Center;
		}

        public void DustStorm()
		{
            if (Main.rand.NextBool(20)) SoundEngine.PlaySound(SoundID.DD2_BetsyWindAttack with { Pitch = 1f }, Projectile.Center);
            if (Main.rand.NextBool(10)) SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);

            for (int i = 0; i < 20; i++) {
				float angle = Main.rand.NextFloat(MathHelper.TwoPi);
				float distance = Main.rand.NextFloat(explosionSize * 0.125f, explosionSize * 0.5f);
				Vector2 offset = Vector2.One.RotatedBy(angle) * distance;
				Vector2 velocity = offset.RotatedBy(MathHelper.PiOver2) * 0.05f;

				int dust = Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.Ice, velocity.X, velocity.Y);
				Main.dust[dust].noGravity = true;
				if (Main.rand.NextBool(3))
				{
					dust = Dust.NewDust(Projectile.Center + offset, 0, 0, DustID.FireworksRGB, velocity.X, velocity.Y, Alpha: 200, newColor: new Color(20, 255, 255));
					Main.dust[dust].noGravity = true;
					Main.dust[dust].noLight = true;
				}
            }
		}

        public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(18f);

			DustStorm();

            if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.Slow, 6*60);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			target.AddBuff(BuffID.Slow, 6*60);
            base.OnHitPlayer(target, info);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // "Hit anything between the player and the tip of the sword"
            // shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
            Vector2 start = Projectile.Center + new Vector2(explosionSize * 0.85f, 0);
            Vector2 end = Projectile.Center - new Vector2(explosionSize * 0.85f, 0);
            float collisionPoint = 0f; // Don't need that variable, but required as parameter

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, explosionSize *0.5f, ref collisionPoint);
        }
    }
}
