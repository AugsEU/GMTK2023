namespace GMTK2023
{
    #region rTypes

    public enum AITeam
    {
        Ally,
        Enemy
    }

    #endregion rTypes

    internal class AIEntity : Motorbike
    {
        #region rConstants

        const float MAX_SPEED = 20.0f;
        const float TARGET_REACHED_DIST = 40.0f;
        const double MIN_DIRECTION_CHANGE_TIME = 300.0;

        #endregion rConstants


        #region rMembers

        Animator[][] mTeamSkins;
        AITeam mCurrentTeam;
        Vector2 mCurrentTarget;

        bool mStopped;
        MonoTimer mStopTimer;
        float mStopDuration;

        EightDirection mPrevDirection;
        PercentageTimer mDirChangeTimer;

        MonoTimer mShootBulletTimer;
        float mShootDuration;

        bool mIsDead = false;

        #endregion rMembers


        #region rInit

        public AIEntity(Vector2 pos, float angle) : base(pos, angle, MAX_SPEED)
        {
            mCurrentTarget = Vector2.Zero;
            mStopped = false;
            mStopTimer = new MonoTimer();
            mStopTimer.Start();
            mStopDuration = 0.0f;
            mPrevDirection = GetCurrentDir();
            mDirChangeTimer = new PercentageTimer(MIN_DIRECTION_CHANGE_TIME);
            mDirChangeTimer.Start();

            mShootBulletTimer = new MonoTimer();
            mShootBulletTimer.Start();
            mShootDuration = RandomManager.I.GetWorld().GetFloatRange(3500.0f, 12000.0f);
        }

        public override void LoadContent()
        {
            mTeamSkins = new Animator[2][];
            mTeamSkins[(int)AITeam.Ally] = new Animator[8];
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.Up]          = MonoData.I.LoadAnimator("Enemies/AllyUp");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.UpLeft]      = MonoData.I.LoadAnimator("Enemies/AllyUpLeft");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.Left]        = MonoData.I.LoadAnimator("Enemies/AllyLeft");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.DownLeft]    = MonoData.I.LoadAnimator("Enemies/AllyDownLeft");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.Down]        = MonoData.I.LoadAnimator("Enemies/AllyDown");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.DownRight]   = MonoData.I.LoadAnimator("Enemies/AllyDownRight");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.Right]       = MonoData.I.LoadAnimator("Enemies/AllyRight");
            mTeamSkins[(int)AITeam.Ally][(int)EightDirection.UpRight]     = MonoData.I.LoadAnimator("Enemies/AllyUpRight");

            mTeamSkins[(int)AITeam.Enemy] = new Animator[8];
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.Up]          = MonoData.I.LoadAnimator("Enemies/EnemyUp");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.UpLeft]      = MonoData.I.LoadAnimator("Enemies/EnemyUpLeft");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.Left]        = MonoData.I.LoadAnimator("Enemies/EnemyLeft");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.DownLeft]    = MonoData.I.LoadAnimator("Enemies/EnemyDownLeft");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.Down]        = MonoData.I.LoadAnimator("Enemies/EnemyDown");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.DownRight]   = MonoData.I.LoadAnimator("Enemies/EnemyDownRight");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.Right]       = MonoData.I.LoadAnimator("Enemies/EnemyRight");
            mTeamSkins[(int)AITeam.Enemy][(int)EightDirection.UpRight]     = MonoData.I.LoadAnimator("Enemies/EnemyUpRight");

            mTexture = MonoData.I.MonoGameLoad<Texture2D>("Enemies/AllyUp");

            SetTeam(AITeam.Enemy);
        }

        #endregion rInit





        #region rUpdate

        public override void Update(GameTime gameTime)
        {
            if(mCurrentTarget == Vector2.Zero)
            {
                GetNewTarget();
            }

            ConsiderStop();

            GoToTarget(gameTime);
            SetAcelerate(!mStopped);

            if(mShootBulletTimer.GetElapsedMs() > mShootDuration)
            {
                ShootBullet();
                mShootBulletTimer.Reset();
                mShootDuration = RandomManager.I.GetWorld().GetFloatRange(1500.0f, 5000.0f);
            }

            base.Update(gameTime);
        }

        void GoToTarget(GameTime gameTime)
        {
            if((mCentreOfMass - mCurrentTarget).LengthSquared() < TARGET_REACHED_DIST * TARGET_REACHED_DIST)
            {
                GetNewTarget();
            }

            Vector2 toTarget = mCurrentTarget - mCentreOfMass;
            float angle = -MathF.Atan2(toTarget.Y, toTarget.X);
            EightDirection dir = Util.GetDirectionFromAngle(angle);

            if(dir != mPrevDirection && mDirChangeTimer.GetPercentageF() >= 1.0f)
            {
                TargetDirection(dir);
                mPrevDirection = dir;
                mDirChangeTimer.Reset();
                mDirChangeTimer.Start();
            }
        }

        void ConsiderStop()
        {
            if (mStopDuration == 0.0f)
            {
                if(mStopped)
                {
                    mStopDuration = RandomManager.I.GetWorld().GetFloatRange(1500.0f, 2500.0f);
                }
                else
                {
                    mStopDuration = RandomManager.I.GetWorld().GetFloatRange(10500.0f, 25500.0f);
                }
            }

            if (mStopTimer.GetElapsedMs() > mStopDuration)
            {
                mStopped = !mStopped;
                mStopDuration = 0.0f;
                mStopTimer.Reset();
            }
        }


        void ShootBullet()
        {
            EntityManager.I.QueueRegisterEntity(new Bullet(mPosition, GetCurrentDir(), GetTeam()));
        }

        public override void Kill()
        {
            mIsDead = true;

            base.Kill();
        }

        #endregion rUpdate





        #region rUtil

        public void SetTeam(AITeam aiTeam)
        {
            mCurrentTeam = aiTeam;
            mDirectionTextures = mTeamSkins[(int)aiTeam];
        }

        public AITeam GetTeam()
        {
            return mCurrentTeam;
        }

        void GetNewTarget()
        {
            if (mCurrentTarget != Vector2.Zero)
            {
                AITargetManager.I.ReportReachedPoint(mCurrentTarget);
            }

            mCurrentTarget = AITargetManager.I.GiveMeATarget();

            if(RandomManager.I.GetWorld().PercentChance(10.0f))
            {
                mStopDuration = RandomManager.I.GetWorld().GetFloatRange(1500.0f, 2500.0f);
                mStopped = true;
                mStopTimer.Reset();
            }
        }

        public bool IsDead()
        {
            return mIsDead;
        }

        #endregion rUtil
    }
}
