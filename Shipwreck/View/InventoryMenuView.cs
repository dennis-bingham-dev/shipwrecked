﻿using Shipwreck.Control;
using Shipwreck.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharprompt;
using Shipwreck.Helpers;
using Shipwreck.Model.Views;

namespace Shipwreck.View
{
    class InventoryMenuView : MenuView
    {
        public override bool InGameView => true;
        protected override string Title => "Inventory Menu";
        protected override List<MenuItem> MenuItems => new List<MenuItem>
        {
            new MenuItem
            {
                DisplayName = "View Food",
                Type = MenuItemType.ViewFood
            },
            new MenuItem
            {
                DisplayName = "Eat Food",
                Type = MenuItemType.EatFood
            },
            new MenuItem
            {
                DisplayName = "Drop Item",
                Type = MenuItemType.DropItem
            },
            new MenuItem
            {
                DisplayName = "View Character",
                Type = MenuItemType.ViewCharacter
            },
            new MenuItem
            {
                DisplayName = "Close Inventory",
                Type = MenuItemType.Close
            },
        };

        protected override bool HandleInput(MenuItem menuItem)
        {
            const bool done = false;

            switch(menuItem.Type)
            {
                // case "A":
                //     ShowAllItems();
                //     ViewHelpers.Continue();
                //     break;
                // case "G":
                //     ViewGear();
                //     ViewHelpers.Continue();
                //     break;
                case MenuItemType.ViewFood:
                    ViewFood();
                    ViewHelpers.Continue();
                    break;
                // case "R":
                //     ViewResources();
                //     ViewHelpers.Continue();
                //     break;
                case MenuItemType.EatFood:
                    ViewFood();
                    Console.WriteLine();
                    EatFood();
                    ViewHelpers.Continue();
                    break;
                // case "Q":
                //     EquipGear();
                //     ViewHelpers.Continue();
                //     break;
                case MenuItemType.DropItem:
                    DropItem();
                    ViewHelpers.Continue();
                    break;
                case MenuItemType.ViewCharacter:
                    GameMenuView.ShowPlayerStats();
                    ViewHelpers.Continue();
                    break;
            }
            return done;
        }

        private void ShowAllItems()
        {
            StringBuilder line;
            var inventory = Shipwreck.CurrentGame.Player.Inventory;

            Console.WriteLine("\n-------------------------\n Inventory:\n-------------------------");
            Console.WriteLine($" Active Weapon: {inventory.ActiveWeapon?.Name ?? "None"} +{inventory.ActiveWeapon?.AttackPower ?? 0}");
            Console.WriteLine($" Active Armor:  {inventory.ActiveArmor?.Name ?? "None"} +{inventory.ActiveArmor?.DefensePower ?? 0}");
            Console.WriteLine("");
            
            line = new StringBuilder("                                              ");
            line.Insert(1, "ITEM");
            line.Insert(16, "QTY");
            line.Insert(20, "DESC");
            Console.WriteLine(line);

            foreach (var itemRecord in inventory.Items)
            {
                line = new StringBuilder("                                              ");
                line.Insert(1, itemRecord.InventoryItem.Name);
                line.Insert(16, itemRecord.Quantity);
                line.Insert(20, itemRecord.InventoryItem.Description);
                Console.WriteLine(line);
            }
            Console.WriteLine("-------------------------");
        }

        private void ViewGear()
        {
            var inventory = Shipwreck.CurrentGame.Player.Inventory;
            var weaponItems = InventoryController.GetItemsByType<Weapon>(inventory);
            var armorItems = InventoryController.GetItemsByType<Armor>(inventory);
            var gearItems = weaponItems.Concat(armorItems).ToList();
            
            Console.WriteLine("\n-------------------------\n Gear:\n-------------------------");

            var line = new StringBuilder("                                              ");
            line.Insert(1, "T");
            line.Insert(4, "ITEM");
            line.Insert(19, "QTY");
            line.Insert(23, "BONUS");
            Console.WriteLine(line);

            foreach (var gear in gearItems)
            {
                // Type itemType = gear.InventoryItem.GetType();
                var itemBonus = 0;
                var isActive = false;

                if (gear.InventoryItem.GetType() == typeof(Armor))
                {
                    itemBonus = ((Armor)gear.InventoryItem).DefensePower;
                    isActive = inventory.ActiveArmor.Name == gear.InventoryItem.Name;
                }
                else if (gear.InventoryItem.GetType().IsSubclassOf(typeof(Weapon)))
                {
                    itemBonus = ((Weapon)gear.InventoryItem).AttackPower;
                    isActive = inventory.ActiveWeapon.Name == gear.InventoryItem.Name;
                }

                line = new StringBuilder("                                              ");
                line.Insert(1, gear.InventoryItem.GetType() == typeof(Armor) ? "A" : "W");
                line.Insert(3, isActive ? "*" : "");
                line.Insert(4, gear.InventoryItem.Name);
                line.Insert(19, gear.Quantity);
                line.Insert(23, $"+{itemBonus}");
                Console.WriteLine(line);
            }
        }

