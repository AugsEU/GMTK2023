namespace GMTK2023
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameScreen : Screen
    {
        public static Rectangle PLAYABLE_AREA = new Rectangle(0, 88, 960, 417);
        public const double READY_TIME = 1500.0;
        public const double GO_TIME = 500.0;

        #region rMembers

        // Textures
        Texture2D mBG;
        Texture2D mHealthBarBG;
        Texture2D mHealthBarSegStart;
        Texture2D mHealthBarSegMid;
        Texture2D mHealthBarSegEnd;

        Player mPlayer;

        MonoTimer mReadyGoTimer;

        #endregion rMembers


        #region rInit

        public GameScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
            mReadyGoTimer = new MonoTimer();
        }

        public override void LoadContent()
        {
            mBG = MonoData.I.MonoGameLoad<Texture2D>("Backgrounds/GameScreen");

            mHealthBarBG = MonoData.I.MonoGameLoad<Texture2D>("UI/HealthBar");
            mHealthBarSegStart = MonoData.I.MonoGameLoad<Texture2D>("UI/HealthBarSegStart");
            mHealthBarSegMid = MonoData.I.MonoGameLoad<Texture2D>("UI/HealthBarSegMid");
            mHealthBarSegEnd = MonoData.I.MonoGameLoad<Texture2D>("UI/HealthBarSegEnd");


            base.LoadContent();
        }

        public override void OnActivate()
        {
            mReadyGoTimer.FullReset();
            mReadyGoTimer.Start();

            EntityManager.I.ClearEntities();
            AITargetManager.I.Init();

            SpawnInitialEntities();
            base.OnActivate();
        }


        void SpawnInitialEntities()
        {
            RandomManager.I.GetWorld().ChugNumber(DateTime.Now.Millisecond);

            AITargetManager.I.Init();
            Vector2 playerSpawn = new Vector2(100.0f, 100.0f);
            mPlayer = new Player(playerSpawn, 0.0f);

            EntityManager.I.RegisterEntity(mPlayer);
            AITargetManager.I.RegisterPos(playerSpawn);

            int numToSpawn = 10;

            for(int i = 0; i < numToSpawn; i++)
            {
                Vector2 pos = AITargetManager.I.GiveMeATarget();
                pos.X = (pos.X - SCREEN_WIDTH) / 2.0f + SCREEN_WIDTH;

                AIEntity newEntity = new AIEntity(pos, MathF.PI);
                EntityManager.I.RegisterEntity(newEntity);
            }

            AITargetManager.I.Init();
        }

        #endregion rInit


        #region rUpdate

        public override void Update(GameTime gameTime)
        {
            if(mReadyGoTimer.GetElapsedMs() > READY_TIME + GO_TIME)
            {
                EntityManager.I.Update(gameTime);
            }
        }

        #endregion rUpdate

        #region rDraw

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(new Color(0, 0, 0));

            StartScreenSpriteBatch(info);

            MonoDraw.DrawTextureDepth(info, mBG, Vector2.Zero, DrawLayer.Background);
            EntityManager.I.Draw(info);

            DrawUI(info);

            EndScreenSpriteBatch(info);

            return mScreenTarget;
        }


        public void DrawUI(DrawInfo info)
        {
            if(mReadyGoTimer.GetElapsedMs() < READY_TIME + GO_TIME)
            {
                DrawReadyGoText(info);
            }

            DrawHealthBar(info, new Vector2(44.0f, 20.0f));
        }

        public void DrawHealthBar(DrawInfo info, Vector2 topLeft)
        {
            int health = mPlayer.GetHealth();

            MonoDraw.DrawTextureDepth(info, mHealthBarBG, topLeft, DrawLayer.Text);

            for (int i = 0; i < health; i++)
            {
                Texture2D segTexture = mHealthBarSegMid;
                if(i == 0)
                {
                    segTexture = mHealthBarSegStart;
                }
                else if (i == Player.MAX_HEALTH - 1)
                {
                    segTexture = mHealthBarSegEnd;
                }

                MonoDraw.DrawTextureDepth(info, segTexture, topLeft, DrawLayer.Text);

                topLeft.X += segTexture.Width;
            }

        }


        public void DrawReadyGoText(DrawInfo info)
        {
            SpriteFont font = FontManager.I.GetFont("Pixica-24");
            double time = mReadyGoTimer.GetElapsedMs();
            string text = "Ready?";
            Vector2 pos = new Vector2(SCREEN_WIDTH / 2.0f, 0.0f);

            if(time > READY_TIME)
            {
                text = "GO!";
                pos.Y = SCREEN_HEIGHT / 2.0f;
            }
            else
            {
                float t = MathF.Min((float)(time / READY_TIME) * 2.0f, 1.0f);
                pos.Y = t * SCREEN_HEIGHT / 2.0f;
            }

            MonoDraw.DrawStringCentred(info, font, pos + new Vector2(2.0f, 2.0f), Color.DarkBlue, text, DrawLayer.Text);
            MonoDraw.DrawStringCentred(info, font, pos, Color.White, text, DrawLayer.Text);
        }

        #endregion rDraw




    }
}
