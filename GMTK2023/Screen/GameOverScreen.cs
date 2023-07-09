namespace GMTK2023
{
    internal class GameOverScreen : Screen
    {
        public GameOverScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            SpriteFont pixelFont = FontManager.I.GetFont("Pixica-24");

            Vector2 centre = new Vector2(mScreenTarget.Width / 2, mScreenTarget.Height / 2);

            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(new Color(0, 0, 0));

            StartScreenSpriteBatch(info);

            MonoDraw.DrawStringCentred(info, pixelFont, centre, Color.White, "GAME OVER");

            EndScreenSpriteBatch(info);

            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.I.KeyPressed(GameKeys.Confirm))
            {
                ScreenManager.I.ActivateScreen(ScreenType.Title);
            }
        }
    }
}
