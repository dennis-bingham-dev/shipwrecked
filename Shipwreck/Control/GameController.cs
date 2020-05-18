﻿using Shipwreck.Model;
using Shipwreck.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shipwreck.Control
{
    class GameController
    {
        public static void CreateNewGame(string characterName)
        {
            Player player = new Player(characterName, 5);
            InventoryController.AddDefaultItemsToInventory(player.Inventory);
            Day day = new Day();

            Shipwreck.CurrentGame = new Game(player, day);
            Shipwreck.NewDayView.Display();
        }

        public static void Wait(int numDays)
        {
            for (int i = 0; i < numDays && Shipwreck.CurrentGame != null; i++)
            {
                AdvanceDay(true);
            }
        }

        public static void EndGame()
        {
            Shipwreck.CurrentGame.Player.Die();
            Shipwreck.CurrentGame = null;

            GameOverView gameOverView = new GameOverView();
            gameOverView.Display();
        }

        private static void AdvanceDay(bool waiting = false)
        {
            Player player = Shipwreck.CurrentGame?.Player;
            int exp = 25;

            // They get hungry
            player.Hunger += Day.HungerPerDay; // this probably ought to be a value somewhere
            if (Shipwreck.CurrentGame.Player.Hunger > 20) // also ought to be a value somewhere...
            {
                EndGame();
            }

            // Their fire burns
            FireController.Burn(Shipwreck.CurrentGame?.Fire);
            exp = Shipwreck.CurrentGame?.Fire.IsBurning == true ? exp + 15: exp;


            // let the player know the next day has started
            Shipwreck.CurrentGame?.Day.IncrementDay();
            // gain EXP
            // TODO EXP should be higher if a fire is burning
            player.GainExperience(exp);
            Shipwreck.NewDayView.Display();

            // TODO implement potential weather deaths

            Random random = new Random();
            if (waiting && random.Next(100000) == 1)
            {
                string message = "\n You're Saved! What luck!" +
                    "\n A gang of local poachers found you on their way back to town." +
                    "\n Good thing their not picky about how they earn a living...";
                GameOverView gameOverView = new GameOverView(message);
                gameOverView.Display();
            }

        }
    }
}