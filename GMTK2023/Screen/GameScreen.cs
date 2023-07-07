﻿namespace GMTK2023
{
    /// <summary>
    /// Gameplay screen
    /// </summary>
    internal class GameScreen : Screen
    {
        Player testPlayer;

        public GameScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public override void OnActivate()
        {
            EntityManager.I.ClearEntities();
            testPlayer = new Player(new Vector2(100.0f, 100.0f), 0.0f);
            EntityManager.I.RegisterEntity(testPlayer);
            base.OnActivate();
        }

        public override RenderTarget2D DrawToRenderTarget(DrawInfo info)
        {
            //Draw out the game area
            info.device.SetRenderTarget(mScreenTarget);
            info.device.Clear(new Color(0, 0, 0));

            StartScreenSpriteBatch(info);

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
