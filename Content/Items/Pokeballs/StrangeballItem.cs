using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Pokeballs
{
	public class StrangeballItem : BallItem
	{
		protected override int BallProj => ModContent.ProjectileType<StrangeballProj>();
		protected override int BallValue => 1000000;
		protected override float CatchRate => 1f;

        
    }

	public class StrangeballProj : BallProj{}
}
