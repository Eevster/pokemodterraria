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

		public UIHoverImageButton(Asset<Texture2D> texture, string hoverText) : base(texture) {
			this.hoverText = hoverText;
		}

        public UIHoverImageButton(Asset<Texture2D> texture, Asset<Texture2D> image, Color color, string hoverText) : base(texture) {
			this.hoverText = hoverText;
            this.image = image;
            this.color = color;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			// When you override UIElement methods, don't forget call the base method
			// This helps to keep the basic behavior of the UIElement
			if(drawPanel) base.DrawSelf(spriteBatch);

            if(image != null){
                CalculatedStyle innerDimensions = GetInnerDimensions();
                float shopx = innerDimensions.X;
			    float shopy = innerDimensions.Y;
                spriteBatch.Draw(image.Value, new Vector2(shopx + ((Width.Pixels - image.Width())/2), shopy + ((Height.Pixels - image.Height())/2) + (IsMouseHovering?-hoverUp:0)), color);
            }

			// IsMouseHovering becomes true when the mouse hovers over the current UIElement
			if (IsMouseHovering){
				Main.hoverItemName = hoverText;
            }
		}

        public override void Update(GameTime gameTime) {
			base.Update(gameTime);

			// Checking ContainsPoint and then setting mouseInterface to true is very common
			// This causes clicks on this UIElement to not cause the player to use current items
			if (ContainsPoint(Main.MouseScreen)) {
				Main.LocalPlayer.mouseInterface = true;
			}
        }
	}

	internal class UIHoverPokeballButton : UIHoverImageButton
	{
		internal string pokemonName;
		internal Asset<Texture2D> pokemonTexture;

        public UIHoverPokeballButton(Asset<Texture2D> texture, Asset<Texture2D> image, Color color, string hoverText) : base(texture, image, color, hoverText) {
			this.hoverText = hoverText;
            this.image = image;
            this.color = color;
		}

		public void SetPokemon(string pokemonName){
			this.pokemonName = pokemonName;
			pokemonTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/Pokesprites/Icons/"+pokemonName);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);

			if (IsMouseHovering){
				CalculatedStyle innerDimensions = GetInnerDimensions();
				float shopx = innerDimensions.X;
				float shopy = innerDimensions.Y;

				if(pokemonTexture != null){
					spriteBatch.Draw(pokemonTexture.Value, new Vector2(shopx + Width.Pixels/2, shopy - 25f - 4f*pokemonTexture.Height()/2 + (IsMouseHovering?-hoverUp:0)), pokemonTexture.Value.Bounds, color, 0f, pokemonTexture.Size()*0.5f, 4f, SpriteEffects.None, 0);
				}

				if(pokemonName != null){
					if(pokemonName != ""){
						Vector2 vector2 = ((DynamicSpriteFont)FontAssets.MouseText).MeasureString(pokemonName);
						DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, (DynamicSpriteFont)FontAssets.MouseText, pokemonName, new Vector2(shopx + Width.Pixels/2, shopy - 15f + (IsMouseHovering?-hoverUp:0)), Color.White, 0, vector2*0.5f, 1.5f, SpriteEffects.None, 0);
					}
				}
			}
		}
	}
}