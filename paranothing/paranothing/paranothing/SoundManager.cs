using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace paranothing
{
    class SoundManager
    {
        private static SoundManager instance;
        private SoundBank soundBank;

        public static SoundManager getInstance()
        {
            if (instance == null)
                instance = new SoundManager();
            return instance;
        }

        private SoundManager() { }

        public void setSoundBank(ref SoundBank soundBank)
        {
            this.soundBank = soundBank;
        }

        public bool playSound(string soundName)
        {
            if (soundBank != null)
            {
                try
                {
                    Cue cue = soundBank.GetCue(soundName);
                    cue.Play();
                    return true;
                }
                catch (ArgumentException) { return false; }
            }
            return false;
        }

        public Cue playSound(string soundName, byte changeSignature)
        {
            if (soundBank != null)
            {
                try
                {
                    Cue cue = soundBank.GetCue(soundName);
                    cue.Play();
                    return cue;
                }
                catch (ArgumentException) { return null; }
            }

            return null;
        }
    }
}
