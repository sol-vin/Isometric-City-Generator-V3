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
    public class Input
    {
        public int HoldTime;

        public virtual bool Released()
        {
            return false;
        }


        public virtual bool Pressed()
        {
            return false;
        }


        public virtual bool Down()
        {
            return false;
        }
    }
}
