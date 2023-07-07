namespace GMTK2023
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameScreen : Screen
    {
        public GameScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            SpriteFont pixelFont = FontManager.I.GetFont("Pixica-24");

            Vector2 centre = new Vector2(mScreenTarget.Width / 2, mScreenTarget.Height / 2);

            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(new Color(0, 0, 0));

            StartScreenSpriteBatch(info);

            MonoDraw.DrawStringCentred(info, pixelFont, centre, Color.Wheat, "THIS IS THE GAME SCREEN");

            EndScreenSpriteBatch(info);

            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
