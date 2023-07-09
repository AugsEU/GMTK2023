﻿using GMTK2023.UI;

namespace GMTK2023
{
    internal class TitleScreen : Screen
    {
        Texture2D mBackgroundTex;
        ScreenTransitionButton mStartGameBtn;

        public TitleScreen(GraphicsDeviceManager graphics) : base(graphics)
        {
            mStartGameBtn = new ScreenTransitionButton(new Vector2(500.0f, 200.0f), "Start Game", ScreenType.Game);
        }

        public override void LoadContent()
        {
            mBackgroundTex = MonoData.I.MonoGameLoad<Texture2D>("Backgrounds/TitleScreen");
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

            MonoDraw.DrawTextureDepth(info, mBackgroundTex, Vector2.Zero, DrawLayer.BackgroundElement);

            mStartGameBtn.Draw(info);

            EndScreenSpriteBatch(info);

            return mScreenTarget;
        }

        public override void Update(GameTime gameTime)
        {
            mStartGameBtn.Update(gameTime);
        }
    }
}
