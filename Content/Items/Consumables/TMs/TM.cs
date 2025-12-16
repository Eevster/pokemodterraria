
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.UI.MoveLearnUI;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using rail;
using ReLogic.Content;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Consumables.TMs
{
	public abstract class TechnicalMachine : PokemonConsumableItem
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMNormal";

		public bool singleMove = false;
        public TypeIndex moveType = TypeIndex.Normal;
		public string[] moves = [];
        public Asset<Texture2D> tmTexture;

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

            if (Name[..2] != "TM") singleMove = true;
			if (singleMove)
			{
				SetMove();
			}
		}
		public void SetMove()
		{
			string moveName = Name.Replace("TM", "");
			if (PokemonData.pokemonAttacks.TryGetValue(moveName, out PokemonAttackInfo value))
			{
				moves = [moveName];
                moveType = (TypeIndex)value.attackType;
            }
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			if (singleMove)
			{
				string tooltip =
                    Language.GetTextValue("Mods.Pokemod.PokemonInfo.ItemActiveUse") + "\n" +
					Language.GetTextValue("Mods.Pokemod.PokemonInfo.TMContains") + Name.Replace("TM", "") + "\n" +
					"[" + "[c/" + PokemonNPCData.GetTypeColor((int)moveType) + ":" + Language.GetTextValue("Mods.Pokemod.PokemonTypes." + moveType) + "]" + "]";
				tooltips.Add(new(Mod, Name + "Tooltip", tooltip));
			}
        }


        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
		{
			return UseTM(item, player);
		}

		public override bool OnItemUse(Projectile proj)
		{
			bool used = false;
			Player player = Main.player[Main.myPlayer];
			if (player != null)
			{
				CaughtPokemonItem item = null;

				foreach (Item invItem in player.inventory)
				{
					if (invItem.ModItem is CaughtPokemonItem)
					{
						CaughtPokemonItem invPokemon = (CaughtPokemonItem)invItem?.ModItem;
						if (invPokemon.proj != null)
						{
							if (invPokemon.proj == proj)
							{
								item = invPokemon;
								break;
							}
						}
					}
				}
				if (item != null) used = UseTM(item, player);
			}
			Item.consumable = used;
			return used;
		}

		public bool UseTM(CaughtPokemonItem item, Player player)
		{
			if (!MoveLearnUIState.hidden) return false;

			if (PokemonData.pokemonInfo[item.PokemonName].HasType(moveType) || moveType == TypeIndex.Normal)
			{
				List<string> newMoves = moves.ToList();
				foreach (string move in item.moves)
				{
					newMoves.Remove(move);
				}

				if (newMoves.Count > 0)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Item.consumable = true;
					item.LearnMove(newMoves[Main.rand.Next(newMoves.Count)], Item.type);
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

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            if (singleMove)
            {
                tmTexture = ModContent.Request<Texture2D>("Pokemod/Content/Items/Consumables/TMs/TM" + moveType.ToString());

                spriteBatch.Draw(tmTexture.Value,
                    position: position - scale * new Vector2(tmTexture.Value.Width / 2, tmTexture.Value.Height / 2),
                    sourceRectangle: tmTexture.Value.Bounds,
                    drawColor,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: scale,
                    SpriteEffects.None,
                    layerDepth: 0f);
                return false;
            }
            return true;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (singleMove)
            {
                tmTexture = ModContent.Request<Texture2D>("Pokemod/Content/Items/Consumables/TMs/TM" + moveType.ToString());

                spriteBatch.Draw(tmTexture.Value,
                    position: Item.position - Main.screenPosition,
                    sourceRectangle: tmTexture.Value.Bounds,
                    lightColor,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: scale,
                    SpriteEffects.None,
                    layerDepth: 0f);
				return false;
			}
            return true;
        }
    }

    public class AirSlashTM : TechnicalMachine { }
    public class BlizzardTM : TechnicalMachine { }
    public class BrickBreakTM : TechnicalMachine { }
    public class BulletSeedTM : TechnicalMachine { }
    public class CrunchTM : TechnicalMachine { }
    public class DigTM : TechnicalMachine { }
    public class DragonBreathTM : TechnicalMachine { }
    public class DragonRushTM : TechnicalMachine { }
    public class EarthquakeTM : TechnicalMachine { }
    public class ExplosionTM : TechnicalMachine { }
    public class FireBlastTM : TechnicalMachine { }
    public class FlamethrowerTM : TechnicalMachine { }
    public class FlashCannonTM : TechnicalMachine { }
    public class FocusPunchTM : TechnicalMachine { }
    public class FuryCutterTM : TechnicalMachine { }
    public class GigaDrainTM : TechnicalMachine { }
    public class HexTM : TechnicalMachine { }
    public class HydroPumpTM : TechnicalMachine { }
    public class HyperBeamTM : TechnicalMachine { }
    public class IceBeamTM : TechnicalMachine { }
    public class NightSlashTM : TechnicalMachine { }
    public class OverheatTM : TechnicalMachine { }
    public class PinMissileTM : TechnicalMachine { }
    public class PsychicTM : TechnicalMachine { }
    public class PsychoCutTM : TechnicalMachine { }
    public class RockSlideTM : TechnicalMachine { }
    public class ShadowBallTM: TechnicalMachine { }
    public class SludgeBombTM: TechnicalMachine { }
    public class SolarBeamTM : TechnicalMachine { }
    public class StoneEdgeTM: TechnicalMachine { }
    public class SwiftTM : TechnicalMachine { }
    public class ThunderTM : TechnicalMachine { }
    public class ThunderboltTM : TechnicalMachine { }
    public class ToxicTM : TechnicalMachine { }
    public class WaterPulseTM: TechnicalMachine { }
    public class WingAttackTM: TechnicalMachine { }



    public abstract class TMBug : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMBug";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Bug;
			moves = [];
		}
	}

	public class TMDark : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMDark";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Dark;
			moves = ["Crunch", "NightSlash"];
		}
	}

	public class TMDragon : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMDragon";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Dragon;
			moves = ["DragonBreath", "DragonRush"];
		}
	}

	public class TMElectric : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMElectric";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Electric;
			moves = ["ElectroBall", "ThunderWave", "Thunderbolt", "Thunder"];
		}
	}

	public abstract class TMFairy : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMFairy";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Fairy;
			moves = [];
		}
	}

	public class TMFighting : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMFighting";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Fighting;
			moves = ["FocusPunch", "BrickBreak"];
		}
	}

	public class TMFire : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMFire";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Fire;
			moves = ["Flamethrower", "FireBlast", "Overheat"];
		}
	}

	public class TMFlying : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMFlying";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Flying;
			moves = ["AirSlash"];
		}
	}

	public class TMGhost : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMGhost";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Ghost;
			moves = ["ConfuseRay", "Hex", "NightShade", "ShadowBall"];
		}
	}

	public class TMGrass : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMGrass";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Grass;
			moves = ["MagicalLeaf", "BulletSeed", "GigaDrain", "LeafStorm", "SolarBeam"];
		}
	}

	public class TMGround : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMGround";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Ground;
			moves = ["Dig", "Earthquake", "MudShot"];
		}
	}

	public class TMIce : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMIce";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Ice;
			moves = ["IceFang", "IceBeam", "Blizzard"];
		}
	}

	public class TMNormal : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMNormal";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Normal;
			moves = ["Swift", "HyperBeam", "DoubleEdge", "Slash"];
		}
	}

	public class TMPoison : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMPoison";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Poison;
			moves = ["Toxic", "SludgeBomb"];
		}
	}

	public class TMPsychic : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMPsychic";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Psychic;
			moves = ["Psybeam", "Psychic"];
		}
	}

	public class TMRock : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMRock";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Rock;
			moves = ["RockSlide", "StoneEdge"];
		}
	}
	public class TMSteel : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMSteel";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Steel;
			moves = ["FlashCannon"];
		}
	}

	public class TMWater : TechnicalMachine
	{
        public override string Texture => "Pokemod/Content/Items/Consumables/TMs/TMWater";
        public override void SetDefaults()
		{
			base.SetDefaults();

			moveType = TypeIndex.Water;
			moves = ["WaterPulse", "Waterfall", "HydroPump"];
		}
	}
}
