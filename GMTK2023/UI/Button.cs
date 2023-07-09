namespace GMTK2023
{
    abstract class Button
    {
        enum ButtonState
        {
            Normal,
            Hover,
            Pressed
        }

        Texture2D mBackTexture;
        Texture2D mHoverTexture;
        Texture2D mPressedTexture;
        ButtonState mState;
        Vector2 mPosition;
        string mText;

        public Button(Vector2 topLeft, string Text)
        {
            mPosition = topLeft;
            mText = Text;

            mBackTexture = MonoData.I.MonoGameLoad<Texture2D>("UI/ButtonBack");
            mHoverTexture = MonoData.I.MonoGameLoad<Texture2D>("UI/ButtonBackHover");
            mPressedTexture = MonoData.I.MonoGameLoad<Texture2D>("UI/ButtonBackPress");
        }

        public void Update(GameTime gameTime)
        {
            if(IsMouseOver())
            {
                if(InputManager.I.IsLClickDown())
                {
                    mState = ButtonState.Pressed;
                }
                else 
                {
                    if (mState == ButtonState.Pressed)
                    {
                        DoAction();
                    }
                    mState = ButtonState.Hover;
                }
            }
            else
            {
                mState = ButtonState.Normal;
            }
        }

        bool IsMouseOver()
        {
            Vector2 mousePos = InputManager.I.GetMouseWorldPos();

            return mousePos.X > mPosition.X && mousePos.X < mPosition.X + mBackTexture.Width
                && mousePos.Y > mPosition.Y && mousePos.Y < mPosition.Y + mBackTexture.Height;
        }

        public void Draw(DrawInfo info)
        {
            Texture2D tex = mBackTexture;
            if(mState == ButtonState.Hover)
            {
                tex = mHoverTexture;
            }
            else if(mState == ButtonState.Pressed)
            {
                tex = mPressedTexture;
            }

            MonoDraw.DrawTextureDepth(info, tex, mPosition, DrawLayer.Text);

            SpriteFont font = FontManager.I.GetFont("Pixica-24");
            Vector2 strPos = new Vector2(mBackTexture.Width, mBackTexture.Height) * 0.5f + mPosition;
            MonoDraw.DrawShadowStringCentred(info, font, strPos, Color.AliceBlue, mText, DrawLayer.Text);
        }

        protected abstract void DoAction();
    }
}
