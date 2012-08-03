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
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile(filename))
                {
                    StreamWriter writer = new StreamWriter(rawStream);
                    //save game state
                    int stateInt = (int)game.GameState;
                    writer.WriteLine(stateInt.ToString());
                    /*
                    int levelDim = (int)game.GameLevel;
                    writer.WriteLine(levelDim.ToString());
                    string wallpaperColor = "Value";
                    writer.WriteLine(wallpaperColor);
                    int playerStartPosition
                    writer
                     * */
                    //close
                    writer.Close();
                }
            }

        }

        //Loading
        public void loadGame(Game1 game)
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(filename))
                {
                    try
                    {
                        using (IsolatedStorageFileStream rawStream = isf.OpenFile(filename, System.IO.FileMode.Open))
                        {
                            StreamReader reader = new StreamReader(rawStream);
                            //restore game state
                            int stateInt = int.Parse(reader.ReadLine());
                            game.GameState = (GameState)stateInt;
                            //close
                            reader.Close();
                        }
                    }
                    catch
                    {
                        //redirect to description screen
                        game.GameState = GameState.Description;
                    }

                    //remove file
                    isf.DeleteFile(filename);
                }
                else
                {
                    //redirect to description screen
                    game.GameState = GameState.Description;
                }
            }
        }

        # endregion
    }
}
