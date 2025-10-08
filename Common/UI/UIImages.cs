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
	internal class UIImageSection : UIImage
	{
		private Asset<Texture2D> _Texture;
		public int horizontalFrames;
		public int verticalFrames;
		public int xFrame;
		public int yFrame;

		public UIImageSection(Asset<Texture2D> texture, int horizontalFrames, int verticalFrames, int xFrame, int yFrame) : base(texture)
		{
			_Texture = texture;
			this.horizontalFrames = horizontalFrames;
			this.verticalFrames = verticalFrames;
			this.xFrame = xFrame;
			this.yFrame = yFrame;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(_Texture.Value, dimensions.Center(), _Texture.Frame(horizontalFrames, verticalFrames, xFrame, yFrame), Color, 0f, _Texture.Frame(horizontalFrames, verticalFrames).Size() * 0.5f, ImageScale, SpriteEffects.None, 0f);
		}
	}

	internal class UIAnimImage : UIImage
	{
		private Asset<Texture2D> _Texture;
		public int totalFrames;
		public int fromFrame;
		public int toFrame;

		public int frameRate;

		private int timer;
		private int currentFrame;

		public UIAnimImage(Asset<Texture2D> texture, int totalFrames, int fromFrame, int toFrame) : base(texture)
		{
			_Texture = texture;
			this.totalFrames = totalFrames;
			this.fromFrame = fromFrame;
			this.toFrame = toFrame;

			timer = 0;
			currentFrame = fromFrame;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(_Texture.Value, dimensions.Center(), _Texture.Frame(1, totalFrames, 0, currentFrame), Color, 0f, _Texture.Frame(1, totalFrames).Size() * 0.5f, ImageScale, SpriteEffects.None, 0f);
		}

		public override void Update(GameTime gameTime)
		{
			if (++timer > frameRate)
			{
				if (++currentFrame > toFrame)
				{
					currentFrame = fromFrame;
				}
				timer = 0;
			}
			base.Update(gameTime);
		}
	}
}