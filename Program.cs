using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    // Game item class to represent collectible items
    class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }

        public Item(string name, string description, int value)
        {
            Name = name;
            Description = description;
            Value = value;
        }
    }

    // Player class to store player information
    class Player
    {
        public int Health { get; set; }
        public int Gold { get; set; }
        public List<Item> Inventory { get; set; }
        public string CurrentLocation { get; set; }

        public Player()
        {
            Health = 100;
            Gold = 0;
            Inventory = new List<Item>();
            CurrentLocation = "Entrance";
        }

        // Add an item to player's inventory
        public void AddItem(Item item)
        {
            Inventory.Add(item);
            Console.WriteLine($"Added {item.Name} to your inventory!");
        }

        // Check if player has a specific item
        public bool HasItem(string itemName)
        {
            return Inventory.Any(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }

        // Display player stats
        public void DisplayStats()
        {
            Console.WriteLine("\n--- Player Stats ---");
            Console.WriteLine($"Health: {Health}");
            Console.WriteLine($"Gold: {Gold}");
            Console.WriteLine("Inventory:");

            if (Inventory.Count == 0)
            {
                Console.WriteLine("  Empty");
            }
            else
            {
                foreach (Item item in Inventory)
                {
                    Console.WriteLine($"  {item.Name} - {item.Description} (Value: {item.Value})");
                }
            }
            Console.WriteLine("-------------------\n");
        }
    }

    // Game class to handle the game logic
    class Game
    {
        private Player player;
        private Dictionary<string, Action> locations;
        private Random random;
        private bool gameRunning;
        private const string saveFilePath = "savegame.txt";

        public Game()
        {
            player = new Player();
            random = new Random();
            gameRunning = true;

            // Initialize locations dictionary with corresponding methods
            locations = new Dictionary<string, Action>
            {
                { "Entrance", EntranceArea },
                { "MainHall", MainHall },
                { "RightRoom", RightRoom },
                { "LeftRoom", LeftRoom },
                { "NextRoom", NextRoom },
                { "Treasury", Treasury },
                { "DungeonRoom", DungeonRoom }
            };
        }

        // Start the game
        public void Start()
        {
            DisplayWelcomeMessage();

            // Ask if player wants to load a saved game
            if (File.Exists(saveFilePath))
            {
                Console.WriteLine("A saved game was found. Do you want to load it? (yes/no)");
                string choice = Console.ReadLine().ToLower();

                if (choice == "yes" || choice == "y")
                {
                    LoadGame();
                }
            }

            // Main game loop
            while (gameRunning)
            {
                // Navigate to current location
                if (locations.ContainsKey(player.CurrentLocation))
                {
                    locations[player.CurrentLocation]();
                }
                else
                {
                    Console.WriteLine("Error: Invalid location. Returning to entrance.");
                    player.CurrentLocation = "Entrance";
                }
            }
        }

        // Display welcome message
        private void DisplayWelcomeMessage()
        {
            Console.WriteLine("=== THE CASTLE ADVENTURE ===");
            Console.WriteLine("Welcome to the adventure! You arrive at an old castle.");
            Console.WriteLine("Type 'help' at any prompt to see available commands.");
            Console.WriteLine("================================");
        }

        // Process common commands available everywhere
        private bool ProcessCommonCommands(string input)
        {
            switch (input.ToLower())
            {
                case "help":
                    DisplayHelp();
                    return true;

                case "stats":
                    player.DisplayStats();
                    return true;

                case "save":
                    SaveGame();
                    return true;

                case "quit":
                    Console.WriteLine("Are you sure you want to quit? (yes/no)");
                    string quitChoice = Console.ReadLine().ToLower();
                    if (quitChoice == "yes" || quitChoice == "y")
                    {
                        Console.WriteLine("Thanks for playing! Goodbye!");
                        gameRunning = false;
                    }
                    return true;

                default:
                    return false;
            }
        }

        // Display help information
        private void DisplayHelp()
        {
            Console.WriteLine("\n--- Available Commands ---");
            Console.WriteLine("help - Display this help message");
            Console.WriteLine("stats - Show your character stats");
            Console.WriteLine("save - Save your game progress");
            Console.WriteLine("quit - Exit the game");
            Console.WriteLine("------------------------\n");
        }

        // Save game progress to file
        private void SaveGame()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(saveFilePath))
                {
                    // Save player stats
                    writer.WriteLine($"Health:{player.Health}");
                    writer.WriteLine($"Gold:{player.Gold}");
                    writer.WriteLine($"Location:{player.CurrentLocation}");

                    // Save inventory
                    writer.WriteLine($"InventoryCount:{player.Inventory.Count}");
                    foreach (Item item in player.Inventory)
                    {
                        writer.WriteLine($"Item:{item.Name},{item.Description},{item.Value}");
                    }
                }

                Console.WriteLine("Game saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
            }
        }

        // Load game progress from file
        private void LoadGame()
        {
            try
            {
                if (File.Exists(saveFilePath))
                {
                    string[] lines = File.ReadAllLines(saveFilePath);
                    player = new Player(); // Reset player
                    player.Inventory.Clear(); // Clear inventory

                    foreach (string line in lines)
                    {
                        if (line.StartsWith("Health:"))
                        {
                            player.Health = int.Parse(line.Substring(7));
                        }
                        else if (line.StartsWith("Gold:"))
                        {
                            player.Gold = int.Parse(line.Substring(5));
                        }
                        else if (line.StartsWith("Location:"))
                        {
                            player.CurrentLocation = line.Substring(9);
                        }
                        else if (line.StartsWith("Item:"))
                        {
                            string[] itemData = line.Substring(5).Split(',');
                            if (itemData.Length >= 3)
                            {
                                Item item = new Item(
                                    itemData[0],
                                    itemData[1],
                                    int.Parse(itemData[2])
                                );
                                player.Inventory.Add(item);
                            }
                        }
                    }

                    Console.WriteLine("Game loaded successfully!");
                    player.DisplayStats();
                }
                else
                {
                    Console.WriteLine("No saved game found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
                // If loading fails, start a new game
                player = new Player();
            }
        }

        // Castle entrance area
        private void EntranceArea()
        {
            Console.WriteLine("\nYou stand before the entrance to an ancient castle. The massive wooden doors are slightly ajar.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Enter the castle");
            Console.WriteLine("2. Examine the entrance");
            Console.WriteLine("3. Leave (end game)");

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("You push the heavy doors open and step inside...");
                    player.CurrentLocation = "MainHall";
                    break;

                case "2":
                    Console.WriteLine("The castle is made of dark stone, weathered by time. The entrance has intricate carvings around the frame.");
                    Console.WriteLine("You notice a small coin on the ground.");
                    Console.WriteLine("Do you want to pick it up? (yes/no)");

                    string pickupChoice = Console.ReadLine().ToLower();
                    if (pickupChoice == "yes" || pickupChoice == "y")
                    {
                        Console.WriteLine("You found 10 gold coins!");
                        player.Gold += 10;
                    }
                    break;

                case "3":
                    Console.WriteLine("You decide not to enter the castle and leave. Game over.");
                    gameRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Castle main hall
        private void MainHall()
        {
            Console.WriteLine("\nYou are in the castle's grand hall. Dust covers the once elegant furniture.");
            Console.WriteLine("There are doorways to the right and left, and a large staircase ahead.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Go through the right doorway");
            Console.WriteLine("2. Go through the left doorway");
            Console.WriteLine("3. Climb the staircase");
            Console.WriteLine("4. Return to the entrance");

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    player.CurrentLocation = "RightRoom";
                    break;

                case "2":
                    player.CurrentLocation = "LeftRoom";
                    break;

                case "3":
                    player.CurrentLocation = "NextRoom";
                    break;

                case "4":
                    player.CurrentLocation = "Entrance";
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Right room with the skeleton
        private void RightRoom()
        {
            Console.WriteLine("\nYou enter a dusty chamber. A skeleton lies in the corner, still wearing tattered noble clothes.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Examine the skeleton");
            Console.WriteLine("2. Search the room");
            Console.WriteLine("3. Go to the dungeon (doorway at the back)");
            Console.WriteLine("4. Return to the main hall");

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    if (!player.HasItem("Rusty Key"))
                    {
                        Console.WriteLine("You carefully examine the skeleton. You find a rusty key clutched in its bony fingers!");
                        Item key = new Item("Rusty Key", "An old rusted key that might open something important", 5);
                        player.AddItem(key);
                    }
                    else
                    {
                        Console.WriteLine("You've already taken the key from the skeleton.");
                    }
                    break;

                case "2":
                    // Random chance to find something
                    int chance = random.Next(1, 4);
                    if (chance == 1)
                    {
                        Console.WriteLine("You find 15 gold coins hidden in a crack in the wall!");
                        player.Gold += 15;
                    }
                    else
                    {
                        Console.WriteLine("You search the room but find nothing of interest.");
                    }
                    break;

                case "3":
                    player.CurrentLocation = "DungeonRoom";
                    break;

                case "4":
                    player.CurrentLocation = "MainHall";
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Left room with the chest
        private void LeftRoom()
        {
            Console.WriteLine("\nYou enter what appears to be an old study. There's a wooden chest in the corner.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Open the chest");
            Console.WriteLine("2. Examine the bookshelves");
            Console.WriteLine("3. Go to the treasury (through a small door)");
            Console.WriteLine("4. Return to the main hall");

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("You open the chest and find 100 gold coins!");
                    player.Gold += 100;
                    Console.WriteLine("The chest is now empty.");
                    break;

                case "2":
                    Console.WriteLine("You examine the bookshelves and find an old leather-bound book.");
                    Console.WriteLine("Do you want to take it? (yes/no)");

                    string bookChoice = Console.ReadLine().ToLower();
                    if (bookChoice == "yes" || bookChoice == "y")
                    {
                        if (!player.HasItem("Ancient Book"))
                        {
                            Item book = new Item("Ancient Book", "A mysterious book with strange symbols", 20);
                            player.AddItem(book);
                        }
                        else
                        {
                            Console.WriteLine("You already have this book in your inventory.");
                        }
                    }
                    break;

                case "3":
                    player.CurrentLocation = "Treasury";
                    break;

                case "4":
                    player.CurrentLocation = "MainHall";
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Next room with the locked door
        private void NextRoom()
        {
            Console.WriteLine("\nYou climb the staircase and reach a landing with a large locked door.");
            Console.WriteLine("This appears to be the exit from the castle's upper level.");

            if (player.HasItem("Rusty Key"))
            {
                Console.WriteLine("You have the rusty key that might unlock this door.");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Use the key to unlock the door");
                Console.WriteLine("2. Return to the main hall");
            }
            else
            {
                Console.WriteLine("The door is locked. You need a key to open it.");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Return to the main hall");
            }

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            if (player.HasItem("Rusty Key"))
            {
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("You use the rusty key and the door unlocks with a loud click!");
                        Console.WriteLine("You push the door open and step outside to a balcony overlooking the countryside.");

                        if (player.Gold >= 100)
                        {
                            Console.WriteLine("Congratulations! You've escaped the castle with a fortune of " + player.Gold + " gold!");
                        }
                        else
                        {
                            Console.WriteLine("Congratulations! You've escaped the castle with " + player.Gold + " gold.");
                        }

                        Console.WriteLine("Would you like to play again? (yes/no)");
                        string playAgain = Console.ReadLine().ToLower();

                        if (playAgain == "yes" || playAgain == "y")
                        {
                            player = new Player();
                        }
                        else
                        {
                            Console.WriteLine("Thanks for playing!");
                            gameRunning = false;
                        }
                        break;

                    case "2":
                        player.CurrentLocation = "MainHall";
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                switch (choice)
                {
                    case "1":
                        player.CurrentLocation = "MainHall";
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        // Treasury room
        private void Treasury()
        {
            Console.WriteLine("\nYou enter a small treasury. Most valuables seem to have been looted long ago.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Search for remaining treasures");
            Console.WriteLine("2. Examine the wall mural");
            Console.WriteLine("3. Return to the left room");

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    // Use a for loop to simulate searching multiple spots
                    Console.WriteLine("You start searching around the room...");
                    int totalFound = 0;

                    for (int i = 0; i < 3; i++)
                    {
                        int foundAmount = random.Next(0, 31);
                        totalFound += foundAmount;

                        switch (i)
                        {
                            case 0:
                                Console.WriteLine($"You check behind a loose stone... {foundAmount} gold found.");
                                break;
                            case 1:
                                Console.WriteLine($"You look under an old carpet... {foundAmount} gold found.");
                                break;
                            case 2:
                                Console.WriteLine($"You examine an old vase... {foundAmount} gold found.");
                                break;
                        }
                    }

                    if (totalFound > 0)
                    {
                        Console.WriteLine($"In total, you found {totalFound} gold coins!");
                        player.Gold += totalFound;
                    }
                    else
                    {
                        Console.WriteLine("Unfortunately, you didn't find any gold this time.");
                    }
                    break;

                case "2":
                    Console.WriteLine("The wall mural depicts the history of the castle. One section shows a hidden room behind the dungeon.");
                    if (!player.HasItem("Treasure Map"))
                    {
                        Console.WriteLine("You notice a small map tucked into a crack in the wall!");
                        Item map = new Item("Treasure Map", "A map showing a secret location in the castle", 30);
                        player.AddItem(map);
                    }
                    break;

                case "3":
                    player.CurrentLocation = "LeftRoom";
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Dungeon room
        private void DungeonRoom()
        {
            Console.WriteLine("\nYou descend into a dark, damp dungeon. Chains hang from the walls, and there are old cells for prisoners.");
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1. Investigate the cells");
            Console.WriteLine("2. Look for secret passages");
            Console.WriteLine("3. Return to the right room");

            string choice = Console.ReadLine();

            if (ProcessCommonCommands(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1":
                    Console.WriteLine("You check the cells one by one. Most are empty, but in the last one you find a skeleton clutching something.");
                    if (!player.HasItem("Silver Amulet"))
                    {
                        Item amulet = new Item("Silver Amulet", "A silver amulet with strange engravings", 50);
                        player.AddItem(amulet);
                    }
                    else
                    {
                        Console.WriteLine("You've already taken the silver amulet.");
                    }
                    break;

                case "2":
                    if (player.HasItem("Treasure Map"))
                    {
                        Console.WriteLine("Using your treasure map, you locate a loose stone in the wall!");
                        Console.WriteLine("Behind it, you find a small treasure chest containing 200 gold coins and a jeweled dagger!");
                        player.Gold += 200;

                        if (!player.HasItem("Jeweled Dagger"))
                        {
                            Item dagger = new Item("Jeweled Dagger", "An ornate dagger with precious gems", 100);
                            player.AddItem(dagger);
                        }
                    }
                    else
                    {
                        Console.WriteLine("You tap on walls and check for loose stones, but find nothing unusual.");
                        Console.WriteLine("You have a feeling you're missing something that would help you find a secret passage.");
                    }
                    break;

                case "3":
                    player.CurrentLocation = "RightRoom";
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Game textAdventure = new Game();
                textAdventure.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.WriteLine("The game will now exit. Sorry for the inconvenience.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}