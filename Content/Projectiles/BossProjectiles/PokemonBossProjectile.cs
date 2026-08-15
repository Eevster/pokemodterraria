using Microsoft.Xna.Framework;
using Pokemod.Content.DamageClasses;
using Pokemod.Content.NPCs;
using Pokemod.Content.NPCs.Bosses;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.BossProjectiles
{
    public abstract class PokemonBossProjectile : ModProjectile
    {
        public virtual int attackType => (int)TypeIndex.Normal;
        public virtual bool isSpecial => true;

        public PokemonBossBody boss;

        public Player targetPlayer;
        public Projectile targetPokemon;

        public Vector2 targetPosition;

        public bool foundTarget = false;

        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<PokemonDamageClass>();
        }

        public void TrackOwner()
        {
            if (boss == null || boss.NPC.active == false) return;

            Projectile.Center = boss.NPC.Center;
            Projectile.velocity = Vector2.Zero;
        }
    }
}
