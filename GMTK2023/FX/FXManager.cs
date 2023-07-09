namespace GMTK2023
{
	/// <summary>
	/// Manages all effects that display on screen.
	/// </summary>
	internal class FXManager : Singleton<FXManager>
	{
		#region rMembers

		List<FX> mFXList = new List<FX>();
		int mScreenWidth = 0;
		int mScreenHeight = 0;

		#endregion





		#region rInitialisation

		/// <summary>
		/// Initialise the FXManager with a screen width and height
		/// </summary>
		public void Init(int screenWidth, int screenHeight)
		{
			mScreenHeight = screenHeight;
			mScreenWidth = screenWidth;
		}

		#endregion rInitialisation





		#region rUpdate

		/// <summary>
		/// Update all effects
		/// </summary>
		/// <param name="gameTime">Frame time</param>
		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < mFXList.Count; i++)
			{
				FX fx = mFXList[i];

				if (fx.Finished())
				{
					mFXList.RemoveAt(i);
					i--;
				}
				else
				{
					fx.Update(gameTime);
				}
			}
		}

		#endregion rUpdate





		#region rDraw

		/// <summary>
		/// Draws all effects
		/// </summary>
		/// <param name="info">Info needed to draw</param>
		public void Draw(DrawInfo info)
		{
			foreach (FX fx in mFXList)
			{
				fx.Draw(info);
			}
		}

		#endregion rDraw





		#region rAddEffects

		/// <summary>
		/// Add animtion
		/// </summary>
		public void AddAnimator(Vector2 pos, Animator anim, DrawLayer drawLayer)
		{
			mFXList.Add(new AnimationFX(pos, anim, drawLayer));
		}



		/// <summary>
		/// Clear all effects
		/// </summary>
		public void Clear()
		{
			mFXList.Clear();
		}

		#endregion rAddEffects





		#region rUtility

		/// <summary>
		/// Get the size we can draw
		/// </summary>
		public Point GetDrawableSize()
		{
			return new Point(mScreenWidth, mScreenHeight);
		}

		#endregion rUtility
	}
}
