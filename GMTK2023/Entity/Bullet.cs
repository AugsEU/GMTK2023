namespace GMTK2023
{
    internal class Bullet : Entity
    {
        const float BULLET_SPEED = 60.0f;

        AITeam mTeam;
        Vector2 mVelocity;

        public Bullet(Vector2 pos, EightDirection dir, AITeam mTeam) : base(pos)
        {
            float angle = Util.GetAngleFromDirection(dir);
            mVelocity = new Vector2(BULLET_SPEED, 0.0f);
            mVelocity = MonoMath.Rotate(mVelocity, angle);
        }

        public override void LoadContent()
        {
            switch (mTeam)
            {
                case AITeam.Ally:
                    mTexture = MonoData.I.MonoGameLoad<Texture2D>("Bullet/BulletAlly");
                    break;
                case AITeam.Enemy:
                    mTexture = MonoData.I.MonoGameLoad<Texture2D>("Bullet/BulletEnemy");
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            float dt = Util.GetDeltaT(gameTime);

            mPosition += mVelocity * dt;

            if(mPosition.X < mTexture.Width || mPosition.X > Screen.SCREEN_WIDTH + mTexture.Width
                || mPosition.Y < mTexture.Height || mPosition.Y > Screen.SCREEN_HEIGHT + mTexture.Height)
            {
                EntityManager.I.QueueDeleteEntity(this);
            }

            base.Update(gameTime);
        }

        public override void Draw(DrawInfo info)
        {
            MonoDraw.DrawTextureDepth(info, mTexture, mPosition, DrawLayer.Bullets);
        }

        public override void OnCollideEntity(Entity entity)
        {
            if (entity is AIEntity)
            {
                AIEntity aiEntity = (AIEntity)entity;

                if(aiEntity.GetTeam() == AITeam.Ally && mTeam == AITeam.Enemy)
                {
                    aiEntity.SetTeam(AITeam.Enemy);
                }
                else if(aiEntity.GetTeam() == AITeam.Enemy && mTeam == AITeam.Ally)
                {
                    aiEntity.Kill();
                }
            }
            else if(entity is Player)
            {
                Player player = (Player)entity;

                player.AddHealth(1);
            }

            base.OnCollideEntity(entity);
        }
    }
}
