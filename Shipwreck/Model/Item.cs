﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shipwreck.Model
{
    class Item
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Quantity { get; set; } // Inventory should handle quantity
        // private bool Droppable { get; set; }

        public Item(string name, string description, int quantity = 1)
        {
            Name = name;
            Quantity = quantity;
            Description = description;
        }
    }
}
