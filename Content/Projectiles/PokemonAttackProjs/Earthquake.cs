using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    internal class Earthquake : PokemonAttack
    {
        const int explosionSize = 500;
        public override string Texture => "Pokemod/Content/Projectiles/PokemonAttackProjs/MagicalLeaf";
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hide = true;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 10;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;  

            Projectile.penetrate = -1;

            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						pokemonOwner.canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 5){
					int remainProjs = 1;
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Bottom, Vector2.Zero, ModContent.ProjectileType<Earthquake>(), 0, 2f, pokemon.owner, 0f, pokemonOwner.GetPokemonAttackDamage(GetType().Name))];
                            remainProjs--;
							pokemonOwner.canAttackOutTimer = false;
							if(remainProjs <= 0){
								break;
							}
						}
					} 
				}
            }
		}

        public override void OnSpawn(IEntitySource source)
        {
            if (Projectile.ai[0] == 1) //Sets child height to match the height of it's target so dust appears from the ground.
            {
                Projectile.height = (int)Projectile.ai[2];
                Projectile.timeLeft = 1;
            }
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.Bottom, 10, 6, DustID.Dirt, 0, -3);
            }

            if (Projectile.ai[0] == 0) //Parent creates sound and screen effects.
            {
                SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);
                //Screen Shake
                PunchCameraModifier modifier = new PunchCameraModifier(Projectile.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 7f, 5f, 25, explosionSize * 2f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }

            base.OnSpawn(source);
        }

        public override void AI()
        {
            //ai[0]: 0 = parent, 1 = child. Ensures only the parent can make child projectiles.
            if (Projectile.ai[0] == 1f)
            {
                return;
            }

            List<NPC> targets = new List<NPC>();

            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.CanBeChasedBy() && npc.life > 0)
                    {
                        if ((npc.Center - Projectile.Center).Length() < explosionSize)
                        {
                            if (Collision.SolidCollision(npc.Bottom, npc.width, 6))
                            {
                                targets.Add(npc);
                            }
                        }
                    }
                }
                if (targets.Count > 0)
                {
                    int earthquakeDamage = (int)Projectile.ai[1];

                    foreach (NPC npc in targets)
                    {
                        Projectile.NewProjectile(Projectile.InheritSource(Projectile), npc.Bottom, Vector2.Zero, ModContent.ProjectileType<Earthquake>(), earthquakeDamage, 2f, Projectile.owner, 1f, default, npc.height);
                    }
                }
                Projectile.netUpdate = true;
            }
        }
    }
}