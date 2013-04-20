using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace InputEngine.Input
{
    public class KeyBoardInput : Input
    {
        private Keys _key;
        public Keys Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
                InputHandler.Flush();
            }
        }

        public KeyBoardInput(Keys key)
        {
            _key = key;
        }
        public override bool Pressed()
        {
            return InputHandler.KeyPressed(Key);
        }

        public override bool Released()
        {
            return InputHandler.KeyReleased(Key);
        }

        public override bool Down()
        {
            return InputHandler.KeyDown(Key);
        }

    }
}
