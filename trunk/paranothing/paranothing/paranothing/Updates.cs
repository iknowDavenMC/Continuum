using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace paranothing
{
    interface Updates
    {
        void update(GameTime time, GameController control);
    }
}
