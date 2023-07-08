namespace GMTK2023
{
    internal class Player : Motorbike
    {
        #region rConstants

        const float GRAPPLE_EXTEND_SPEED = 70.0f;
        const float GRAPPLE_MAX_LENGTH = 180.0f;

        #endregion rConstants





        #region rMembers

        bool mGrappleInAction;
        float mGrappleLength;
        Vector2 mGrappleDir;

        AIEntity mGrappledEntity;

        #endregion rMembers


        #region rInit

        public Player(Vector2 pos, float angle) : base(pos, angle)
        {
            mGrappledEntity = null;
            mGrappleInAction = false;
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
            HandleMouse(gameTime);

            UpdateGrapple(gameTime);

            base.Update(gameTime);
        }

        void HandleKeys(GameTime gameTime)
        {
            bool up = InputManager.I.KeyHeld(GameKeys.MoveUp);
            bool down = InputManager.I.KeyHeld(GameKeys.MoveDown);
            bool left = InputManager.I.KeyHeld(GameKeys.MoveLeft);
            bool right = InputManager.I.KeyHeld(GameKeys.MoveRight);

            SetAcelerate(true);

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
                SetAcelerate(false);
            }
        }

        void HandleMouse(GameTime gameTime)
        {
            if(InputManager.I.KeyPressed(GameKeys.FireGun))
            {
                BeginGrapple();
            }

            if(!InputManager.I.KeyHeld(GameKeys.FireGun))
            {
                EndGrapple();
            }
        }

        void BeginGrapple()
        {
            mGrappleInAction = true;
            mGrappleLength = 0.0f;
            mGrappleDir = (InputManager.I.GetMouseWorldPos() - GetCentrePos());
        }

        void EndGrapple()
        {
            mGrappleInAction = false;
        }

        void UpdateGrapple(GameTime gameTime)
        {
            float dt = Util.GetDeltaT(gameTime);

            if (mGrappleInAction)
            {
                if (mGrappledEntity is null)
                {
                    if (mGrappleLength >= GRAPPLE_MAX_LENGTH)
                    {
                        EndGrapple();
                    }
                    else
                    {
                        float speed = GRAPPLE_EXTEND_SPEED - (mGrappleLength / GRAPPLE_MAX_LENGTH) * GRAPPLE_EXTEND_SPEED * 0.85f;
                        mGrappleLength += dt * speed;
                    }
                }
                else
                {
                    mGrappleDir = (mGrappledEntity.GetCentrePos() - GetCentrePos());
                }
            }
            else
            {
                float speed = GRAPPLE_EXTEND_SPEED - (mGrappleLength / GRAPPLE_MAX_LENGTH) * GRAPPLE_EXTEND_SPEED * 0.25f;
                speed *= 1.5f;
                mGrappleLength -= dt * speed;
            }
        }

        #endregion rUpdate





        #region rDraw

        public override void Draw(DrawInfo info)
        {
            // Grapple
            if (mGrappleLength > 0.0f)
            {
                Vector2 grappleStart = mCentreOfMass;
                Vector2 grappleDir = mGrappleDir;
                grappleDir.Normalize();
                Vector2 grappleEnd = grappleStart + grappleDir * mGrappleLength;

                if (mGrappledEntity is not null)
                {
                    grappleEnd = mGrappledEntity.GetCentrePos();
                }

                MonoDraw.DrawLine(info, grappleStart, grappleEnd, Color.Black, 2.0f, DrawLayer.SubEntity);
            }

            base.Draw(info);
        }

        #endregion rDraw
    }
}
