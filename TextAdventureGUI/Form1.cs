using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TextAdventureGUI
{
    public partial class Linnaseikkailu : Form
    {
        // Game state
        private Player player;
        private bool gameRunning;
        private const string saveFile = "savegame.txt";
        private Color themeBackColor = ColorTranslator.FromHtml("#2C2416");      // Tumma ruskea
        private Color themeParchmentColor = ColorTranslator.FromHtml("#EDE4D4"); // Pergamentti
        private Color themeGoldColor = ColorTranslator.FromHtml("#D4AF37");      // Kulta
        private Color themeStoneColor = ColorTranslator.FromHtml("#767676");     // Kiviharmaa
        private Color themeAccentColor = ColorTranslator.FromHtml("#8B0000");    // Tummanpunainen


        public Linnaseikkailu()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            player = new Player();
            gameRunning = true;

            // Set up UI
            this.Text = "Text Adventure";
            storytext.Clear();

            // Disable all option buttons initially
            btnOption1.Enabled = false;
            btnOption2.Enabled = false;
            btnOption3.Enabled = false;
            btnOption4.Enabled = false;

            // Set status
            UpdateStatus();

            // Start the game
            DisplayLocation("Entrance");
        }

        private void UpdateStatus()
        {
            toolStripStatusLabel1.Text = $"Health: {player.Health} | Gold: {player.Gold} | Items: {player.Inventory.Count}";
        }

        private void DisplayLocation(string location)
        {
            player.CurrentLocation = location;
            UpdateLocationImage(location);
            storytext.Clear();

            switch (location)
            {
                case "Entrance":
                    storytext.AppendText("You are standing outside a castle. The entrance is slightly ajar.\n\n");
                    storytext.AppendText("What do you want to do?");

                    SetupButtons(
                        "Enter the castle", () => DisplayLocation("MainHall"),
                        "Examine the entrance", ExamineEntrance,
                        "Leave (end game)", EndGame,
                        "", null
                    );
                    break;

                case "MainHall":
                    storytext.AppendText("You are in the grand hall of the castle. There are doorways to the right and left, and a staircase ahead.\n\n");
                    storytext.AppendText("What do you want to do?");

                    SetupButtons(
                        "Go right", () => DisplayLocation("RightRoom"),
                        "Go left", () => DisplayLocation("LeftRoom"),
                        "Go upstairs", () => DisplayLocation("UpperRoom"),
                        "Return to entrance", () => DisplayLocation("Entrance")
                    );
                    break;

                case "RightRoom":
                    storytext.AppendText("You enter a dusty chamber. A skeleton lies in the corner, still wearing tattered noble clothes.\n\n");
                    storytext.AppendText("What do you want to do?");

                    SetupButtons(
                        "Examine skeleton", ExamineSkeleton,
                        "Search the room", SearchRightRoom,
                        "Return to hall", () => DisplayLocation("MainHall"),
                        "", null
                    );
                    break;

                case "LeftRoom":
                    storytext.AppendText("You enter what appears to be an old study. There's a wooden chest in the corner.\n\n");
                    storytext.AppendText("What do you want to do?");

                    SetupButtons(
                        "Open chest", OpenChest,
                        "Search the room", SearchLeftRoom,
                        "Return to hall", () => DisplayLocation("MainHall"),
                        "", null
                    );
                    break;

                case "UpperRoom":
                    storytext.AppendText("You climb the staircase and reach a landing with a large locked door.\n\n");

                    if (player.HasKey)
                    {
                        storytext.AppendText("You have the rusty key that might fit this door.");
                        SetupButtons(
                            "Use key", UseKey,
                            "Return to hall", () => DisplayLocation("MainHall"),
                            "", null,
                            "", null
                        );
                    }
                    else
                    {
                        storytext.AppendText("The door is locked. You need a key to open it.");
                        SetupButtons(
                            "Return to hall", () => DisplayLocation("MainHall"),
                            "", null,
                            "", null,
                            "", null
                        );
                    }
                    break;
            }

            UpdateStatus();
        }

        private void SetupButtons(string text1, Action action1, string text2, Action action2, string text3, Action action3, string text4 = "", Action action4 = null)
        {
            // Button 1
            btnOption1.Text = text1;
            btnOption1.Enabled = !string.IsNullOrEmpty(text1);
            btnOption1.Tag = action1;
            btnOption1.Visible = btnOption1.Enabled;

            // Button 2
            btnOption2.Text = text2;
            btnOption2.Enabled = !string.IsNullOrEmpty(text2);
            btnOption2.Tag = action2;
            btnOption2.Visible = btnOption2.Enabled;

            // Button 3
            btnOption3.Text = text3;
            btnOption3.Enabled = !string.IsNullOrEmpty(text3);
            btnOption3.Tag = action3;
            btnOption3.Visible = btnOption3.Enabled;

            // Button 4
            btnOption4.Text = text4;
            btnOption4.Enabled = !string.IsNullOrEmpty(text4);
            btnOption4.Tag = action4;
            btnOption4.Visible = btnOption4.Enabled;
        }

        // Event handlers for button clicks
        private void btnOption1_Click(object sender, EventArgs e)
        {
            if (btnOption1.Tag is Action action)
            {
                action();
            }
        }

        private void btnOption2_Click(object sender, EventArgs e)
        {
            if (btnOption2.Tag is Action action)
            {
                action();
            }
        }

        private void btnOption3_Click(object sender, EventArgs e)
        {
            if (btnOption3.Tag is Action action)
            {
                action();
            }
        }

        private void btnOption4_Click(object sender, EventArgs e)
        {
            if (btnOption4.Tag is Action action)
            {
                action();
            }
        }

        // Game actions
        private void ExamineEntrance()
        {
            storytext.Clear();
            storytext.AppendText("You examine the entrance. The door is made of heavy oak, with iron fixtures. You notice a gold coin on the ground!\n\n");

            SetupButtons(
                "Take coin", () => {
                    player.Gold += 10;
                    UpdateStatus();
                    DisplayLocation("Entrance");
                },
                "Ignore it", () => DisplayLocation("Entrance"),
                "", null,
                "", null
            );
        }

        private void ExamineSkeleton()
        {
            storytext.Clear();

            if (!player.HasItem("Rusty Key"))
            {
                storytext.AppendText("You examine the skeleton. In its bony hand, you find a rusty key!\n\n");
                Item key = new Item("Rusty Key", "An old key that might open something important", 5);
                player.AddItem(key);
                UpdateStatus();
            }
            else
            {
                storytext.AppendText("You've already taken the key from the skeleton.\n\n");
            }

            SetupButtons(
                "Return", () => DisplayLocation("RightRoom"),
                "", null,
                "", null,
                "", null
            );
        }

        private void SearchRightRoom()
        {
            storytext.Clear();

            Random random = new Random();
            if (random.Next(3) == 0)
            {
                storytext.AppendText("You search the room and find 15 gold coins hidden in a crack in the wall!\n\n");
                player.Gold += 15;
                UpdateStatus();
            }
            else
            {
                storytext.AppendText("You search the room but find nothing of interest.\n\n");
            }

            SetupButtons(
                "Return", () => DisplayLocation("RightRoom"),
                "", null,
                "", null,
                "", null
            );
        }

        private void OpenChest()
        {
            storytext.Clear();
            storytext.AppendText("You open the chest and find 100 gold coins!\n\n");
            player.Gold += 100;
            UpdateStatus();

            SetupButtons(
                "Return", () => DisplayLocation("LeftRoom"),
                "", null,
                "", null,
                "", null
            );
        }

        private void SearchLeftRoom()
        {
            storytext.Clear();
            storytext.AppendText("You examine the bookshelves and find an old book about the castle's history.\n\n");

            if (!player.HasItem("Ancient Book"))
            {
                storytext.AppendText("You decide to take the book with you.");
                Item book = new Item("Ancient Book", "A history of the castle", 10);
                player.AddItem(book);
                UpdateStatus();
            }
            else
            {
                storytext.AppendText("You already have this book.");
            }

            SetupButtons(
                "Return", () => DisplayLocation("LeftRoom"),
                "", null,
                "", null,
                "", null
            );
        }

        private void UseKey()
        {
            storytext.Clear();
            storytext.AppendText("You use the rusty key on the door. It turns with a loud click, and the door swings open!\n\n");

            if (player.Gold >= 100)
            {
                storytext.AppendText($"Congratulations! You escaped the castle with a fortune of {player.Gold} gold!");
            }
            else
            {
                storytext.AppendText($"You escaped the castle with {player.Gold} gold. Not bad!");
            }

            SetupButtons(
                "Play again", () => {
                    InitializeGame();
                },
                "Quit", () => {
                    Application.Exit();
                },
                "", null,
                "", null
            );
        }

        private void EndGame()
        {
            storytext.Clear();
            storytext.AppendText("You decide to leave without exploring the castle. Game over!\n\n");

            SetupButtons(
                "Play again", () => {
                    InitializeGame();
                },
                "Quit", () => {
                    Application.Exit();
                },
                "", null,
                "", null
            );
        }

        // Menu functions
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Start a new game? Your current progress will be lost.",
                "New Game",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                InitializeGame();
            }
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(saveFile))
                {
                    writer.WriteLine($"Location:{player.CurrentLocation}");
                    writer.WriteLine($"Health:{player.Health}");
                    writer.WriteLine($"Gold:{player.Gold}");
                    writer.WriteLine($"HasKey:{player.HasKey}");

                    writer.WriteLine($"ItemCount:{player.Inventory.Count}");
                    foreach (Item item in player.Inventory)
                    {
                        writer.WriteLine($"Item:{item.Name},{item.Description},{item.Value}");
                    }
                }

                MessageBox.Show("Game saved successfully!", "Save Game", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(saveFile))
            {
                MessageBox.Show("No saved game found.", "Load Game", MessageBoxButtons.OK);
                return;
            }

            try
            {
                player = new Player();
                player.Inventory.Clear();

                string[] lines = File.ReadAllLines(saveFile);

                foreach (string line in lines)
                {
                    if (line.StartsWith("Location:"))
                    {
                        player.CurrentLocation = line.Substring(9);
                    }
                    else if (line.StartsWith("Health:"))
                    {
                        player.Health = int.Parse(line.Substring(7));
                    }
                    else if (line.StartsWith("Gold:"))
                    {
                        player.Gold = int.Parse(line.Substring(5));
                    }
                    else if (line.StartsWith("HasKey:"))
                    {
                        player.HasKey = bool.Parse(line.Substring(7));
                    }
                    else if (line.StartsWith("Item:"))
                    {
                        string[] itemData = line.Substring(5).Split(',');
                        if (itemData.Length == 3)
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

                MessageBox.Show("Game loaded successfully!", "Load Game", MessageBoxButtons.OK);
                DisplayLocation(player.CurrentLocation);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InitializeGame();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit",
                MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string inventoryText = "Inventory:\n\n";

            if (player.Inventory.Count == 0)
            {
                inventoryText += "Empty";
            }
            else
            {
                foreach (Item item in player.Inventory)
                {
                    inventoryText += $"{item.Name} - {item.Description} (Value: {item.Value})\n";
                }
            }

            MessageBox.Show(inventoryText, "Inventory", MessageBoxButtons.OK);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Text Adventure\nA simple adventure game created with C# and Windows Forms.\n\nCreated by: Kari Markus",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // Päivittää sijainnin kuvan
        private void UpdateLocationImage(string location)
        {
            string imagePath = "";

            switch (location)
            {
                case "Entrance":
                    imagePath = Path.Combine(Application.StartupPath, "Images", "entrance.jpg");
                    break;
                case "MainHall":
                    imagePath = Path.Combine(Application.StartupPath, "Images", "mainhall.jpg");
                    break;
                case "RightRoom":
                    imagePath = Path.Combine(Application.StartupPath, "Images", "skeletonroom.jpg");
                    break;
                case "LeftRoom":
                    imagePath = Path.Combine(Application.StartupPath, "Images", "studyroom.jpg");
                    break;
                case "UpperRoom":
                    imagePath = Path.Combine(Application.StartupPath, "Images", "landing.jpg");
                    break;
                default:
                    imagePath = Path.Combine(Application.StartupPath, "Images", "default.jpg");
                    break;
            }

            if (File.Exists(imagePath))
            {
                try
                {
                    // Vapauta mahdollinen aiempi kuva muistista
                    if (picLocation.Image != null)
                    {
                        picLocation.Image.Dispose();
                    }

                    // Lataa uusi kuva
                    picLocation.Image = Image.FromFile(imagePath);
                }
                catch (Exception ex)
                {
                    // Jos kuvan lataus epäonnistuu, näytä virheilmoitus
                    Console.WriteLine($"Error loading image: {ex.Message}");
                    picLocation.Image = CreateDefaultImage();
                }
            }
            else
            {
                // Jos kuvaa ei löydy, näytä oletuskuva
                picLocation.Image = CreateDefaultImage();
            }
        }

        // Luo oletuskuva, jos oikeaa kuvaa ei löydy
        private Image CreateDefaultImage()
        {
            // Luo yksinkertainen gradientti-kuva
            Bitmap bmp = new Bitmap(picLocation.Width, picLocation.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Täytä tausta liukuvärillä
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    Color.DarkBlue, Color.LightBlue,
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, 0, 0, bmp.Width, bmp.Height);
                }

                // Lisää teksti keskelle
                string text = "Castle Adventure";
                using (Font font = new Font("Arial", 20, FontStyle.Bold))
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    g.DrawString(text, font, Brushes.White,
                        new Rectangle(0, 0, bmp.Width, bmp.Height), sf);
                }
            }

            return bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnOption1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnOption2_Click_1(object sender, EventArgs e)
        {

        }

        private void btnOption3_Click_1(object sender, EventArgs e)
        {

        }

        private void btnOption4_Click_1(object sender, EventArgs e)
        {

        }
    }
}