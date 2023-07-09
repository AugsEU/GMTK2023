using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMTK2023.UI
{
    internal class ScreenTransitionButton : Button
    {
        ScreenType mScreen;

        public ScreenTransitionButton(Vector2 topLeft, string text, ScreenType screen) : base(topLeft, text)
        {
            mScreen = screen;
        }

        protected override void DoAction()
        {
            ScreenManager.I.ActivateScreen(mScreen);
        }
    }
}
