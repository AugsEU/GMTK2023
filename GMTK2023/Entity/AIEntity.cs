﻿namespace GMTK2023
{
    internal class AIEntity : Motorbike
    {
        #region rConstants

        const float TARGET_REACHED_DIST = 40.0f;
        const double MIN_DIRECTION_CHANGE_TIME = 300.0;

        #endregion rConstants





        #region rTypes

        public enum AITeam
        {
            Ally,
            Enemy
        }

        #endregion rTypes


        #region rMembers

        Animator[][] mTeamSkins;
        AITeam mCurrentTeam;
        Vector2 mCurrentTarget;

        bool mStopped;
        MonoTimer mStopTimer;
        float mStopDuration;

        EightDirection mPrevDirection;
        PercentageTimer mDirChangeTimer;

        #endregion rMembers


        #region rInit

        public AIEntity(Vector2 pos, float angle) : base(pos, angle)
        {
            mCurrentTarget = Vector2.Zero;
            mStopped = false;
            mStopTimer = new MonoTimer();
            mStopTimer.Start();
            mStopDuration = 0.0f;
            mPrevDirection = GetCurrentDir();
            mDirChangeTimer = new PercentageTimer(MIN_DIRECTION_CHANGE_TIME);
            mDirChangeTimer.Start();
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

        #endregion rUpdate





        #region rUtil

        void SetTeam(AITeam aiTeam)
        {
            mCurrentTeam = aiTeam;
            mDirectionTextures = mTeamSkins[(int)aiTeam];
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

        #endregion rUtil
    }
}
