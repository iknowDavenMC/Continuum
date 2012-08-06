using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace paranothing
{
    class Level
    {
        public int Width, Height; // Complete width and height of the level
        public int playerX, playerY; // Player's starting position
        public Color wallpaperColor;
        private List<Saveable> savedObjs;
        public Level()
        {
            Width = 640;
            Height = 360;
            playerX = 38;
            playerY = 58;
            savedObjs = new List<Saveable>();
            wallpaperColor = Color.White;
        }

        public Level(int Width, int Height, int playerX, int playerY, Color wallpaperColor)
        {
            this.Width = Width;
            this.Height = Height;
            this.playerX = playerX;
            this.playerY = playerY;
            this.wallpaperColor = wallpaperColor;
            savedObjs = new List<Saveable>();
        }

        public void addObj(Saveable obj)
        {
            savedObjs.Add(obj);
        }

        public void removeObj(Saveable obj)
        {
            savedObjs.Remove(obj);
        }

        public List<Saveable> getObjs()
        {
            return savedObjs;
        }

        public string getSaveString()
        {
            string saveString = "StartLevel";
            saveString += "\nplayerX:" + playerX;
            saveString += "\nplayerY:" + playerY;
            saveString += "\nwidth:" + Width;
            saveString += "\nheight:" + Height;
            saveString += "\ncolor:" + wallpaperColor.R+","+wallpaperColor.G+","+wallpaperColor.B;
            foreach (Saveable obj in savedObjs)
            {
                saveString += "\n" + obj.saveData();
            }
            saveString += "\nEndLevel";
            return saveString;
        }

        public void loadFromFile(string filename)
        {
            try
            {
                Stream stream = TitleContainer.OpenStream(filename);
                StreamReader reader = new StreamReader(stream);
                createFromString(reader.ReadToEnd());
            }
            catch (System.IO.FileNotFoundException)
            {
            }
        }

        public void createFromString(string saveString)
        {
            Width = 640;
            Height = 360;
            playerX = 38;
            playerY = 58;
            savedObjs = new List<Saveable>();

            string[] saveLines = saveString.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);
            int lineNum = 0;
            string line = "";
            while (!line.StartsWith("StartLevel") && lineNum < saveLines.Length)
            {
                line = saveLines[lineNum];
                lineNum++;
            }
            while (!line.StartsWith("EndLevel") && lineNum < saveLines.Length)
            {
                line = saveLines[lineNum];
                // Level attributes
                if (line.StartsWith("playerX:"))
                    playerX = int.Parse(line.Substring(8));
                if (line.StartsWith("playerY:"))
                    playerY = int.Parse(line.Substring(8));
                if (line.StartsWith("width:"))
                    Width = int.Parse(line.Substring(6));
                if (line.StartsWith("height:"))
                    Height = int.Parse(line.Substring(7));
                if (line.StartsWith("color:"))
                    wallpaperColor = parseColor(line.Substring(6));
                // Shadow
                // Stairs
                if (line.StartsWith("StartStair"))
                    addObj((Saveable)createStair(saveLines, ref lineNum));
                // Rubble
                if (line.StartsWith("StartRubble"))
                    addObj((Saveable)createRubble(saveLines, ref lineNum));
                // Chair
                // Door
                // Wardrobe
                if (line.StartsWith("StartWardrobe"))
                    addObj((Saveable)createWardrobe(saveLines, ref lineNum));
                // Key
                // Portrait
                if (line.StartsWith("StartPortrait"))
                    addObj((Saveable)createPortrait(saveLines, ref lineNum));
                // Older Painting
                // Moved Painting
                // Bookcase
                // Button
                // Wall
                if (line.StartsWith("StartWall"))
                    addObj((Saveable)createWall(saveLines,ref lineNum));
                // Floor
                if (line.StartsWith("StartFloor"))
                    addObj((Saveable)createFloor(saveLines, ref lineNum));
                lineNum++;
            }
        }

        private Color parseColor(string color)
        {
            string[] rgb = color.Split(new char[] { ',' });
            int r = int.Parse(rgb[0]);
            int g = int.Parse(rgb[1]);
            int b = int.Parse(rgb[2]);

            return new Color(r, g, b);
        }

        private Wall createWall(string[] lines, ref int startLine)
        {
            int X = 0, Y = 0, Width = 0, Height = 0;
            bool intact = true;
            int lineNum = startLine;
            string line = "";
            while (!line.StartsWith("EndWall") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("width:"))
                {
                    try { Width = int.Parse(line.Substring(6)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("height:"))
                {
                    try { Height = int.Parse(line.Substring(7)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("intact:"))
                {
                    try { intact = bool.Parse(line.Substring(7)); }
                    catch (FormatException) { }
                }
                lineNum++;
            }
            startLine = --lineNum;
            return new Wall(X, Y, Width, Height, intact);
        }

        private Floor createFloor(string[] lines, ref int startLine)
        {
            int X = 0, Y = 0, Width = 0, Height = 0;
            int lineNum = startLine;
            string line = "";
            while (!line.StartsWith("EndFloor") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("width:"))
                {
                    try { Width = int.Parse(line.Substring(6)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("height:"))
                {
                    try { Height = int.Parse(line.Substring(7)); }
                    catch (FormatException) { }
                }
                lineNum++;
            }
            startLine = --lineNum;
            return new Floor(X, Y, Width, Height);
        }

        private Stairs createStair(string[] lines, ref int startLine)
        {
            int X = 0, Y = 0;
            Direction direction = Direction.Left;
            bool intact = true;
            int lineNum = startLine;
            string line = "";
            while (!line.StartsWith("EndStair") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("direction:"))
                {
                    string dir = line.Substring(10).Trim();
                    if (dir == "Right")
                        direction = Direction.Right;
                    else
                        direction = Direction.Left;
                }
                if (line.StartsWith("intact:"))
                {
                    try { intact = bool.Parse(line.Substring(7)); }
                    catch (FormatException) { }
                }
                lineNum++;
            }
            startLine = --lineNum;
            return new Stairs(X, Y, direction, intact);
        }

        private Wardrobe createWardrobe(string[] lines, ref int startLine)
        {
            int X = 0, Y = 0;
            bool locked = false;
            string name = "WR";
            string link = "WR";
            int lineNum = startLine;
            string line = "";
            while (!line.StartsWith("EndWardrobe") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("name:"))
                {
                    name = line.Substring(5).Trim();
                }
                if (line.StartsWith("locked:"))
                {
                    try { locked = bool.Parse(line.Substring(7)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("link:"))
                {
                    link = line.Substring(5).Trim();
                }
                lineNum++;
            }
            startLine = --lineNum;
            Wardrobe w = new Wardrobe(X, Y, name, locked);
            w.setLinkedWR(link);
            return w;
        }

        private Portrait createPortrait(string[] lines, ref int startLine)
        {
            int X = 0, Y = 0;
            int lineNum = startLine;
            string line = "";
            while (!line.StartsWith("EndPortrait") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                lineNum++;
            }
            startLine = --lineNum;
            return new Portrait(X, Y);
        }

        private Rubble createRubble(string[] lines, ref int startLine)
        {
            int X = 0, Y = 0;
            int lineNum = startLine;
            string line = "";
            while (!line.StartsWith("EndRubble") && lineNum < lines.Length)
            {
                line = lines[lineNum];
                if (line.StartsWith("x:"))
                {
                    try { X = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                if (line.StartsWith("y:"))
                {
                    try { Y = int.Parse(line.Substring(2)); }
                    catch (FormatException) { }
                }
                lineNum++;
            }
            startLine = --lineNum;
            return new Rubble(X, Y);
        }
    }
}
