using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace paranothing
{
    class GameController
    {
        public KeyboardState keyState;

        private List<Updatable> updatableObjs;
        private List<Drawable> drawableObjs;
        public Boy player;
        public enum GameState { Game };
        public enum Direction { Left, Right, Up, Down }
        public GameState state;

        public GameController(Boy player)
        {
            this.player = player;
            updatableObjs = new List<Updatable>();
            drawableObjs = new List<Drawable>();
            updatableObjs.Add(player);
            state = GameState.Game;
        }

        public void updateObjs(GameTime time)
        {
            keyState = Keyboard.GetState();
            foreach (Updatable obj in updatableObjs)
            {
                obj.update(time, this);
            }
        }

        public void drawObjs(SpriteBatch renderer)
        {
            foreach (Drawable obj in drawableObjs)
            {
                obj.draw(renderer);
            }
            player.draw(renderer);
        }

    }
}
