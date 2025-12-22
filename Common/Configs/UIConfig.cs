using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Pokemod.Common.Configs
{
	public class UIConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[Header("PokemonCursor")] 
		
		[Range(100f, 500f)]
		[Increment(50f)]
		[DrawTicks]
		[DefaultValue(250f)]
		public float ArrowDistance;

		[Range(0.2f, 2f)]
		[Increment(0.2f)]
		[DrawTicks]
		[DefaultValue(1f)]
		public float ArrowScale;

		[Range(16f, 64f)]
		[Increment(8f)]
		[DrawTicks]
		[DefaultValue(32f)]
		public float PokemonImageDistance;
	}
}
