using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Pokemod.Common.Configs
{
	public class BetaMonsConfig : ModConfig
	{
		
		public override ConfigScope Mode => ConfigScope.ServerSide;

		

		[Header("Items")] 
		
		[DefaultValue(false)] 
		[ReloadRequired]
		public bool BetaMonsToggle;
	}
}
