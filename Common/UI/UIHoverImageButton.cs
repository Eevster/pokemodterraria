using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.UI
{
	// This ExampleUIHoverImageButton class inherits from UIImageButton. 
	// Inheriting is a great tool for UI design. 
	// By inheriting, we get the Image drawing, MouseOver sound, and fading for free from UIImageButton
	// We've added some code to allow the Button to show a text tooltip while hovered
	internal class UIHoverImageButton : UIImageButton
	{
		// Tooltip text that will be shown on hover
		internal string hoverText;
		internal Asset<Texture2D> image;
		internal Color color;
		internal bool drawPanel = true;
		internal float hoverUp;

		internal bool canBeSelected;
		internal bool selected;

		public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture)
		{
			this.hoverText = hoverText;
		}

		public UIHoverImageButton(Asset<Texture2D> texture, Asset<Texture2D> image, Color color, string hoverText) : base(texture)
		{
			this.hoverText = hoverText;
			this.image = image;
			this.color = color;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			// When you override UIElement methods, don't forget call the base method
			// This helps to keep the basic behavior of the UIElement
			if (drawPanel) base.DrawSelf(spriteBatch);

			if (image != null && (!canBeSelected || (canBeSelected && (selected || IsMouseHovering))))
			{
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float shopx = innerDimensions.X;
				float shopy = innerDimensions.Y;
				spriteBatch.Draw(image.Value, new Vector2(shopx + ((Width.Pixels - image.Width()) / 2), shopy + ((Height.Pixels - image.Height()) / 2) + (IsMouseHovering ? -hoverUp : 0)), color);
			}

			// IsMouseHovering becomes true when the mouse hovers over the current UIElement
			if (IsMouseHovering)
			{
				Main.hoverItemName = hoverText;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Checking ContainsPoint and then setting mouseInterface to true is very common
			// This causes clicks on this UIElement to not cause the player to use current items
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}
	}
	
	internal class UIHoverPanelImageButton : UIHoverImageButton
	{
		private Asset<Texture2D> _borderTexture;
		private Asset<Texture2D> _backgroundTexture;
		private Asset<Texture2D> _Texture;
		public Color BorderColor = Color.Black;
		public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;

		private float _visibilityActive = 1f;
		private float _visibilityInactive = 0.75f;
	
		// Added by TML.
		private bool _needsTextureLoading;

		private void LoadTextures()
		{
			if (_borderTexture == null)
				_borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder");

			if (_backgroundTexture == null)
				_backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground");
		}

		public UIHoverPanelImageButton(Asset<Texture2D> texture, string hoverText) : base(texture, hoverText)
		{
			_Texture = texture;
			_needsTextureLoading = true;
		}

		public UIHoverPanelImageButton(Asset<Texture2D> texture, Asset<Texture2D> image, Color color, string hoverText) : base(texture, image, color, hoverText)
		{
			_Texture = texture;
			_needsTextureLoading = true;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_needsTextureLoading)
			{
				_needsTextureLoading = false;
				LoadTextures();
			}

			if (_backgroundTexture != null)
				DrawPanel(spriteBatch, _backgroundTexture.Value, BackgroundColor);

			if (_borderTexture != null)
				DrawPanel(spriteBatch, _borderTexture.Value, BorderColor);

			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(_Texture.Value, dimensions.Center()-0.5f*_Texture.Size(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
			
			if (image != null && (!canBeSelected || (canBeSelected && (selected || IsMouseHovering))))
			{
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float shopx = innerDimensions.X;
				float shopy = innerDimensions.Y;
				spriteBatch.Draw(image.Value, new Vector2(shopx + ((Width.Pixels - image.Width()) / 2), shopy + ((Height.Pixels - image.Height()) / 2) + (IsMouseHovering ? -hoverUp : 0)), color);
			}

			// IsMouseHovering becomes true when the mouse hovers over the current UIElement
			if (IsMouseHovering)
			{
				Main.hoverItemName = hoverText;
			}
		}

		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			int _cornerSize = 12;
			int _barSize = 4;

			color *= (base.IsMouseHovering ? _visibilityActive : _visibilityInactive);

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width - _cornerSize, point.Y + (int)dimensions.Height - _cornerSize);
			int width = point2.X - point.X - _cornerSize;
			int height = point2.Y - point.Y - _cornerSize;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, _cornerSize, _cornerSize), new Rectangle(0, 0, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, width, _cornerSize), new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, width, _cornerSize), new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(0, _cornerSize, _cornerSize, _barSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, width, height), new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color);
		}
	}

	internal class UIHoverPokeballButton : UIHoverImageButton
	{
		internal string pokemonName;
		internal Asset<Texture2D> pokemonTexture;

		public UIHoverPokeballButton(Asset<Texture2D> texture, Asset<Texture2D> image, Color color, string hoverText) : base(texture, image, color, hoverText)
		{
			this.hoverText = hoverText;
			this.image = image;
			this.color = color;
		}

		public void SetPokemon(string pokemonName)
		{
			this.pokemonName = pokemonName;
			pokemonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/" + pokemonName);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			if (IsMouseHovering)
			{
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float shopx = innerDimensions.X;
				float shopy = innerDimensions.Y;

				if (pokemonTexture != null)
				{
					spriteBatch.Draw(pokemonTexture.Value, new Vector2(shopx + Width.Pixels / 2, shopy - 25f - 4f * pokemonTexture.Height() / 2 + (IsMouseHovering ? -hoverUp : 0)), pokemonTexture.Value.Bounds, color, 0f, pokemonTexture.Size() * 0.5f, 4f, SpriteEffects.None, 0);
				}

				if (pokemonName != null)
				{
					if (pokemonName != "")
					{
						Vector2 vector2 = ((DynamicSpriteFont)FontAssets.MouseText).MeasureString(pokemonName);
						DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, (DynamicSpriteFont)FontAssets.MouseText, pokemonName, new Vector2(shopx + Width.Pixels / 2, shopy - 15f + (IsMouseHovering ? -hoverUp : 0)), Color.White, 0, vector2 * 0.5f, 1.5f, SpriteEffects.None, 0);
					}
				}
			}
		}
	}
}