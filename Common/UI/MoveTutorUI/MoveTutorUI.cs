using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items;
using Pokemod.Content.Items.Collectables;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI.MoveTutorUI
{
	public class MoveTutorUIState : UIState
	{
		public class MoveCost
        {
            public string moveName;
            public int CostAmount;
            public int CostItemType;
            public MoveCost(string moveName, int levelToLearn)
            {
                this.moveName = moveName;
                if (levelToLearn >= 100)
                {
                    CostAmount = 1;
                    CostItemType = ItemID.PlatinumCoin;
                    return;
                }
                int bracket = (int)Math.Floor(levelToLearn / 15f);
                switch (bracket)
                {
                    case 0: // 1 - 14 Copper
                        CostAmount = levelToLearn * 6;
                        CostItemType = ItemID.CopperCoin;
                        break;
                    case 1: // 15 - 29 Low Silver
                        CostAmount = (levelToLearn - 15) * 2 + 1;
                        CostItemType = ItemID.SilverCoin;
                        break;
                    case 2: // 30 - 44 High Silver
                        CostAmount = (int)((levelToLearn - 30) * 4.5f) + 30;
                        CostItemType = ItemID.SilverCoin;
                        break;
                    default: // 45 + Gold
                        CostAmount = (int)((levelToLearn - 45) * 1.8f) + 1;
                        CostItemType = ItemID.GoldCoin;
                        break;
                }
                if (CostAmount < 1)
                {
                    CostAmount = 1;
                }
            }
            public MoveCost(string moveName, int itemAmount, int itemType)
            {
                this.moveName = moveName;
                this.CostAmount = itemAmount;
                this.CostItemType = itemType;
            }
        }

        public Item[] pokeballItem = [null];
        private CaughtPokemonItem pokemon;
        private List<MoveCost> moveList;
        public List<MoveCost> teachingMoves;

        public UIPanel MoveTutorPanel;
        public UIPanel movesPanel;
        private UIList MoveButtonsList;
		private UIText TextPrompt;

        private UIAnimImage PokemonImage;
        private UIImage PokemonFrame;

        public void OpenPanel()
		{
            BuildTeachingList();
            moveList = [];
            moveList.Clear();
            MoveButtonsList?.Clear();
            if (pokeballItem[0] == null)
            {
                pokeballItem[0] = new();
            }


            //Main Field
            MoveTutorPanel = new UIPanel();
            MoveTutorPanel.SetPadding(0);
            UIHelpers.SetRectangle(MoveTutorPanel, left: 60f, top: 244f, width: 446, height: 426);
            MoveTutorPanel.BackgroundColor = new Color(0f, 0f, 0f, 0f);
            MoveTutorPanel.BorderColor = new Color(0f, 0f, 0f, 0f);

			//Item Panel
			{
				UIPanel itemPanel = new UIPanel();
				itemPanel.SetPadding(0);
                UIHelpers.SetRectangle(itemPanel, left: 7f, top: 14f, width: 360, height: 44);
				itemPanel.BackgroundColor = new Color(0f, 0f, 0f, 0f);
				itemPanel.BorderColor = new Color(0f, 0f, 0f, 0f);

				//Item Slot
				{
                    UIItemSlot itemSlot = new(pokeballItem, 0, ItemSlot.Context.ChestItem)
                    {
                        Left = new(0f, 0f),
                        Top = new(0f, 0f),
                        Width = new(44f, 0f),
                        Height = new(44f, 0f)
                    };
                    itemSlot.OnLeftClick += (a, b) => SetPokemon();
                    itemPanel.Append(itemSlot);
                }

				//Prompt Text
				{
                    TextPrompt = new(Language.GetText("Mods.Pokemod.MoveTutorUI.Prompt"))
                    {
                        Left = new(48f, 0f),
                        Top = new(0f, 0f),
                        Width = new(306f, 0f),
                        Height = new(44f, 0f),
                        TextOriginX = 0f,
                        TextOriginY = 0.5f,
                    };
                    itemPanel.Append(TextPrompt);
                }
                MoveTutorPanel.Append(itemPanel);
            }

			//Moves Panel
			{
                movesPanel = new UIPanel
                {
                    BackgroundColor = new(0f, 0f, 0f, 0f),
                    BorderColor = new(0f, 0f, 0f, 0f)
                };
                movesPanel.SetPadding(0);
                UIHelpers.SetRectangle(movesPanel, left: 7f, top: 64f, width: 425, height: 356);

                //Pokemon Icon
                {
                    Asset<Texture2D> iconFrameTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/PokemonIconFrame");
                    Asset<Texture2D> pokemonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/PokemonIconEmpty");

                    PokemonFrame = new UIImage(iconFrameTexture) 
                    { 
                        Color = new(0f, 0f, 0f, 0f) 
                    };
                    UIHelpers.SetRectangle(PokemonFrame, left: 6f, top: 6f, width: 104f, height: 104f);
                    PokemonImage = new(pokemonTexture, 1, 0, 0) 
                    { 
                        Color = new(0f, 0f, 0f, 0f), 
                        frameRate = 8 
                    };
                    UIHelpers.SetRectangleAlign(PokemonImage, left: 0.5f, top: 0.5f, width: 96f, height: 96f);

                    PokemonFrame.Append(PokemonImage);
                    movesPanel.Append(PokemonFrame);
                }

				//Move Button List
				{
                    MoveButtonsList = new();
                    UIHelpers.SetRectangle(MoveButtonsList, left: 118f, top: 6f, width: 306f, height: 344f);
                    UIScrollbar scrollbar = new UIScrollbar();
                    UIHelpers.SetRectangleAlign(scrollbar, left: 1f, top: 0f, width: 0f, height: 344f);
                    MoveButtonsList.Append(scrollbar);
                    MoveButtonsList.SetScrollbar(scrollbar);
                    movesPanel.Append(MoveButtonsList);
                }
                MoveTutorPanel.Append(movesPanel);
            }
            SetPokemon();
            Append(MoveTutorPanel);
        }

        public void PopulateButtonList()
        {
            if (moveList.Count <= 0 || pokemon == null) return;
            Asset<Texture2D> moveButtonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/MoveButton");
            Asset<Texture2D> moveButtonActiveTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/MoveButtonActive");
            for (int i = 0; i < moveList.Count; i++)
            {
                string moveName = moveList[i].moveName;
                bool newMove = true;
                foreach (string move in pokemon.moves)
                {
                    if (move == moveName)
                    {
                        newMove = false;
                    }
                }

                //Move Button
                {
                    Color inactiveColor = new(114, 89, 92, 180);
                    UIHoverImageButton moveButton = new(moveButtonTexture, moveButtonActiveTexture, Color.White, PokemonData.SetMoveTooltip(pokemon, moveName));
                    moveButton.tooltipSolid = true;
                    moveButton.canBeSelected = true;
                    UIHelpers.SetRectangle(moveButton, left: 0f, top: 0f, width: 292f, height: 42f);
                    int moveIndex = newMove? i : -1;
                    if (!newMove) moveButton.color = inactiveColor;
                    moveButton.OnLeftClick += (a, b) => SelectMove(moveIndex);

                    //Move Name Text
                    var buttonText = new UIText(Language.GetText("Mods.Pokemod.Projectiles." + moveName + ".DisplayName"), 1f)
                    {
                        TextColor = newMove? Color.White : inactiveColor,
                        HAlign = 0.5f,
                        VAlign = 0.5f,
                        Width = new(292f, 0f),
                        Height = new(21f, 0f),
                    };
                    moveButton.Append(buttonText);

                    //Cost Item Icon
                    {
                        int costItemType = moveList[i].CostItemType;
                        UIItemIcon costIcon = new(new Item(costItemType), false);
                        UIHelpers.SetRectangle(costIcon, left: 4f, top: 6f, width: 30f, height: 30f);
                        moveButton.Append(costIcon);

                        //Amount
                        {
                            int costAmount = moveList[i].CostAmount;
                            UIText costText = new(costAmount.ToString(), 0.8f);
                            UIHelpers.SetRectangleAlign(costText, left: 0f, top: 1f, width: 15f, height: 15f);
                            costIcon.Append(costText);
                        }
                    }
                    MoveButtonsList.Add(moveButton);
                }
            }
        }

        private void BuildTeachingList()
        {
            teachingMoves = [];
            //Template for when tutor moves are added:
            //teachingMoves.Add(new("SludgeBomb", 3, ModContent.ItemType<HeartScale>()));
        }

        public void SetPokemon()
        {
            moveList.Clear();
            MoveButtonsList?.Clear();

            if (pokeballItem[0]?.ModItem is CaughtPokemonItem pokemonItem)
            {
                pokemon = pokemonItem;
                UpdateData();
                PopulateButtonList();
            }
            else ClearPokemon();
        }

        public void ClearPokemon()
        {
            pokemon = null;
            if (PokemonImage != null)
            {
                TextPrompt.SetText(Language.GetText("Mods.Pokemod.MoveTutorUI.Prompt"));
                PokemonFrame.Color = new(0f, 0f, 0f, 0f);
                movesPanel.BackgroundColor = new(0f, 0f, 0f, 0f);
                movesPanel.BorderColor = new(0f, 0f, 0f, 0f);
                PokemonImage.SetAnimation(ModContent.Request<Texture2D>("Pokemod/Assets/Textures/UI/PokemonIconEmpty"));
                PokemonImage.Color = new(0f, 0f, 0f, 0f);
                PokemonImage.totalFrames = 1;
                PokemonImage.fromFrame = 0;
                PokemonImage.toFrame = 0;
            }
        }

        public void UpdateData()
		{
			string pokemonName = pokemon.PokemonName;
            int[] pokemonTypes = PokemonData.pokemonInfo[pokemonName].pokemonTypes;
            List<MoveLvl> learnSet = [.. PokemonData.pokemonInfo[pokemonName].movePool];

            //Fill Move List
			foreach (MoveLvl moveLvl in learnSet)
			{
                if (moveLvl.levelToLearn <= pokemon.level)
                {
                    moveList.Add(new MoveCost(moveLvl.moveName, moveLvl.levelToLearn));
                }
			}
            foreach (MoveCost teachingMove in teachingMoves)
            {
                foreach(int pokemonType in pokemonTypes)
                {
                    int moveType = PokemonData.pokemonAttacks[teachingMove.moveName].attackType;
                    if (moveType == pokemonType && !moveList.Contains(teachingMove))
                    {
                        moveList.Add(teachingMove);
                    }
                }
            }

            //Update Visuals
            if (ModContent.TryFind<ModProjectile>("Pokemod", pokemonName + "PetProjectile", out var pokemonProj))
            {
                if (pokemonProj is PokemonPetProjectile pokemonPet)
                {
                    PokemonFrame.Color = new(255,255,255,80);
                    movesPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
                    movesPanel.BorderColor = Color.Black * 0.7f;

                    Asset<Texture2D> pokemonSpriteSheet = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Pets/" 
                        + pokemonName + "PetProjectile" 
                        + (pokemon.Shiny ? "Shiny" : "") 
                        + ((pokemon.variant != null && pokemon.variant != "") ? "_" + pokemon.variant : ""));
                    PokemonImage.SetAnimation(pokemonSpriteSheet);
                    PokemonImage.Color = Color.White;
                    PokemonImage.totalFrames = pokemonPet.totalFrames;
                    PokemonImage.fromFrame = pokemonPet.idleStartEnd[0];
                    PokemonImage.toFrame = pokemonPet.idleStartEnd[1];
                }
            }
            TextPrompt.SetText(Language.GetText("Mods.Pokemod.MoveTutorUI.Description").WithFormatArgs(pokemonName));
        }

        private void SelectMove(int moveIndex)
        {
            Player player = Main.LocalPlayer;

            if (moveIndex < 0) 
            {
                SoundEngine.PlaySound(SoundID.MenuClose with { Pitch = -0.5f });
                CombatText.NewText(player.Hitbox, new Color(1f, 0.8f, 0.6f), "Move Already Known");
                return;
            }
            string move = moveList[moveIndex].moveName;
            int item = moveList[moveIndex].CostItemType;
            int amount = moveList[moveIndex].CostAmount;
            bool success = false;

            switch (item)
            {
                case ItemID.CopperCoin:
                    if (player.BuyItem(Item.buyPrice(copper: amount))) success = true;
                    break;
                case ItemID.SilverCoin:
                    if (player.BuyItem(Item.buyPrice(silver: amount))) success = true;
                    break;
                case ItemID.GoldCoin:
                    if (player.BuyItem(Item.buyPrice(gold: amount))) success = true;
                    break;
                case ItemID.PlatinumCoin:
                    if (player.BuyItem(Item.buyPrice(platinum: amount))) success = true;
                    break;
                default:
                    if (player.CountItem(item) >= amount)
                    {
                        int refundCount = 0;
                        for (int i = 0; i < amount; i++)
                        {
                            success = true;
                            if (!player.ConsumeItem(item))
                            {
                                success = false;
                                break;
                            }
                            refundCount++;
                        }
                        if (!success && refundCount > 0)
                        {
                            player.QuickSpawnItem(new EntitySource_Misc("RefundUsedItem"), item, refundCount);
                        }
                    }
                    break;
            }

            if (success)
            {
                pokemon.LearnMove(move, item, amount);
                SoundEngine.PlaySound(SoundID.Coins);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.MenuClose with { Pitch = -0.5f });
                CombatText.NewText(player.Hitbox, new Color(1f, 0.8f, 0.6f), "Too Expensive");
            }
        }

        public void ClosePanel()
        {
            Player player = Main.LocalPlayer;
            player.QuickSpawnItem(new EntitySource_Misc("RefundUsedItem"), pokeballItem[0], pokeballItem[0].stack);
            pokeballItem[0].TurnToAir();
            RemoveAllChildren();
        }
	}
}