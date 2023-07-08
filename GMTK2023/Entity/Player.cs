namespace GMTK2023
{
    internal class Player : Motorbike
    {
        #region rConstants

        const float GRAPPLE_EXTEND_SPEED = 70.0f;
        const float GRAPPLE_MAX_LENGTH = 120.0f;
        const float GRAPPLE_CHANGE_SPEED = 0.4f;
        const float GRAPPLE_LOCK_DISTANCE = 25.0f;
        const float GRAPPLE_RADIAL_SPIN_SPEED = Motorbike.MAX_SPEED + 10.0f;
        const float GRAPPLE_RADIAL_SPIN_START_ANGLE = 0.9f;
        const float GRAPPLE_RADIAL_SPIN_MIN_RADIUS = 15.0f;
        const float GRAPPLE_ANGLE_TO_CHANGE_TEAM = MathF.PI * 1.85f;

        const int MAX_HEALTH = 7;

        #endregion rConstants





        #region rMembers

        bool mGrappleInAction;
        float mGrappleLength;
        Vector2 mGrappleDir;
        AIEntity mGrappledEntity;
        float mRadialAngleSpeed;
        float mRadialAngle;
        float mTotalRadialAngleTravelled;


        int mHealth;

        #endregion rMembers


        #region rInit

        public Player(Vector2 pos, float angle) : base(pos, angle)
        {
            mGrappledEntity = null;
            mGrappleInAction = false;
            mRadialAngleSpeed = 0.0f;
            mRadialAngle = 0.0f;
            mTotalRadialAngleTravelled = 0.0f;

            mHealth = MAX_HEALTH;
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
            float dt = Util.GetDeltaT(gameTime);

            HandleKeys(gameTime);
            HandleMouse(gameTime);

            UpdateGrapple(gameTime);

            base.Update(gameTime);

            if(MathF.Abs(mRadialAngleSpeed) > 0.0f && mGrappledEntity is not null)
            {
                mRadialAngle += dt * mRadialAngleSpeed;
                mTotalRadialAngleTravelled += MathF.Abs(dt * mRadialAngleSpeed);
                ForceAngle(-mRadialAngle - MathF.Sign(mRadialAngleSpeed) * MathF.PI / 2.0f);
                mSpeed = GRAPPLE_RADIAL_SPIN_SPEED;
                Vector2 calculatedPos = mGrappledEntity.GetCentrePos();
                calculatedPos.X += mGrappleLength * MathF.Cos(mRadialAngle);
                calculatedPos.Y += mGrappleLength * MathF.Sin(mRadialAngle);

                SetCentrePos(calculatedPos);
                ForceInBounds(GameScreen.PLAYABLE_AREA);

                if(mTotalRadialAngleTravelled > GRAPPLE_ANGLE_TO_CHANGE_TEAM)
                {
                    mGrappledEntity.SetTeam(AITeam.Ally);
                }
            }
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
            float dt = Util.GetDeltaT(gameTime);

            if (InputManager.I.KeyPressed(GameKeys.FireGun))
            {
                BeginGrapple();
            }

            if(!InputManager.I.KeyHeld(GameKeys.FireGun))
            {
                EndGrapple();
            }

            if(mGrappleInAction && mGrappledEntity is null)
            {
                Vector2 newGrappleDir = (InputManager.I.GetMouseWorldPos() - GetCentrePos());
                float newAngle = MathF.Atan2(newGrappleDir.Y, newGrappleDir.X);
                float currAngle = MathF.Atan2(mGrappleDir.Y, mGrappleDir.X);

                float angleDiff = MonoMath.GetAngleDiff(newAngle, currAngle);
                float maxAngleChange = GRAPPLE_CHANGE_SPEED * dt;

                angleDiff = Math.Clamp(angleDiff, -maxAngleChange, maxAngleChange);

                mGrappleDir = MonoMath.Rotate(mGrappleDir, angleDiff);
            }
        }

        void BeginGrapple()
        {
            mGrappleInAction = true;
            mGrappleLength = 0.0f;
            mGrappleDir = (InputManager.I.GetMouseWorldPos() - GetCentrePos());
            mGrappledEntity = null;
            mRadialAngleSpeed = 0.0f;
            mTotalRadialAngleTravelled = 0.0f;
        }

        void EndGrapple()
        {
            mGrappleInAction = false;
            mGrappledEntity = null;
            mRadialAngleSpeed = 0.0f;
            mTotalRadialAngleTravelled = 0.0f;
        }

        void UpdateGrapple(GameTime gameTime)
        {
            float dt = Util.GetDeltaT(gameTime);
            if (mGrappleInAction)
            {
                MonoDebug.DLog("Update grap {0} ", mRadialAngleSpeed);
                if (mGrappledEntity is null)
                {
                    MonoDebug.DLog("   Entity is null");
                    if (mGrappleLength >= GRAPPLE_MAX_LENGTH)
                    {
                        EndGrapple();
                    }
                    else
                    {
                        float speed = GRAPPLE_EXTEND_SPEED - (mGrappleLength / GRAPPLE_MAX_LENGTH) * GRAPPLE_EXTEND_SPEED * 0.85f;
                        mGrappleLength += dt * speed;
                    }

                    Vector2 grappleHead = GetGrappleHead();
                    AIEntity nearestToGrappleHead = EntityManager.I.GetNearestOfType<AIEntity>(GetGrappleHead());
                    float distanceToGrappleHead = (grappleHead - nearestToGrappleHead.GetCentrePos()).Length();

                    if(distanceToGrappleHead < GRAPPLE_LOCK_DISTANCE)
                    {
                        mGrappledEntity = nearestToGrappleHead;
                    }
                }
                else if(mRadialAngleSpeed == 0.0f)
                {
                    MonoDebug.DLog("    Consider spin");
                    mGrappleDir = (mGrappledEntity.GetCentrePos() - GetCentrePos());
                    mGrappleLength = mGrappleDir.Length();

                    Vector2 velocity = GetVelocity();

                    if (velocity.LengthSquared() > 0.0f)
                    {
                        Vector2 ourVelocityDir = Vector2.Normalize(GetVelocity());
                        Vector2 ourGrappleDir = Vector2.Normalize(mGrappleDir);

                        float angleDiff = MonoMath.GetAngleDiff(ourVelocityDir, ourGrappleDir);

                        bool angleIsGood = MathF.Abs(angleDiff) > GRAPPLE_RADIAL_SPIN_START_ANGLE;
                        bool grappleIsBigEnough = mGrappleLength > GRAPPLE_RADIAL_SPIN_MIN_RADIUS;
                        bool grappleIsTooBig = mGrappleLength > GRAPPLE_MAX_LENGTH;

                        MonoDebug.DLog("Bool checks: {0} {1} {2}", angleIsGood, grappleIsBigEnough, grappleIsTooBig);

                        if ((angleIsGood && grappleIsBigEnough) || grappleIsTooBig)
                        {
                            float crossProduct = ourVelocityDir.X * ourGrappleDir.Y - ourVelocityDir.Y * ourGrappleDir.X;
                            mRadialAngleSpeed = MathF.Sign(crossProduct) * GRAPPLE_RADIAL_SPIN_SPEED / mGrappleLength;
                            mRadialAngle = MathF.Atan2(-mGrappleDir.Y, -mGrappleDir.X);
                        }
                    }
                    else if(mGrappleLength > GRAPPLE_MAX_LENGTH)
                    {
                        EndGrapple();
                    }
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
                Vector2 grappleEnd = GetGrappleHead();

                if (mGrappledEntity is not null)
                {
                    grappleEnd = mGrappledEntity.GetCentrePos();
                }

                MonoDraw.DrawLine(info, grappleStart, grappleEnd, Color.Black, 2.0f, DrawLayer.SubEntity);
            }

            base.Draw(info);
        }

        #endregion rDraw


        #region rUtility

        Vector2 GetGrappleHead()
        {
            Vector2 grappleDir = mGrappleDir;
            grappleDir.Normalize();
            return mCentreOfMass + grappleDir * mGrappleLength;
        }

        public void AddHealth(int delta)
        {
            mHealth += delta;
            if(mHealth <= 0)
            {
                Kill();
            }
        }

        #endregion rUtility
    }
}
