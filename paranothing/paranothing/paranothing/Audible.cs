using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace paranothing
{
    interface Audible
    {
        void Play();
        void setCue(Cue cue);
        Cue getCue();
    }
}
