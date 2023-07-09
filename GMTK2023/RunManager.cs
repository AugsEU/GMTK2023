namespace GMTK2023
{
    class RunManager : Singleton<RunManager>
    {
        const int NUM_HEALTH_PACKS = 2;

        int mHighScore = 0;
        int mCurrentHealth;
        int mHealthPacksRemaining;
        int mRoundNumber;
        bool mRunStarted = false;


        public bool HasStarted()
        {
            return mRunStarted;
        }

        public void StartRun()
        {
            mHealthPacksRemaining = NUM_HEALTH_PACKS;
            mCurrentHealth = Player.MAX_HEALTH;
            mRoundNumber = 0;
            mRunStarted = true;
        }


        public void EndRound()
        {
            mRoundNumber++;
        }

        public void EndRun()
        {
            mRunStarted = false;
        }

        public int GetHealth()
        {
            return mCurrentHealth;
        }


        public int GetNumberOfEnemies()
        {
            return 2 * (int)MathF.Ceiling(MathF.Sqrt(mRoundNumber + 0.25f));
        }

        public void UseHealthPack()
        {
            if (mHealthPacksRemaining > 0)
            {
                mCurrentHealth = Player.MAX_HEALTH;
                mHealthPacksRemaining--;
            }
        }
    }
}
