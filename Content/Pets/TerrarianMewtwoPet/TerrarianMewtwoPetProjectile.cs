using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TerrarianMewtwoPet
{
	public class TerrarianMewtwoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 62;
		public override int hitboxHeight => 66;

		public override int totalFrames => 46;
		public override int animationSpeed => 5;

        public override int moveStyle => 2;
        public override int[] idleStartEnd => [0, 10];
        public override int[] walkStartEnd => [22, 27];
        public override int[] jumpStartEnd => [22, 27];
        public override int[] fallStartEnd => [22, 27];
        public override int[] attackStartEnd => [11, 21];

        public override int[] idleFlyStartEnd => [22, 27];
        public override int[] walkFlyStartEnd => [29, 34];
        public override int[] attackFlyStartEnd => [34, 45];

        public override void ChangeAttackColor(PokemonAttack attack, bool condition = false, int shaderID = 0, Color color = default)
        {
            condition = attack.attackType == (int)TypeIndex.Fire || attack.attackType == (int)TypeIndex.Psychic;
            shaderID = ItemID.WispDye;
            color = new Color(80, 250, 20);
            base.ChangeAttackColor(attack, condition, shaderID, color);
        }
    }

    public class TerrarianMewtwoPetProjectileShiny : TerrarianMewtwoPetProjectile {}
}
