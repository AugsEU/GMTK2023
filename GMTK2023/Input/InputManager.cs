namespace GMTK2023
{
    using InputBindingMap = Dictionary<GameKeys, InputBindSet>;
    using InputBindingPair = KeyValuePair<GameKeys, InputBindSet>;

    enum GameKeys
    {
        //Menu
        Confirm,
        Pause,

        //Arnold
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        FireGun,
    }

    /// <summary>
    /// Singleton that manages inputs.
    /// </summary>
    internal class InputManager : Singleton<InputManager>
    {
        #region rMembers

        InputBindingMap mInputBindings = new InputBindingMap();

        #endregion rMembers





        #region rInitialisation

        /// <summary>
        /// Init input manager
        /// </summary>
        public void Init()
        {
            //Set default bindings for now. TO DO: Load from file.
            SetDefaultBindings();
        }



        /// <summary>
        /// Set default bindings
        /// </summary>
        public void SetDefaultBindings()
        {
            mInputBindings.Add(GameKeys.Confirm, new InputBindSet(new KeyBinding(Keys.Enter)));
            mInputBindings.Add(GameKeys.Pause, new InputBindSet(new KeyBinding(Keys.Escape)));

            mInputBindings.Add(GameKeys.MoveLeft, new InputBindSet(new KeyBinding(Keys.A)));
            mInputBindings.Add(GameKeys.MoveRight, new InputBindSet(new KeyBinding(Keys.D)));
            mInputBindings.Add(GameKeys.MoveUp, new InputBindSet(new KeyBinding(Keys.W)));
            mInputBindings.Add(GameKeys.MoveDown, new InputBindSet(new KeyBinding(Keys.S)));
            mInputBindings.Add(GameKeys.FireGun, new InputBindSet(new MouseBtnBinding(MouseButton.Left)));

        }

        #endregion rInitialisation





        #region rKeySense

        /// <summary>
        /// Update Inputs
        /// </summary>
        /// <param name="gameTime">Frame time</param>
        public void Update(GameTime gameTime)
        {
            foreach (InputBindingPair keyBindPair in mInputBindings)
            {
                keyBindPair.Value.Update(gameTime);
            }
        }



        /// <summary>
        /// Was a button pressed recently?
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it was pressed in the last update</returns>
        public bool KeyPressed(GameKeys key)
        {
            return mInputBindings[key].AnyKeyPressed();
        }



        /// <summary>
        /// Was a button pressed recently?
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if it was pressed in the last update</returns>
        public bool KeyHeld(GameKeys key)
        {
            return mInputBindings[key].AnyKeyHeld();
        }
        #endregion rKeySense
    }
}
