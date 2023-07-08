using System.ComponentModel.DataAnnotations;

namespace GMTK2023
{
    abstract class Motorbike : EightWayMover
    {
        const float MAX_SPEED = 30.0f;
        const float TURN_SPEED = 0.8f;
        const float FRICTION = 8.0f;
        const float ACELERATE = 1.0f;
        const float MIN_SPEED = 15.0f;
        const float BOOST_SPEED = 10.0f;

        bool mAccelerating;

        protected Motorbike(Vector2 pos, float angle) : base(pos, angle, TURN_SPEED)
        {
            mAccelerating = false;
        }

        protected void SetAcelerate(bool acel)
        {
            mAccelerating = acel;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = Util.GetDeltaT(gameTime);

            if(mAccelerating)
            {
                mSpeed = Math.Clamp(mSpeed, MIN_SPEED, MAX_SPEED);
                mSpeed += dt * ACELERATE;
            }
            else
            {
                mSpeed -= dt * FRICTION;
                if(mSpeed < MIN_SPEED)
                {
                    mSpeed = 0.0f;
                }
            }

            mSpeed = Math.Clamp(mSpeed, 0.0f, MAX_SPEED);

            base.Update(gameTime);
        }
    }
}
