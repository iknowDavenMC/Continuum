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
        # region Attributes

        private string filename;
        Level level;

        # endregion

        # region Constructor

        public GameStorage(string name)
        {
            filename = name;
        }

        # endregion

        # region Methods

        //Saving
        public void saveGame(Level gameLevel)
        {
#if WINDOWS
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForDomain())
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
#endif
            {
                using (IsolatedStorageFileStream rawStream = isf.CreateFile(filename))
                {
                    StreamWriter writer = new StreamWriter(rawStream);

                    //save level
                    writer.WriteLine(gameLevel.getSaveString());
                    
                    //close
                    writer.Close();
                 }
            }

        }

        //Loading
        public Level loadGame(Game1 game)
        {
#if WINDOWS
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForDomain())
#else
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
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

                            //restore level
                            level = new Level();
                            level.createFromString(file);

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
                    return level;
                }
                else
                {
                    //redirect to description screen
                    game.GameState = GameState.MainMenu;
                }
                return null;
            }
        }

        # endregion
    }
}