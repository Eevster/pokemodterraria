using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets.PikachuPet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PikachuPet
{
    public class PikachuPetProjectileShiny : PokemonPetProjectile
    {
       
        public override int baseHP => 35;
        public override int baseDef => 40;
        public override int nAttackProjs => 1;
        public override int baseDamage => 3;
        public override int PokemonBuff => ModContent.BuffType<PikachuPetBuffShiny>();
        public override float enemySearchDistance => 1000;
        public override bool canAttackThroughWalls => false;
        public override int attackDuration => 30;
        public override int attackCooldown => 60;
        public override bool canMoveWhileAttack => true;

        public override int totalFrames => 19;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 8];
        public override int[] walkStartEnd => [9, 17];
        public override int[] jumpStartEnd => [11, 11];
        public override int[] fallStartEnd => [15, 15];
        public override int[] attackStartEnd => [18, 18];

        public override string[] evolutions => ["Raichu"];
        public override string[] itemToEvolve => ["ThunderStoneItem"];

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

            //current HP set to beyond max so pokemon won't instantly despawn
            currentHp = 1000;
            //ends here

            //Projectile.width = 48;
            Projectile.width = 16;
            DrawOffsetX = -(24 - Projectile.width / 2);
            Projectile.height = 36;
            Projectile.aiStyle = -1; // Use custom AI
            Projectile.light = 0.3f;
            Projectile.tileCollide = true;
        }

        public override void Attack(float distanceFromTarget, Vector2 targetCenter)
        {
            currentStatus = (int)ProjStatus.Attack;
            SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
            if (Projectile.owner == Main.myPlayer)
            {
                attackProjs[0] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ThunderboltHold>(), GetPokemonDamage(), 2f, Projectile.owner)];
            }
            timer = attackDuration;
            canAttack = false;
        }

        public override void UpdateAttackProjs(int i, ref float maxFallSpeed)
        {
            Projectile.velocity.X *= 0.9f;
            maxFallSpeed = 2f;
            attackProjs[0].Center = Projectile.Center;
        }

        public override void UpdateNoAttackProjs(int i)
        {
            attackProjs[0].Kill();
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 26;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        
    }

}
