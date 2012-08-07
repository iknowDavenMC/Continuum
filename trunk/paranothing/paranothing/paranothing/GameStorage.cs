using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;

namespace paranothing
{
    /// <summary>
    /// Class to save and load file : need a valid user's profile to work on xbox
    /// </summary>
    class GameStorage
    {
        # region Attribute

        private string filename;

        # endregion

        # region Constructor

        public GameStorage(string name)
        {
            filename = name;
        }

        # endregion

        # region Methods

        //Saving
        public void saveGame(Game1 game)
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile(filename))
                {
                    StreamWriter writer = new StreamWriter(rawStream);
                    //save game state
                    int stateInt = (int)game.GameState;
                    writer.WriteLine(stateInt.ToString());
                    /*
                     * //level
                    int levelDim = (int)game.GameLevel;
                    writer.WriteLine(levelDim.ToString());
                    string wallpaperColor = "Value";
                    writer.WriteLine(wallpaperColor);
                     * //player
                    int playerStartX = player.getX();
                    writer.WriteLine(playerStartX.ToString());
                    int playerStartY = player.getY();
                    writer.WriteLine(playerStartY.ToString());
                     * //shadows
                    int shadowsStartX = shadows.getX();
                    writer.WriteLine(shadowsStartX.ToString());
                    int shadowsStartY = shadows.getY();
                    writer.WriteLine(shadowsStartY.ToString());
                    float shadowsPatrolDistance = shadows.PatrolDistance();
                    writer.WriteLine(shadowsPatrolDistance.ToString());
                     * //walls
                    int wallsPosX = walls.getX();
                    writer.WriteLine(wallsPosX.ToString());
                    int wallsPosY = walls.getY();
                    writer.WriteLine(wallsPosY.ToString());
                    int wallsLength = walls.Length();
                    writer.WriteLine(wallsLength.ToString());
                    bool wallsBroken = walls.isBroken();
                    writer.WriteLine(wallsBroken.ToString());
                     * //floors
                    int floorsPosX = floors.getX();
                    writer.WriteLine(floorsPosX.ToString());
                    int floorsPosY = floors.getY();
                    writer.WriteLine(floorsPosY.ToString());
                    int floorsLength = floors.Length();
                    writer.WriteLine(floorsLength.ToString());
                     * //stairs
                    int stairsPosX = stairs.getX();
                    writer.WriteLine(stairsPosX.ToString());
                    int stairsPosY = stairs.getY();
                    writer.WriteLine(stairsPosY.ToString());
                    string stairsDir = stairs.Direction();
                    writer.WriteLine(stairsDir);
                    bool stairsBroken = stairs.isBroken();
                    writer.WriteLine(stairsBroken.ToString());
                     * //rubble
                    int rubblePosX = rubble.getX();
                    writer.WriteLine(rubblePosX.ToString());
                    int rubblePosY = rubble.getY();
                    writer.WriteLine(rubblePosY.ToString());
                     * //chairs
                    int chairsPosX = chairs.getX();
                    writer.WriteLine(chairsPosX.ToString());
                    int chairsPosY = chairs.getY();
                    writer.WriteLine(chairsPosY.ToString());
                     * //doors
                    int doorsPosX = doors.getX();
                    writer.WriteLine(doorsPosX.ToString());
                    int doorsPosY = doors.getY();
                    writer.WriteLine(doorsPosY.ToString());
                     * //wardrobes
                    int wardrobesPosX = wardrobes.getX();
                    writer.WriteLine(wardrobesPosX.ToString());
                    int wardrobesPosY = wardrobes.getY();
                    writer.WriteLine(wardrobesPosY.ToString());
                    bool wardrobesStartLocked = wardrobesStartLocked();
                    writer.WriteLine(wardrobesStartLocked.ToString());
                    bool wardrobesLinked = wardrobes.isLinked();
                    writer.WriteLine(wardrobesLinked.ToString());
                     * //keys
                    int keysPosX = keys.getX();
                    writer.WriteLine(keysPosX.ToString());
                    int keysPosY = keys.getY();
                    writer.WriteLine(keysPosY.ToString());
                    long keysTime = keys.TimePeriod();
                    writer.WriteLine(keysTime.ToString());
                    string keysCanUnlock = keys.CanUnlock();
                    writer.WriteLine(keysCanUnlock);
                     * //paintings
                    int paintingsPosX = paintings.getX();
                    writer.WriteLine(paintingsPosX.ToString());
                    int paintingsPosY = paintings.getY();
                    writer.WriteLine(paintingsPosY.ToString());
                     * //old paintings
                    int oldPaintingsPosX = oldPaintings.getX();
                    writer.WriteLine(oldPaintingsPosX.ToString());
                    int oldPaintingsPosY = oldPaintings.getY();
                    writer.WriteLine(oldPaintingsPosY.ToString());
                     * //moved paintings
                    int movedPaintingsPosPreX = movedPaintings.getPreX();
                    writer.WriteLine(movedPaintingsPosPreX.ToString());
                    int movedPaintingsPosPreY = movedPaintings.getPreY();
                    writer.WriteLine(movedPaintingsPosPreY.ToString());
                    int movedPaintingsPosPastX = movedPaintings.getPastX();
                    writer.WriteLine(movedPaintingsPosPastX.ToString());
                    int movedPaintingsPosPastY = movedPaintings.getPastY();
                    writer.WriteLine(movedPaintingsPosPastY.ToString());
                     * //bookcases
                    int bookcasesPosX = bookcases.getX();
                    writer.WriteLine(bookcasesPosX.ToString());
                    int bookcasesPosY = bookcases.getY();
                    writer.WriteLine(bookcasesPosY.ToString());
                    string bookcasesButA = bookcases.ButtonA();
                    writer.WriteLine(bookcasesButA);
                    string bookcasesButB = bookcases.ButtonB();
                    writer.WriteLine(bookcasesButB);
                     * //buttons
                    int buttonsPosX = buttons.getX();
                    writer.WriteLine(buttonsPosX.ToString());
                    int buttonsPosY = buttons.getY();
                    writer.WriteLine(buttonsPosY.ToString());
                    
                     * */
                    //close
                    writer.Close();
                }
            }

        }

        //Loading
        public void loadGame(Game1 game)
        {
#if WINDOWS_PHONE
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        using (IsolatedStorageFileStream rawStream = isf.OpenFile(filename, System.IO.FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(rawStream);
                            string file = reader.ReadToEnd();
                            //restore game state
                            //int stateInt = int.Parse(reader.ReadLine());
                            //game.GameState = (GameState)stateInt;
                            //close
                            reader.Close();
                        }
                    }
                    catch
                    {
                        //redirect to description screen
                        game.GameState = GameState.MainMenu;
                    }

                    //remove file
                    isf.DeleteFile(filename);
                }
                else
                {
                    //redirect to description screen
                    game.GameState = GameState.MainMenu;
                }
            }
        }

        # endregion
    }
}
