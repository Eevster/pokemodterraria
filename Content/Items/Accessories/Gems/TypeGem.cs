using Pokemod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.NPCs;

namespace Pokemod.Content.Items.Accessories.Gems
{
    public abstract class TypeGem : ModItem
	{
		public override string Texture => "Pokemod/Content/Items/Accessories/Gems/"+Item.ModItem.Name;
		public virtual int pokemonType => 0;
		public float damageMult = 0.1f;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Language.GetTextValue("Mods.Pokemod.PokemonTypes."+(TypeIndex)pokemonType), (int)(100f*damageMult));
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(silver: 50);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual) {
			player.GetModPlayer<PokemonPlayer>().typeMult[pokemonType] += damageMult;
		}
	}

	public class BugGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Bug;
	}
	public class DarkGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Dark;
	}
	public class DragonGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Dragon;
	}
	public class ElectricGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Electric;
	}
	public class FairyGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Fairy;
	}
	public class FightingGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Fighting;
	}
	public class FireGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Fire;
	}
	public class FlyingGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Flying;
	}
	public class GhostGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Ghost;
	}
	public class GrassGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Grass;
	}
	public class GroundGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Ground;
	}
	public class IceGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Ice;
	}
	public class NormalGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Normal;
	}
	public class PoisonGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Poison;
	}
	public class PsychicGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Psychic;
	}
	public class RockGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Rock;
	}
	public class SteelGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Steel;
	}
	public class WaterGem : TypeGem
	{
		public override int pokemonType => (int)TypeIndex.Water;
	}
}