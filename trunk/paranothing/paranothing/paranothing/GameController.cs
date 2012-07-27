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
        private List<Collideable> collideableObjs;
        public Boy player;
        public enum GameState { SplashScreen, HelpScreen, Game };
        public enum Direction { Left, Right, Up, Down }
        public GameState state;

        public GameController(Boy player)
        {
            this.player = player;
            updatableObjs = new List<Updatable>();
            drawableObjs = new List<Drawable>();
            collideableObjs = new List<Collideable>();
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
            if (player.X + player.Width > 400)
                player.X = 400 - player.Width;
            if (player.X < 0)
                player.X = 0;
            foreach (Collideable obj in collideableObjs)
            {
                Boy.BoyState currState = player.state;
                if (obj is Stairs)
                {
                    Stairs stair = (Stairs) obj;
                    if (stair.isSolid() && collides(stair.getBounds(), player.getBounds()))
                    {
                        player.state = Boy.BoyState.Stairs;
                    }
                    else
                    {
                        player.state = Boy.BoyState.Walk;
                    }
                }
            }
        }

        public void drawObjs(SpriteBatch renderer)
        {
            Color tint = Color.White;
            foreach (Drawable obj in drawableObjs)
            {
                obj.draw(renderer, tint);
            }

            player.draw(renderer, tint);
        }

        public bool collides(Rectangle box1, Rectangle box2)
        {
            return Rectangle.Intersect(box1, box2).Width != 0;
        }

        public void addObject(Object obj)
        {
            if (obj is Drawable)
            {
                drawableObjs.Add((Drawable)obj);
            }
            if (obj is Updatable)
            {
                updatableObjs.Add((Updatable)obj);
            }
            if (obj is Collideable)
            {
                collideableObjs.Add((Collideable)obj);
            }

        }

    }
}
