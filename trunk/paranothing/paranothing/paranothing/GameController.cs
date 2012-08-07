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
        public Level level;
        public Camera camera;

        private static GameController instance;

        public static GameController getInstance()
        {
            if (null == instance)
            {
                instance = new GameController();
            }
            return instance;
        }

        protected GameController()
        {
            updatableObjs = new List<Updatable>();
            drawableObjs = new List<Drawable>();
            collideableObjs = new List<Collideable>();
            state = GameState.Game;
            timePeriod = TimePeriod.Present;
        }

        public void setPlayer(Boy player)
        {
            this.player = player;
            addObject(player);
            addObject(player.actionBubble);
        }

        public void setCamera(Camera camera)
        {
            this.camera = camera;
            addObject(camera);
        }

        public void initLevel(bool preserveTime)
        {
            updatableObjs = new List<Updatable>();
            drawableObjs = new List<Drawable>();
            collideableObjs = new List<Collideable>();
            player.X = level.playerX;
            player.Y = level.playerY;
            addObject(player);
            addObject(player.actionBubble);
            addObject(camera);
            foreach (Saveable obj in level.getObjs())
            {
                addObject(obj);
            }
            if (!preserveTime)
                timePeriod = TimePeriod.Present;
            Console.WriteLine(level.getSaveString());
        }

        public void updateObjs(GameTime time)
        {
            keyState = Keyboard.GetState();
            foreach (Updatable obj in updatableObjs)
            {
                obj.update(time);
            }
            player.actionBubble.hide();
            foreach (Collideable obj in collideableObjs)
            {
                Boy.BoyState currState = player.state;
                bool colliding = collides(((Collideable)obj).getBounds(), player.getBounds());
                if (obj is Stairs)
                {
                    Stairs stair = (Stairs)obj;
                    if (stair.isSolid())
                    {
                        if (colliding)
                        {
                            if (player.X + 30 >= stair.X && player.X + 8 <= stair.X)
                            {
                                if (((stair.direction == Direction.Left && player.Y + 58 == stair.getSmallBounds().Y)
                                    || (stair.direction == Direction.Right && player.Y + 58 == stair.Y + stair.getBounds().Height))
                                    && (player.state == Boy.BoyState.Idle || player.state == Boy.BoyState.Walk))
                                {
                                    player.actionBubble.setAction(ActionBubble.BubbleAction.Stair, false);
                                    player.interactor = (Interactive)obj;
                                    player.actionBubble.show();
                                }
                            }
                            else if (player.X + 30 >= stair.X + stair.getBounds().Width && player.X + 8 <= stair.X + stair.getBounds().Width)
                            {
                                if (((stair.direction == Direction.Right && player.Y + 58 == stair.getSmallBounds().Y)
                                    || (stair.direction == Direction.Left && player.Y + 58 == stair.Y + stair.getBounds().Height))
                                    && (player.state == Boy.BoyState.Idle || player.state == Boy.BoyState.Walk))
                                {
                                    player.actionBubble.setAction(ActionBubble.BubbleAction.Stair, false);
                                    player.interactor = (Interactive)obj;
                                    player.actionBubble.show();
                                }
                            }
                            if (player.state == Boy.BoyState.StairsLeft || player.state == Boy.BoyState.StairsRight)
                            {
                                if (stair.direction == Direction.Left)
                                {
                                    if ((player.direction == Direction.Left && (int)player.Y + 58 == stair.getSmallBounds().Y)
                                        || (player.direction == Direction.Right && (int)player.Y + 58 == stair.Y + stair.getBounds().Height))
                                    {
                                        player.state = Boy.BoyState.Walk;
                                    }
                                }
                                if (stair.direction == Direction.Right)
                                {
                                    if ((player.direction == Direction.Right && (int)player.Y + 58 == stair.getSmallBounds().Y)
                                        || (player.direction == Direction.Left && (int)player.Y + 58 == stair.Y + stair.getBounds().Height))
                                    {
                                        player.state = Boy.BoyState.Walk;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (obj is Wardrobe)
                {
                    Wardrobe wardrobe = (Wardrobe) obj;
                    if (colliding && player.X + (player.direction == Direction.Left ? 8 : 32) > wardrobe.X)
                    {
                        bool negated = false;
                        if (collides(wardrobe.enterBox, player.getBounds()))
                        {
                            Wardrobe linkedWR = wardrobe.getLinkedWR();
                            if (wardrobe.isLocked() || linkedWR == null 
                                || linkedWR.isLocked() || collidingWithSolid(linkedWR.enterBox))
                                negated = true;
                            if (player.state == Boy.BoyState.Idle || player.state == Boy.BoyState.Walk)
                            {
                                player.actionBubble.setAction(ActionBubble.BubbleAction.Wardrobe, negated);
                                player.actionBubble.show();
                                player.interactor = (Interactive)obj;
                            }
                        }
                        else
                        {
                            negated = collidingWithSolid(wardrobe.getBounds(), false);
                            if (player.state == Boy.BoyState.Idle || player.state == Boy.BoyState.Walk)
                            {
                                player.actionBubble.setAction(ActionBubble.BubbleAction.Push, negated);
                                player.actionBubble.show();
                                player.interactor = (Interactive)obj;
                            }
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
                    if (player.state != Boy.BoyState.StairsLeft && player.state != Boy.BoyState.StairsRight)
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
                    if (!player.actionBubble.isVisible() && !(player.interactor is Wardrobe))
                        player.interactor = null;
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
            if (timePeriod == TimePeriod.Past)
            {
                tint = Color.White;
                tint.A = 16;
            }
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

        public bool collidingWithSolid(Rectangle box)
        {
            return collidingWithSolid(box, true);
        }

        public bool collidingWithSolid(Rectangle box, bool includePlayer)
        {
            foreach (Collideable col in collideableObjs)
            {
                if (!includePlayer && col is Boy)
                    continue;
                if (col is Stairs)
                    continue;
                if (col.isSolid() && collides(box, col.getBounds()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
