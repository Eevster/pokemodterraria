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

		public void SetAnimation(Asset<Texture2D> animTexture)
		{
			_Texture = animTexture;
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

	public class UIImageFlip : UIImage
	{
		private Asset<Texture2D> _texture;
		private Texture2D _nonReloadingTexture;

		public bool flipX;
		public bool flipY;

        public UIImageFlip(Asset<Texture2D> texture) : base(texture)
		{
			_texture = texture;
		}

		public UIImageFlip(Texture2D nonReloadingTexture) : base(nonReloadingTexture)
		{
			_nonReloadingTexture = nonReloadingTexture;
		}

        protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Texture2D texture2D = null;
			if (_texture != null)
				texture2D = _texture.Value;

			if (_nonReloadingTexture != null)
				texture2D = _nonReloadingTexture;

			if (ScaleToFit) {
				spriteBatch.Draw(texture2D, dimensions.ToRectangle(), Color);
				return;
			}

			Vector2 vector = texture2D.Size();
			Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) / 2f + vector * NormalizedOrigin;
			if (RemoveFloatingPointsFromDrawPosition)
				vector2 = vector2.Floor();

			spriteBatch.Draw(texture2D, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, (flipX && flipY)?(SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically):(flipX?SpriteEffects.FlipHorizontally:(flipY?SpriteEffects.FlipVertically:SpriteEffects.None)), 0f);
		}
	}
}