﻿namespace GMTK2023
{
    internal class Player : EightWayMover
    {
        const float MOVE_SPEED = 10.0f;
        const float TURN_SPEED = 1.1f;

        #region rMembers

        #endregion rMembers


        #region rInit

        public Player(Vector2 pos, float angle) : base(pos, angle, TURN_SPEED)
        {

        }

        public override void LoadContent()
        {
            mDirectionTextures = new Animator[8];
            mDirectionTextures[(int)EightDirection.Up]          = MonoData.I.LoadAnimator("Player/PlayerUp");
            mDirectionTextures[(int)EightDirection.UpLeft]      = MonoData.I.LoadAnimator("Player/PlayerUpLeft");
            mDirectionTextures[(int)EightDirection.Left]        = MonoData.I.LoadAnimator("Player/PlayerLeft");
            mDirectionTextures[(int)EightDirection.DownLeft]    = MonoData.I.LoadAnimator("Player/PlayerDownLeft");
            mDirectionTextures[(int)EightDirection.Down]        = MonoData.I.LoadAnimator("Player/PlayerDown");
            mDirectionTextures[(int)EightDirection.DownRight]   = MonoData.I.LoadAnimator("Player/PlayerDownRight");
            mDirectionTextures[(int)EightDirection.Right]       = MonoData.I.LoadAnimator("Player/PlayerRight");
            mDirectionTextures[(int)EightDirection.UpRight]     = MonoData.I.LoadAnimator("Player/PlayerUpRight");
            mTexture = MonoData.I.MonoGameLoad<Texture2D>("Player/PlayerUp");
        }

        #endregion rInit


        #region rUpdate

        public override void Update(GameTime gameTime)
        {
            HandleKeys(gameTime);

            base.Update(gameTime);
        }

        void HandleKeys(GameTime gameTime)
        {
            bool up = InputManager.I.KeyHeld(GameKeys.MoveUp);
            bool down = InputManager.I.KeyHeld(GameKeys.MoveDown);
            bool left = InputManager.I.KeyHeld(GameKeys.MoveLeft);
            bool right = InputManager.I.KeyHeld(GameKeys.MoveRight);

            mSpeed = MOVE_SPEED;

            if (up)
            {
                if (left)
                {
                    TargetDirection(EightDirection.UpLeft);
                }
                else if (right)
                {
                    TargetDirection(EightDirection.UpRight);
                }
                else
                {
                    TargetDirection(EightDirection.Up);
                }
            }
            else if (left)
            {
                if(down)
                {
                    TargetDirection(EightDirection.DownLeft);
                }
                else
                {
                    TargetDirection(EightDirection.Left);
                }
            }
            else if(down)
            {
                if(right)
                {
                    TargetDirection(EightDirection.DownRight);
                }
                else
                {
                    TargetDirection(EightDirection.Down);
                }
            }
            else if(right)
            {
                TargetDirection(EightDirection.Right);
            }
            else
            {
                mSpeed = 0.0f;
            }
        }

        #endregion rUpdate
    }
}