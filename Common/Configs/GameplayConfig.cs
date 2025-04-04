using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Pokemod.Common.Configs
{
	public class GameplayConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("PokemonWorld")] 
		
		[Range(0f, 5f)]
		[Increment(.5f)]
		[DrawTicks]
		[DefaultValue(1f)]
		public float PokemonSpawnMultiplier;

		[Header("RuleChanges")] 

		[DefaultValue(false)]
		public bool RandomizedStarters;
		[DefaultValue(false)]
		public bool RandomizedEvolutions;
		[DefaultValue(false)]
		public bool ReleaseFaintedPokemon;
	}
}
