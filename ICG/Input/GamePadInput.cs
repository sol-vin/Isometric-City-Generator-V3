using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace InputEngine.Input
{
    public class GamePadInput : Input
    {
        private Buttons _button;
        public Buttons Button
        {
            get
            {
                return _button;
            }

            set
            {
                _button = value;
                InputHandler.Flush();
            }
        } 

        private PlayerIndex _pi;
        public PlayerIndex PlayerIndex
        {
            get
            {
                return _pi;
            }
        } //Read-Only
        
        public GamePadInput(Buttons button, PlayerIndex pi)
        {
            _button = button;
            _pi = pi;
        }

        public override bool Pressed()
        {
            return InputHandler.ButtonPressed(Button, PlayerIndex);
        }

        public override bool Released()
        {
            return InputHandler.ButtonReleased(Button, PlayerIndex);
        }

        public override bool Down()
        {
            return InputHandler.ButtonDown(Button, PlayerIndex);
        }
    }
}
