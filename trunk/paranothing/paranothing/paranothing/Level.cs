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
                string objData = "";
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
                {
                    objData = line;
                    while (!line.StartsWith("EndStair") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Stairs(objData));
                }
                // Rubble
                if (line.StartsWith("StartRubble"))
                {
                    objData = line;
                    while (!line.StartsWith("EndRubble") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Rubble(objData));
                }
                // Chair
                // Door
                if (line.StartsWith("StartDoor"))
                {
                    objData = line;
                    while (!line.StartsWith("EndDoor") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Doors(objData));
                }
                // Wardrobe
                if (line.StartsWith("StartWardrobe"))
                {
                    objData = line;
                    while (!line.StartsWith("EndWardrobe") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Wardrobe(objData));
                }
                // Key
                if (line.StartsWith("StartKey"))
                {
                    objData = line;
                    while (!line.StartsWith("EndKey") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new DoorKeys(objData));
                }
                // Portrait
                if (line.StartsWith("StartPortrait"))
                {
                    objData = line;
                    while (!line.StartsWith("EndPortrait") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Portrait(objData));
                }
                // Older Painting
                // Moved Painting
                // Bookcase
                // Button
                // Wall
                if (line.StartsWith("StartWall"))
                {
                    objData = line;
                    while (!line.StartsWith("EndWall") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Wall(objData)); 
                }
                // Floor
                if (line.StartsWith("StartFloor"))
                {
                    objData = line;
                    while (!line.StartsWith("EndFloor") && lineNum < saveLines.Length)
                    {
                        line = saveLines[lineNum];
                        objData += "\n" + line;
                        lineNum++;
                    }
                    addObj(new Floor(objData));
                }
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
    }
}