        private void ViewFood()
        {
            StringBuilder line;
            var inventory = Shipwreck.CurrentGame?.Player.Inventory;
            var foodItems = InventoryController.GetItemsByType<Food>(inventory);

            Console.WriteLine("\n-------------------------\n Food:\n-------------------------");

            line = new StringBuilder("                                              ");
            line.Insert(3, "ITEM");
            line.Insert(18, "QTY");
            // line.Insert(20, "HLTH");
            line.Insert(22, "HNGR"); // 25 once health is added
            Console.WriteLine(line);

            var count = 1;
            foreach (var foodItem in foodItems)
            {
                line = new StringBuilder("                                              ");
                line.Insert(0, $"{count}.");
                line.Insert(3, foodItem.InventoryItem.Name);
                line.Insert(18, foodItem.Quantity);
                // line.Insert(20, ((Food)foodItem.InventoryItem).HealingPower);
                line.Insert(22, ((Food)foodItem.InventoryItem).FillingPower); // 25 once health is added
                Console.WriteLine(line);

                count++;
            }
        }

        // private void ViewResources()
        // {
        //     StringBuilder line;
        //     var inventory = Shipwreck.CurrentGame?.Player.Inventory;
        //     var resources = InventoryController.GetItemsByType<Resource>(inventory);
        //
        //     Console.WriteLine("\n-------------------------\n Resources:\n-------------------------");
        //
        //     line = new StringBuilder("                                              ");
        //     line.Insert(1, "ITEM");
        //     line.Insert(16, "QTY");
        //     line.Insert(20, "DESC");
        //     Console.WriteLine(line);
        //
        //     foreach (var resource in resources)
        //     {
        //         line = new StringBuilder("                                              ");
        //         line.Insert(1, resource.InventoryItem.Name);
        //         line.Insert(16, resource.Quantity);
        //         line.Insert(20, (resource.InventoryItem).Description);
        //         Console.WriteLine(line);
        //     }
        // }

        private void EatFood()
        { 
            var player = Shipwreck.CurrentGame.Player;
            var inventory = player.Inventory;
            var foodInInventory = InventoryController.GetItemsByType<Food>(inventory);

            var promptItems = foodInInventory.Select(itemRecord => itemRecord.InventoryItem.Name).ToList();
            promptItems.Add("Exit");
            
            var itemToEatName = Prompt.Select("Which item would you like to eat?", promptItems);
            if (itemToEatName == "Exit") return;

            var itemToEatRecord = foodInInventory.FirstOrDefault(record => record.InventoryItem.Name == itemToEatName);
            if (itemToEatRecord == null) return; // todo better way to do this...
            
            if (itemToEatRecord.InventoryItem.Droppable == false)
            {
                Log.Warning($"You can't eat your {itemToEatName}");
            }
            else
            {
                var quantity = ViewHelpers.GetQuantity($"You have {itemToEatRecord.Quantity} {itemToEatName}(s). How many would you like to eat?",
                    itemToEatRecord.Quantity);

                if (quantity == 0) return;

                var previousHealth = player.Health;
                var previousHunger = player.Hunger;
                
                PlayerController.Eat((Food) itemToEatRecord.InventoryItem, quantity);

                Log.Success("Delicious!");
                Log.Info($"Health +{player.Health - previousHealth} \nHunger +{player.Hunger - previousHunger}\n");
            }
        }

        // private void EquipGear()
        // {
        //     var inventory = Shipwreck.CurrentGame.Player.Inventory;
        //     var itemToEquip = ViewHelpers.GetInventoryItem("Which item would you like to equip?");
        //     if (itemToEquip != null)
        //     {
        //         if (itemToEquip.GetType().IsSubclassOf(typeof(Weapon)))
        //         {
        //             inventory.ActiveWeapon = (Weapon)itemToEquip;
        //         }
        //         else if (itemToEquip.GetType() == typeof(Armor))
        //         {
        //             inventory.ActiveArmor = (Armor)itemToEquip;
        //         }
        //         Console.WriteLine($"Your {itemToEquip.Name} has been equipped");
        //     }
        //     else
        //     {
        //         Console.WriteLine($"That is not an item that exists in your inventory");
        //     }
        // }

        private void DropItem()
        {
            var inventory = Shipwreck.CurrentGame.Player.Inventory;
            var droppableItemRecords = inventory.Items.Where(itemRecord => itemRecord.InventoryItem.Droppable).ToList();
            var promptItems = droppableItemRecords.Select(record => record.InventoryItem.Name).ToList();
            promptItems.Add("Exit");
            
            var itemToDropName = Prompt.Select("Which item would you like to drop?", promptItems);
            if (itemToDropName == "Exit") return;

            var itemRecordToDrop =
                droppableItemRecords.FirstOrDefault(record => record.InventoryItem.Name == itemToDropName);
            if (itemRecordToDrop == null) return; // todo there's gotta be a better way to do this...
            
            var quantity = ViewHelpers.GetQuantity($"You have {itemRecordToDrop.Quantity} {itemToDropName}(s). How Many would you like to drop?", itemRecordToDrop.Quantity);

            var numDropped = InventoryController.RemoveItems(inventory, itemRecordToDrop?.InventoryItem, quantity);
            Log.Success($"You dropped {numDropped} {itemToDropName}(s)");
        }
    }
}
