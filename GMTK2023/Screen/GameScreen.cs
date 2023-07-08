namespace GMTK2023
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameScreen : Screen
    {
        public static Rectangle PLAYABLE_AREA = new Rectangle(0, 88, 960, 417);


        Texture2D mBG;

        Player mPlayer;

        public GameScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public override void LoadContent()
        {
            mBG = MonoData.I.MonoGameLoad<Texture2D>("Backgrounds/GameScreen");

            base.LoadContent();
        }

        public override void OnActivate()
        {
            EntityManager.I.ClearEntities();
            AITargetManager.I.Init();
            mPlayer = new Player(new Vector2(100.0f, 100.0f), 0.0f);
            EntityManager.I.RegisterEntity(mPlayer);
            EntityManager.I.RegisterEntity(new AIEntity(new Vector2(500.0f, 200.0f), MathF.PI));
            EntityManager.I.RegisterEntity(new AIEntity(new Vector2(500.0f, 200.0f), MathF.PI));
            base.OnActivate();
        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(new Color(0, 0, 0));

            StartScreenSpriteBatch(info);

            MonoDraw.DrawTextureDepth(info, mBG, Vector2.Zero, DrawLayer.Background);
            EntityManager.I.Draw(info);

            EndScreenSpriteBatch(info);

            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {
            EntityManager.I.Update(gameTime);
        }
    }
}
