using System;
using System.Collections.Generic;

namespace TextAdventureGUI
{
    public class Player
    {
        public int Health { get; set; }
        public int Gold { get; set; }
        public List<Item> Inventory { get; set; }
        public string CurrentLocation { get; set; }
        public bool HasKey { get; set; }

        public Player()
        {
            Health = 100;
            Gold = 0;
            Inventory = new List<Item>();
            CurrentLocation = "Entrance";
            HasKey = false;
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
            if (item.Name == "Rusty Key")
            {
                HasKey = true;
            }
        }

        public bool HasItem(string itemName)
        {
            foreach (Item item in Inventory)
            {
                if (item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
