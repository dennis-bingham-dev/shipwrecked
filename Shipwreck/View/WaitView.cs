﻿using Shipwreck.Control;
using System;

namespace Shipwreck.View
{
    class WaitView : View
    {
        public WaitView()
        {
            InGameView = true;
            Message = "How many days would you like to wait for?";
        }
        
        protected override bool HandleInput(string input)
        {
            var closeView = false;
            
            if (int.TryParse(input, out var numDays))
            {
                GameController.Wait(numDays);
                closeView = true;
            }
            else
            {
                Console.WriteLine("Input must be a number");
            }

            return closeView;
        }
    }
}
