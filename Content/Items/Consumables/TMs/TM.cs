
using Terraria;
using Terraria.ID;
using Pokemod.Content.Pets;
using Pokemod.Content.NPCs;
using System.Linq;
using Terraria.ModLoader;
using Pokemod.Common.UI.MoveLearnUI;
using System.Collections.Generic;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace Pokemod.Content.Items.Consumables.TMs
{
	public abstract class TechnicalMachine : PokemonConsumableItem
	{
		public virtual TypeIndex moveType => TypeIndex.Normal;
		public virtual string[] moves => [];

		public override void SetDefaults()
		{
			Item.width = 28; // The item texture's width
			Item.height = 28; // The item texture's height

			Item.useTime = 1;
			Item.useAnimation = 1;

			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item1;

			Item.maxStack = Item.CommonMaxStack;
			Item.value = Item.buyPrice(gold: 1);

			Item.consumable = true;
		}

		public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
		{
			if (PokemonData.pokemonInfo[item.PokemonName].HasType(moveType) || moveType == TypeIndex.Normal)
			{
				List<string> newMoves = moves.ToList();
				foreach (string move in item.moves)
				{
					newMoves.Remove(move);
				}

				if (newMoves.Count > 0)
				{
					item.LearnMove(newMoves[Main.rand.Next(newMoves.Count)]);
					ReduceStack(player, Item.type);
					return true;
				}
				else
				{
					CombatText.NewText(player.Hitbox, new Color(255, 255, 255), Language.GetTextValue("Mods.Pokemod.PokemonInfo.NoMoveAvailable"));
					return false;
				}
			}
			else
			{
				CombatText.NewText(player.Hitbox, new Color(255, 255, 255), Language.GetTextValue("Mods.Pokemod.PokemonInfo.WrongType"));
			}
			return false;
		}
	}

	public abstract class TMBug : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Bug;
		public override string[] moves => [];
	}

	public abstract class TMDark : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Dark;
		public override string[] moves => [];
	}

	public abstract class TMDragon : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Dragon;
		public override string[] moves => [];
	}

	public class TMElectric : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Electric;
		public override string[] moves => ["ElectroBall","ThunderWave","Thunderbolt","Thunder"];
	}

	public abstract class TMFairy : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Fairy;
		public override string[] moves => [];
	}

	public abstract class TMFighting : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Fighting;
		public override string[] moves => [];
	}

	public class TMFire : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Fire;
		public override string[] moves => ["Flamethrower","FireBlast","Overheat"];
	}

	public class TMFlying : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Flying;
		public override string[] moves => ["AirSlash"];
	}

	public class TMGhost : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Ghost;
		public override string[] moves => ["ConfuseRay","Hex","NightShade"];
	}

	public class TMGrass : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Grass;
		public override string[] moves => ["MagicalLeaf","BulletSeed","GigaDrain","LeafStorm","SolarBeam"];
	}

	public class TMGround : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Ground;
		public override string[] moves => ["Dig","Earthquake"];
	}

	public class TMIce : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Ice;
		public override string[] moves => ["IceFang"];
	}

	public class TMNormal : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Normal;
		public override string[] moves => ["Swift"];
	}

	public class TMPoison : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Poison;
		public override string[] moves => ["Toxic"];
	}

	public class TMPsychic : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Psychic;
		public override string[] moves => ["Psybeam","Psychic"];
	}

	public abstract class TMRock : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Rock;
		public override string[] moves => [];
	}

	public class TMSteel : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Steel;
		public override string[] moves => ["FlashCannon"];
	}

	public class TMWater : TechnicalMachine
	{
		public override TypeIndex moveType => TypeIndex.Water;
		public override string[] moves => ["WaterPulse","Waterfall","HydroPump"];
	}
}
