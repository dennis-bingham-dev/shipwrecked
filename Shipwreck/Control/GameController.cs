﻿using Shipwreck.Model;
using Shipwreck.View;
using System;
using Shipwreck.Model.Game;

namespace Shipwreck.Control
{
    public static class GameController
    {

        public static void WinGame()
        {
            Shipwreck.CurrentGame.Status = GameStatus.Over;
            
            var view = new SimpleView();
            view.Message = "YOU WON!";
            view.Display();
        }
        public static void LoseGame()
        {
            Shipwreck.CurrentGame.Player.Die(); // TODO Do I actually need this method?
            Shipwreck.CurrentGame.Status = GameStatus.Over;

            var view = new SimpleView();
            view.Message = "You Died. Sucks to suck\n GAME OVER";
            view.Display();
        }

        public static void QuitGame()
        {
            Shipwreck.CurrentGame.Status = GameStatus.Over;
            var view = new SimpleView();
            view.Message = "GAME OVER";
            view.Display();
        }

        public static void AdvanceDays(int numDays, bool waiting = false)
        {
            for (var i = 0; i < numDays && Shipwreck.CurrentGame.Status == GameStatus.Playing; i++)
            {
                AdvanceDay(waiting);
            }
        }
        
        private static void WinGame(string message)
        {
            Shipwreck.CurrentGame.Status = GameStatus.Over;
            var view = new SimpleView();
            view.Message = message;
            view.Display();
        }

        private static void AdvanceDay(bool waiting = false)
        {
            var player = Shipwreck.CurrentGame.Player;
            var exp = Shipwreck.CurrentGame.GameSettings.Player.BaseExpPerDay;
            
            player.Hunger -= Shipwreck.CurrentGame.GameSettings.Player.HungerPerDay;
            if (player.Hunger < 0) player.Hunger = 0;
            
            // let the player know the next day has started
            Shipwreck.CurrentGame.Day.Number++;
            new ShowDayView().Display();

            
            /***************************
             * Game ending phenomenon
             ***************************/
            // ENHANCEMENT implement potential weather deaths
            
            // Hunger -> calc before new day starts
            if (Shipwreck.CurrentGame.Player.Hunger <= 0)
            {
                LoseGame();
                return;
            }
            
            // Rescue (if waiting)
            if (waiting && new Random().Next(Shipwreck.CurrentGame.GameSettings.WaitSuccessRate) == 1)
            {
                const string message = "\n You're Saved! What luck!" +
                                       "\n A gang of local poachers found you on their way back to town." +
                                       "\n Good thing they're not picky about how they earn a living...";
                WinGame(message);
            }
            
            
            /***************************
             * Other phenomenon
             ***************************/
            // Their fire burns
            FireController.Burn(Shipwreck.CurrentGame.Fire);
            exp = Shipwreck.CurrentGame.Fire.Status == FireStatus.Burning ? exp + Shipwreck.CurrentGame.GameSettings.Fire.FireExpBoost: exp;
            
            
            /**********************************
             * Gain Exp for living another day
             **********************************/
            player.GainExperience(exp);
        }
    }
}