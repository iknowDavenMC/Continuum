using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace paranothing
{
    class Dialogues : Audible, Interactive
    {
        # region Attributes

        //Audible
        private Cue dialogCue;

        # endregion

        # region Constructor

        public Dialogues()
        {

        }

        # endregion

        # region Methods

        //Audible
        public Cue getCue()
        {
            return dialogCue;
        }

        public void setCue(Cue cue)
        {
            dialogCue = cue;
        }

        public void Play()
        {

        }

        //Interactive
        public void Interact(Boy player)
        {
            //player.state = Boy.BoyState.Dialog;
            //player.X = X + 25;
        }

        # endregion
    }
}
