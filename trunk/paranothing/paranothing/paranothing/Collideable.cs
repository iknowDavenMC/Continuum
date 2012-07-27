using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace paranothing
{
    interface Collideable
    {
        Rectangle getBounds();
        bool isSolid();
    }
}
