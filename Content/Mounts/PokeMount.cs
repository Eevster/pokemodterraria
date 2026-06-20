using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;
using Pokemod.Content.Buffs.MountBuffs;
using Pokemod.Common.Players;
using Pokemod.Content.Pets;

namespace Pokemod.Content.Mounts
{
	public class PokeMount : ModMount
	{
		public PokemonPetProjectile pokemon;
		public override void SetStaticDefaults()
		{
			// Movement
			MountData.jumpHeight = 0; // How high the mount can jump.
			MountData.acceleration = 0.0f; // The rate at which the mount speeds up.
			MountData.jumpSpeed = 0f; // The rate at which the player and mount ascend towards (negative y velocity) the jump height when the jump button is pressed.
			MountData.blockExtraJumps = false; // Determines whether or not you can use a double jump (like cloud in a bottle) while in the mount.
			MountData.constantJump = false; // Allows you to hold the jump button down.
			MountData.heightBoost = 0; // Height between the mount and the ground
			MountData.fallDamage = 0f; // Fall damage multiplier.
			MountData.runSpeed = 0f; // The speed of the mount
			MountData.dashSpeed = 0f; // The speed the mount moves when in the state of dashing.
			MountData.flightTimeMax = 0; // The amount of time in frames a mount can be in the state of flying.

			// Misc
			MountData.fatigueMax = 0;
			MountData.buff = ModContent.BuffType<PokeMountBuff>(); // The ID number of the buff assigned to the mount.

			MountData.spawnDust = ModContent.DustType<Dusts.Sparkle>(); // The ID of the dust spawned when mounted or dismounted

			// Frame data and player offsets
			MountData.totalFrames = 1; // Amount of animation frames for the mount
			MountData.playerYOffsets = [0];
			MountData.xOffset = 0;
			MountData.yOffset = 0;
			MountData.playerHeadOffset = 0;
			MountData.bodyFrame = 3;
			// Standing
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 8;
			MountData.standingFrameStart = 0;
			// Running
			MountData.runningFrameCount = 1;
			MountData.runningFrameDelay = 8;
			MountData.runningFrameStart = 0;
			// In-air
			MountData.inAirFrameCount = 1;
			MountData.inAirFrameDelay = 8;
			MountData.inAirFrameStart = 0;
			// Idle
			MountData.idleFrameCount = 1;
			MountData.idleFrameDelay = 8;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = true;
			// Swim
			MountData.swimFrameCount = MountData.inAirFrameCount;
			MountData.swimFrameDelay = MountData.inAirFrameDelay;
			MountData.swimFrameStart = MountData.inAirFrameStart;

			if (!Main.dedServ)
			{
				MountData.textureWidth = 0;
				MountData.textureHeight = 0;
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			if (!Main.dedServ)
			{
				skipDust = true;
			}

			pokemon = player.GetModPlayer<PokemonPlayer>().GetMountPokemon();

			if(pokemon == null || (pokemon != null && !pokemon.Projectile.active)) Dismount(player, ref skipDust);
		}

        public override void Dismount(Player player, ref bool skipDust)
        {
            if (!Main.dedServ)
			{
				skipDust = true;
			}
        }

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow) {
			if (drawType == 3 && (pokemon != null || (pokemon != null && !pokemon.Projectile.active))) {
				string TextureName = $"Assets/Textures/Pokesprites/Pets/{pokemon.pokemonName}PetProjectile";

				if(pokemon.variant != null && pokemon.variant != "") TextureName += "_" + pokemon.variant;

				Texture2D frontTexture = Mod.Assets.Request<Texture2D>(TextureName).Value;

				Vector2 positionOffset = (frontTexture.Frame(1, pokemon.totalFrames).Size() * Vector2.UnitY * 0.5f) - Vector2.UnitY * 4f;

				playerDrawData.Add(new DrawData(frontTexture, pokemon.Projectile.Bottom - pokemon.Projectile.scale * positionOffset - Main.screenPosition, frontTexture.Frame(1,pokemon.totalFrames,0,pokemon.Projectile.frame), drawColor, rotation, frontTexture.Frame(1,pokemon.totalFrames).Size() * 0.5f, drawScale, spriteEffects, 0));
			}
			
			return true;
		}
	}
}