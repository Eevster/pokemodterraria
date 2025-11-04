using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class NightShade : PokemonAttack
	{
		private Vector2 targetPosition;

		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(targetPosition);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            targetPosition = reader.ReadVector2();
            base.ReceiveExtraAI(reader);
        }

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.ignoreWater= true;
            Projectile.tileCollide= false;
            Projectile.timeLeft = 50;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
			Projectile.light = 0.7f;
			base.SetDefaults();
        }

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){

                        SoundEngine.PlaySound(SoundID.Item103 with { Volume = 0.6f }, pokemon.position);

                        pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), targetCenter, Vector2.Zero, ModContent.ProjectileType<NightShade>(), 1, 2f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
                        break;
					}
				} 
			}
		}

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
            base.ModifyHitNPC(target, ref modifiers);
			PokemonPetProjectile pokemonOwner = (PokemonPetProjectile)pokemonProj.ModProjectile;
            //modifiers.FinalDamage *= 1;
            modifiers.FinalDamage += (int)(pokemonOwner.pokemonLvl * 10f) - 1;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitPlayer(target, ref modifiers);
            PokemonPetProjectile pokemonOwner = (PokemonPetProjectile)pokemonProj.ModProjectile;
            //modifiers.FinalDamage *= 1;
            modifiers.FinalDamage += (int)(pokemonOwner.pokemonLvl * 4f) - 1;
        }


        public override void OnSpawn(IEntitySource source)
		{
			if (attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack)
			{
				SearchTarget(64f);
			}
			else if (attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack)
			{
				if (Trainer.targetPlayer != null)
				{
					targetPlayer = Trainer.targetPlayer;
				}
				else if (Trainer.targetNPC != null)
				{
					targetEnemy = Trainer.targetNPC;
				}
			}

			if (!Main.dedServ)
			{
				for (int j = 0; j < 10; j++)
				{

					Dust newDust = Main.dust[Dust.NewDust(Projectile.Center, 64, 64, DustID.Shadowflame)];
					newDust.noGravity = true;

				}
			}

			base.OnSpawn(source);
		}

        public override void AI()
        {
			Lighting.AddLight(Projectile.Center, 0.4f, 0.1f, 0.4f);

			if(attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				if(Trainer.targetPlayer != null){
					targetPlayer = Trainer.targetPlayer;
				}else if(Trainer.targetNPC != null){
					targetEnemy = Trainer.targetNPC;
				}
			}

			Projectile.Opacity = Projectile.timeLeft*0.025f;

			if (targetEnemy != null || targetPlayer != null)
			{
				if (targetEnemy != null)
				{
					if (targetEnemy.active)
					{
						targetPosition = targetEnemy.Center;
					}
					else
					{
						targetEnemy = null;
					}
				}
				if (targetPlayer != null)
				{
					if (targetPlayer.active && !targetPlayer.dead)
					{
						targetPosition = targetPlayer.Center;
					}
					else
					{
						targetPlayer = null;
					}
				}
				Projectile.Center = targetPosition;
			}

			if(Projectile.owner == Main.myPlayer){
				Projectile.netUpdate = true;
			}
        }

		public override bool? CanHitNPC(NPC target)
        {
			if(targetEnemy != null){
				if(targetEnemy.active){
					return target.whoAmI == targetEnemy.whoAmI;
				}
			}
            return false;
        }

        public override bool CanHitPvp(Player target)
        {
			if(targetPlayer != null){
				if(targetPlayer.active){
					return target.whoAmI == targetPlayer.whoAmI;
				}
			}
            return false;
        }

        public override bool ShouldUpdatePosition() {
			// Update Projectile.Center manually
			return false;
		}
    }
}
