﻿using System;
using Shipwreck.Control;
using Shipwreck.Helpers;
using Shipwreck.Model.Game;

namespace Shipwreck.View
{
    class MainMenuView : View
    {
        public MainMenuView()
        {
            InGameView = false;
            Message = "\n\n----------------------------------"
                      + "\n| Main Menu"
                      + "\n----------------------------------"
                      + "\n N - New Game"
                      + "\n L - Load Game (COMING SOON)"
                      + "\n H - Help Menu"
                      + "\n X - Close Shipwreck"
                      + "\n----------------------------------";
        }

        protected override bool HandleInput(string input)
        {
            switch (input) 
            {
                case "N":
                    StartNewGame();
                    break;
                case "L":
                    LoadGame();
                    break;
                case "H":
                    OpenHelpView();
                    break;
            }

            return false;
        }

        private void StartNewGame()
        {
            // TODO de-mysitify game creation
            Shipwreck.CurrentGame = new Game();
            new NewGameView().Display();
        }

        private void LoadGame()
        {
            // list available save files
            var existingFiles = FileHelper.GetFilesInDir(Shipwreck.Settings.SavePath);
            if (existingFiles.Count == 0)
            {
                Console.WriteLine("There are no save files");
                return;
            }
            
            Console.WriteLine("Existing Save files:");
            foreach (var existingFile in existingFiles)
            {
                Console.WriteLine(existingFile);
            }
            
            // get desired save file
            Console.WriteLine("\nWhich file would you like to load?");
            var fileToLoad = Console.ReadLine() ?? "";
            if (fileToLoad == "x" || fileToLoad.ToUpper() == "X") return;
            fileToLoad = FileHelper.AddExtension(fileToLoad, ".json");

            if (!existingFiles.Contains(fileToLoad))
            {
                Console.WriteLine("That file does not exist");
                return;
            }
            
            // try load file
            if (ShipwreckController.TryLoadGame(fileToLoad))
            {
               GameController.StartGame();
            }
            else
            {
                Console.WriteLine($"Unable to load {fileToLoad}");
            }
        }

        private void OpenHelpView()
        {
            new HelpMenuView().Display();
        }
    }
}
