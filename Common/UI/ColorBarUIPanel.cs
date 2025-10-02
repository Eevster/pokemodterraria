using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Pokemod.Common.UI
{
    public class ColorBarUIPanel : UIPanel
    {
        private float barFill = 0.5f;
        private int _cornerSize = 12;
	    private int _barSize = 4;
        private Asset<Texture2D> _borderTexture;
	    private Asset<Texture2D> _backgroundTexture;
        public Color BarColor = new Color(255, 255, 255) * 0.7f;

        // Added by TML.
        private bool _needsTextureLoading;

        private void LoadTextures()
        {
            // These used to be moved to OnActivate in order to avoid texture loading on JIT thread.
            // Doing so caused issues with missing backgrounds and borders because Activate wasn't always being called.
            if (_borderTexture == null)
                _borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder");

            if (_backgroundTexture == null)
                _backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground");
        }

        public ColorBarUIPanel(float barFill = 0.5f) : base()
        {
            this.barFill = barFill;
            SetPadding(_cornerSize);
            _needsTextureLoading = true;
        }
        
        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
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

        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, float barFill, Color color, Color color2)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - _cornerSize, point.Y + (int)dimensions.Height - _cornerSize);
            int width = point2.X - point.X - _cornerSize;
            int height = point2.Y - point.Y - _cornerSize;

            int limitPoint = new Point(point.X + (int)(dimensions.Width*barFill), point.Y).X;
            
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, _cornerSize, _cornerSize), new Rectangle(0, 0, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, width, _cornerSize), new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, width, _cornerSize), new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(0, _cornerSize, _cornerSize, _barSize), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, width, height), new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color);

            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, (point.X+_cornerSize>limitPoint)?(limitPoint-point.X):_cornerSize, _cornerSize), new Rectangle(0, 0, _cornerSize, _cornerSize), color2); //CornerUpLeft
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, (point2.X+_cornerSize>limitPoint)?(limitPoint-point2.X):_cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color2); //CornerUpRight
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, (point.X+_cornerSize>limitPoint)?(limitPoint-point.X):_cornerSize, _cornerSize), new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color2); //CornerDownLeft
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, (point2.X+_cornerSize>limitPoint)?(limitPoint-point2.X):_cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color2); //CornerDownRight
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, (point.X+_cornerSize+width>limitPoint)?(limitPoint-point.X-_cornerSize):width, _cornerSize), new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color2); //MiddleUp
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, (point.X+_cornerSize+width>limitPoint)?(limitPoint-point.X-_cornerSize):width, _cornerSize), new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color2); // MiddleDown
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, (point.X+_cornerSize>limitPoint)?(limitPoint-point.X):_cornerSize, height), new Rectangle(0, _cornerSize, _cornerSize, _barSize), color2); //LeftCenter
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, (point2.X+_cornerSize>limitPoint)?(limitPoint-point2.X):_cornerSize, height), new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color2); // RightCenter
            spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, (point.X+_cornerSize+width>limitPoint)?(limitPoint-point.X-_cornerSize):width, height), new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color2); //MiddleCenter
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_needsTextureLoading)
            {
                _needsTextureLoading = false;
                LoadTextures();
            }

            if (_backgroundTexture != null)
                DrawPanel(spriteBatch, _backgroundTexture.Value, barFill, BackgroundColor, BarColor);

            if (_borderTexture != null)
                DrawPanel(spriteBatch, _borderTexture.Value, BorderColor);
        }
    }
}