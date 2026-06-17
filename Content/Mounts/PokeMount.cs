using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;

namespace Pokemod.Content.Mounts
{
	/*public class PokeMount : ModMount
	{
		public override void SetStaticDefaults()
		{
			// Movement
			MountData.jumpHeight = 5; // How high the mount can jump.
			MountData.acceleration = 0.19f; // The rate at which the mount speeds up.
			MountData.jumpSpeed = 4f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is pressed.
			MountData.blockExtraJumps = false; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
			MountData.constantJump = true; // Allows you to hold the jump button down.
			MountData.heightBoost = 0; // Height between the mount and the ground
			MountData.fallDamage = 0.8f; // Fall damage multiplier.
			MountData.runSpeed = 9f; // The speed of the mount
			MountData.dashSpeed = 7f; // The speed the mount moves when in the state of dashing.
			MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.

			// Misc
			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<BikeBuff>(); // The ID number of the buff assigned to the mount.

			// Effects
			MountData.spawnDust = ModContent.DustType<Dusts.Sparkle>(); // The ID of the dust spawned when mounted or dismounted.

			// Frame data and player offsets
			MountData.totalFrames = 6; // Amount of animation frames for the mount
			MountData.playerYOffsets = [18, 22, 20, 20, 18, 18]; // Fills an array with values for less repeating code ( Enumerable.Repeat(20, MountData.totalFrames).ToArray() )
			MountData.xOffset = 4;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = 0;
			MountData.bodyFrame = 3;
			// Standing
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			// Running
			MountData.runningFrameCount = 4;
			MountData.runningFrameDelay = 24;
			MountData.runningFrameStart = 2;
			// Flying
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			// In-air
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 12;
			MountData.inAirFrameStart = 1;
			// Idle
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 12;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			// Swim
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			if (!Main.dedServ)
			{
				skipDust = true;
			}
		}

        public override void Dismount(Player player, ref bool skipDust)
        {
            if (!Main.dedServ)
			{
				skipDust = true;
			}
        }

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow) {
			int actualFrame = drawPlayer.mount._frame;

			actualFrame = (drawPlayer.mount._frame+2)%4 + 18;
			frame = texture.Frame(1, 22, 0, actualFrame);

			if (drawType == 1) {
				Texture2D skinTexture = Mod.Assets.Request<Texture2D>($"Mounts/Transformations/{this.Name}_Skin").Value;
				drawColor = Lighting.GetColor((int)(drawPlayer.Center.X/ 16), (int)(drawPlayer.Center.Y/ 16), drawPlayer.skinColor);
				playerDrawData.Add(new DrawData(skinTexture, drawPosition, skinTexture.Frame(1,MountData.totalFrames,0,actualFrame), drawColor, rotation, skinTexture.Frame(1,MountData.totalFrames).Size() * 0.5f, drawScale, spriteEffects, 0));
			}
			
			return true;
		}
	}*/
}