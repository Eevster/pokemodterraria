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
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class DoubleEdge : PokemonAttack
	{
		public override bool CanExistIfNotActualMove => false;
		public bool firstImpact = true;
		private Color dustColor = new Color(255, 200, 150);

		public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(firstImpact);
			base.SendExtraAI(writer);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			firstImpact = reader.ReadBoolean();
			base.ReceiveExtraAI(reader);
		}

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 50;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.timeLeft = 60;

			Projectile.tileCollide = false;
			Projectile.penetrate = 3;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;

			Projectile.hide = true;
			base.SetDefaults();
		}

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if (pokemon.owner == Main.myPlayer)
			{
				for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
				{
					if (pokemonOwner.attackProjs[i] == null)
					{
						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<DoubleEdge>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name) * 3, 16f, pokemon.owner)];
						pokemon.velocity = 30 * Vector2.Normalize(targetCenter - pokemon.Center);
						SoundEngine.PlaySound(SoundID.Item46, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						for (int j = 0; j < 10; j++)
						{
							Dust.NewDust(pokemon.Bottom, 2, 2, DustID.Smoke, -pokemon.velocity.X / 10, -pokemon.velocity.Y / 10);
						}
						break;
					}
				}
			}
		}

		public override void UpdateAttackProjs(Projectile pokemon, int i, ref float maxFallSpeed)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			pokemonOwner.attackProjs[i].velocity = pokemon.velocity;
			if (pokemon.velocity.Length() < 1f)
			{
				pokemonOwner.attackProjs[i].Kill();
				if (!pokemonOwner.canAttack)
				{
					pokemonOwner.timer = 0;
				}
			}
		}

		public override void UpdateNoAttackProjs(Projectile pokemon, int i)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			pokemonOwner.attackProjs[i].Center = pokemon.Center;
			pokemonOwner.attackProjs[i].velocity = pokemon.velocity;
			if (pokemon.velocity.Length() < 5f)
			{
				pokemonOwner.attackProjs[i].Kill();
				if (!pokemonOwner.canAttack)
				{
					pokemonOwner.timer = 0;
				}
			}
		}

		public override void ExtraChanges(Projectile pokemon)
		{
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if (!pokemonOwner.canAttack && pokemonOwner.timer > 0)
			{
				pokemon.velocity.Y *= 0.95f;
			}
		}

		public override void AI()
		{
			if (Main.rand.Next(0, 7) + Projectile.velocity.Length() / 9 >= 6)
			{
				int dust1 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Pixie, Projectile.velocity.X / 4, Projectile.velocity.Y / 4, 0, dustColor, 1.5f);
				int dust2 = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Pixie, Projectile.velocity.X / 8, Projectile.velocity.Y / 8, 0, dustColor, 1f);
				Main.dust[dust1].noGravity = true;
				Main.dust[dust2].noGravity = true;
			}

			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.netUpdate = true;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Impact(damageDone, target.Center);
			base.OnHitNPC(target, hit, damageDone);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Impact(info.Damage, target.Center);
			base.OnHitPlayer(target, info);
		}

		public void Impact(int damage, Vector2 targetPosition)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust = Dust.NewDust(Projectile.Center + Projectile.velocity, 2, 2, DustID.Pixie, -0.4f * Projectile.velocity.X, -0.4f * Projectile.velocity.Y, 0, dustColor, 1f);
				Main.dust[dust].noGravity = true;
			}

			if (firstImpact)
			{
				pokemonProj.velocity *= 0.5f;
				SoundEngine.PlaySound(SoundID.Item72, Projectile.Center);
				firstImpact = false;
			}
		}
	}
}
