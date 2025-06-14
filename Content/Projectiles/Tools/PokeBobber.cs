using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.Tools
{
	public class PokeBobber : ModProjectile
	{
		public static readonly Color[] PossibleLineColors = new Color[] {
			new Color(215, 215, 215),
		};

		// This holds the index of the fishing line color in the PossibleLineColors array.
		private int fishingLineColorIndex;

		public Color FishingLineColor => PossibleLineColors[fishingLineColorIndex];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.BobberWooden);

			DrawOriginOffsetY = -8;
		}

		public override void OnSpawn(IEntitySource source) {
			fishingLineColorIndex = (byte)Main.rand.Next(PossibleLineColors.Length);
		}

		public override void AI() {
			if (!Main.dedServ) {
				Lighting.AddLight(Projectile.Center, FishingLineColor.ToVector3());
			}
		}

		// These last two methods are required so the line color is properly synced in multiplayer.
		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write((byte)fishingLineColorIndex);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			fishingLineColorIndex = reader.ReadByte();
		}
	}
}