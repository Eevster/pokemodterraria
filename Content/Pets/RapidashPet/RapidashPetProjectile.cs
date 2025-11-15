using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RapidashPet
{
	public class RapidashPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];
	}

	public class RapidashPetProjectileShiny : RapidashPetProjectile{
        public override void ChangeAttackColor(PokemonAttack attack, bool condition = false, int shaderID = 0, Color color = default)
        {
            condition = attack.attackType == (int)TypeIndex.Fire;
            color = new Color(100, 95, 105);
            base.ChangeAttackColor(attack, condition, shaderID, color);
        }
    }
}
