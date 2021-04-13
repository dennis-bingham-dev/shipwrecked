﻿using System;
using System.Collections.Generic;
using Shipwreck.Model.Views;

namespace Shipwreck.View
{
    class HelpMenuView : Model.Views.View
    {
        public override bool InGameView { get; set; }
        protected override string Message => "\n"
                                             + "\n----------------------------------"
                                             + "\n| Help Menu"
                                             + "\n----------------------------------"
                                             + "\n P - Purpose of the Game"
                                             + "\n M - Map Legend"
                                             // + "\n R - Resource Help"
                                             // + "\n C - Combat Help"
                                             + "\n X - Exit Help Menu"
                                             + "\n----------------------------------";

        protected override bool HandleInput(string menuOption)
        {
            var closeView = false;
            switch (menuOption) {
                case "P":
                    PurposeHelp();
                    Continue();
                    break;
                case "M":
                    MapHelp();
                    Continue();
                    break;
                // case "R":
                //     ResourceHelp();
                //     break;
                // case "C":
                //     CombatHelp();
                //     break;
            }

            return closeView;
        }

        private void PurposeHelp()
        {
            Console.WriteLine("\n***************************************************************************"
                + "\n The purpose of the game is to survive however you can. You're stuck on a"
                + "\n tropical Island, so you can either try and escape on your own by building"
                + "\n a raft & floating to safety, or by building a large signal fire on"
                + "\n the beach in an attempt to attract help. Maybe your best bet is to simply "
                + "\n wait patiently till someone comes to find you. I mean, after such a big"
                + "\n ship went down SOMEONE's bound to come looking for survivors, Right?"
                + "\n***************************************************************************");
        }

        private void MapHelp()
        {
            Console.WriteLine("\n**************************** MAP LEGEND ***********************************"
                + "\n ⛺️ - Base Camp"
                + "\n\t Days to Travel: 0"
                + "\n\n ⛰ - Mountain"
                + "\n\t Days to Travel: 3"
                + "\n\n 🏔 - Snow-Capped Mountain"
                + "\n\t Days to Travel: -"
                + "\n\n 🌳 - Forest"
                + "\n\t Days to Travel: 2"
                + "\n\n 🏝 - Beach"
                + "\n\t Days to Travel: 1"
                + "\n\n 🌾 - Plains"
                + "\n\t Days to Travel: 1"
                + "\n***************************************************************************");
        }

        private void ResourceHelp()
        {
            throw new NotImplementedException();
        }

        private void CombatHelp()
        {
            throw new NotImplementedException();
        }
    }
}
