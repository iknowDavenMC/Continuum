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
        public GameState state;
        public TimePeriod timePeriod;

        public GameController(Boy player)
        {
            this.player = player;
            updatableObjs = new List<Updatable>();
            drawableObjs = new List<Drawable>();
            collideableObjs = new List<Collideable>();
            updatableObjs.Add(player);
            state = GameState.Title;
            timePeriod = TimePeriod.Present;
        }

        public void updateObjs(GameTime time)
        {
            keyState = Keyboard.GetState();
            foreach (Updatable obj in updatableObjs)
            {
                obj.update(time, this);
            }
            player.actionBubble.hide();
            player.interactor = null;
            foreach (Collideable obj in collideableObjs)
            {
                Boy.BoyState currState = player.state;
                bool colliding = collides(((Collideable)obj).getBounds(), player.getBounds());
                if (obj is Stairs)
                {
                    Stairs stair = (Stairs) obj;
                    if (stair.isSolid())
                    {
                        if (stair.direction == Direction.Left)
                        {
                            if (colliding && player.X > stair.X - 14 && player.Y >= (player.X - stair.X) * 2 / 3 + stair.X)
                            {
                                player.state = Boy.BoyState.StairsLeft;
                            }
                            else if (player.state == Boy.BoyState.StairsLeft)
                            {
                                player.state = Boy.BoyState.Walk;
                            }
                        }
                        else
                        {
                            if (colliding && player.X <= stair.X + stair.getBounds().Width)
                            {
                                player.state = Boy.BoyState.StairsLeft;
                            }
                            else if (player.state == Boy.BoyState.StairsRight)
                            {
                                player.state = Boy.BoyState.Walk;
                            }
                        }
                    }
                }
                else if (obj is Wardrobe)
                {
                    Wardrobe wardrobe = (Wardrobe) obj;
                    if (colliding && player.X + 8 > wardrobe.X)
                    {
                        bool negated;
                        if (wardrobe.isLocked() || wardrobe.getLinkedWR() == null || wardrobe.getLinkedWR().isLocked())
                            negated = true;
                        else
                            negated = false;
                        if (player.state == Boy.BoyState.Idle || player.state == Boy.BoyState.Walk)
                        {
                            player.actionBubble.setAction(ActionBubble.BubbleAction.Wardrobe, negated);
                            player.actionBubble.show();
                            player.interactor = (Interactive)obj;
                        }
                    }
                }
                else if (obj is Portrait)
                {
                    Portrait painting = (Portrait)obj;
                    if (colliding && player.X + player.Width - 10 > painting.X && (player.state == Boy.BoyState.Idle || player.state == Boy.BoyState.Walk))
                    {
                        bool negated = false;
                        player.actionBubble.setAction(ActionBubble.BubbleAction.Portrait, negated);
                        player.actionBubble.show();
                        player.interactor = (Interactive)obj;
                    }
                }
                else if (obj is Floor)
                {
                    if (player.state == Boy.BoyState.Walk || player.state == Boy.BoyState.Walk)
                    {
                        Floor floor = (Floor)obj;
                        while (collides(player.getBounds(), floor.getBounds()))
                        {
                            player.Y--;
                        }
                    }
                }
                else
                {
                    Collideable collider = (Collideable)obj;
                    if (colliding && player.state == Boy.BoyState.Walk && collider.isSolid())
                    {
                        if ((player.direction == Direction.Left && player.X > collider.getBounds().X)
                            || (player.direction == Direction.Right && player.X < collider.getBounds().X))
                        player.state = Boy.BoyState.PushingStill;
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
