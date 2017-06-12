// Authors: Gabriel Chartier and Jacob Stevens
// Date Started: 4.12.16
// Team Name: Snarkle
// This application is a take on the game Farkle

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Snarkle
{
    public sealed partial class GamePage : Page
    {
        // Fields
        int[] dice = new int[6] { 0, 0, 0, 0, 0, 0 };      // Current die values that have been rolled
        int[] dieCounts = new int[6] { 0, 0, 0, 0, 0, 0 }; // Number of times each die value appears in the roll
        List<Player> players = new List<Player>();         // List of players to play the game
        int currentPlayer = 0;                             // The current player whose turn it is

        // Main method
        public GamePage()
        {
            this.InitializeComponent();
        }

        // Set up the game and start when this page is navigated to
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Create the players and set their names
            List<string> names = (List<string>)e.Parameter;
            foreach (string name in names)
                players.Add(new Player() { Name = name });

            // Show the player tabs and names according to the number of players chosen
            player1Marker.Visibility = Visibility.Visible;
            switch (players.Count)
            {
                case 1:
                    player1Tab.Visibility = Visibility.Visible;
                    player1Name.Visibility = Visibility.Visible;
                    player1Score.Visibility = Visibility.Visible;
                    player1Name.Text = names[0];
                    break;
                case 2:
                    player1Tab.Visibility = Visibility.Visible;
                    player1Name.Visibility = Visibility.Visible;
                    player1Score.Visibility = Visibility.Visible;
                    player1Name.Text = names[0];
                    player2Tab.Visibility = Visibility.Visible;
                    player2Name.Visibility = Visibility.Visible;
                    player2Score.Visibility = Visibility.Visible;
                    player2Name.Text = names[1];
                    break;
                case 3:
                    player1Tab.Visibility = Visibility.Visible;
                    player1Name.Visibility = Visibility.Visible;
                    player1Score.Visibility = Visibility.Visible;
                    player1Name.Text = names[0];
                    player2Tab.Visibility = Visibility.Visible;
                    player2Name.Visibility = Visibility.Visible;
                    player2Score.Visibility = Visibility.Visible;
                    player2Name.Text = names[1];
                    player3Tab.Visibility = Visibility.Visible;
                    player3Name.Visibility = Visibility.Visible;
                    player3Score.Visibility = Visibility.Visible;
                    player3Name.Text = names[2];
                    break;
                case 4:
                    player1Tab.Visibility = Visibility.Visible;
                    player1Name.Visibility = Visibility.Visible;
                    player1Score.Visibility = Visibility.Visible;
                    player1Name.Text = names[0];
                    player2Tab.Visibility = Visibility.Visible;
                    player2Name.Visibility = Visibility.Visible;
                    player2Score.Visibility = Visibility.Visible;
                    player2Name.Text = names[1];
                    player3Tab.Visibility = Visibility.Visible;
                    player3Name.Visibility = Visibility.Visible;
                    player3Score.Visibility = Visibility.Visible;
                    player3Name.Text = names[2];
                    player4Tab.Visibility = Visibility.Visible;
                    player4Name.Visibility = Visibility.Visible;
                    player4Score.Visibility = Visibility.Visible;
                    player4Name.Text = names[3];
                    break;
                default:
                    player1Tab.Visibility = Visibility.Visible;
                    player1Name.Visibility = Visibility.Visible;
                    player1Score.Visibility = Visibility.Visible;
                    player1Name.Text = names[0];
                    break;
            }

        }

        #region Dice Check Logic
        // Check the keep dice and add appropriate point values to current round total
        private int checkDice(List<int> keepDice)
        {
            dieCounts = new int[6] { 0, 0, 0, 0, 0, 0 }; // Number of times each die value appears in the roll
            int diceTotal = 0;                           // Points for the current dice kept

            // Calculate how many times each die appears in a keep and store it in die counts
            foreach (int die in keepDice)
                dieCounts[die - 1]++;

            // Check for a six of a kind and add to total
            for (int i = 0; i < 6; i++)
                if (dieCounts[i] == 6)
                {
                    dieCounts[i] = 0;
                    diceTotal += 3000;
                }

            // Check for a five of a kind and add to total
            for (int i = 0; i < 6; i++)
                if (dieCounts[i] == 5)
                {
                    dieCounts[i] = 0;
                    diceTotal += 2000;
                }

            // Check for a straight and add to total
            if (isStraight())
            {
                diceTotal += 1500;
                for (int i = 0; i < 6; i++)
                    dieCounts[i] = 0;
            }

            // Check for two Triplets and add to total
            if (isTwoTriplets())
            {
                diceTotal += 2500;
                for (int i = 0; i < 6; i++)
                    dieCounts[i] = 0;
            }

            // Check for four of a kind and a pair and add to total
            if (isFourPlusPair())
            {
                diceTotal += 1500;
                for (int i = 0; i < 6; i++)
                    dieCounts[i] = 0;
            }

            // Check for three pair and add to total
            if (isThreePair())
            {
                diceTotal += 1500;
                for (int i = 0; i < 6; i++)
                    dieCounts[i] = 0;
            }

            // Check for four of a kind and add to total
            for (int i = 0; i < 6; i++)
                if (dieCounts[i] == 4)
                {
                    dieCounts[i] = 0;
                    diceTotal += 1000;
                }

            // Check for three of a kind and add to total
            for (int i = 0; i < 6; i++)
            {
                if (dieCounts[i] == 3)
                {
                    dieCounts[i] = 0;
                    if (i == 0)
                        diceTotal += 300;
                    else
                        diceTotal += (i + 1) * 100;
                }
            }

            // Check for any extra 1's and add to total
            diceTotal += dieCounts[0] * 100;
            dieCounts[0] = 0;

            // Check for any extra 5's and add to total
            diceTotal += dieCounts[4] * 50;
            dieCounts[4] = 0;

            return diceTotal;
        }

        // Check if a straight exists in the current die counts
        private bool isStraight()
        {
            bool straight = true;
            for (int i = 0; i < 6; i++)
            {
                if (dieCounts[i] != 1)
                    straight = false;
            }
            return straight;
        }

        // Check if two triplets exists in the current die counts
        private bool isTwoTriplets()
        {
            int triplets = 0;
            for (int i = 0; i < 6; i++)
            {
                if (dieCounts[i] == 3)
                    triplets++;
            }
            return triplets == 2;
        }

        // Check if a four plus pair exists in the current die counts
        private bool isFourPlusPair()
        {
            bool fourOfAKind = false;
            bool pair = false;

            for (int i = 0; i < 6; i++)
            {
                if (dieCounts[i] == 4)
                    fourOfAKind = true;
                if (dieCounts[i] == 2)
                    pair = true;
            }
            return pair && fourOfAKind;
        }

        // Check if three pair exist in the current die counts
        private bool isThreePair()
        {
            int pairs = 0;

            for (int i = 0; i < 6; i++)
                if (dieCounts[i] == 2)
                    pairs++;
            return pairs == 3;
        }
        #endregion

        // Roll the dice and check for a farkle
        private async void roll_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();                // Random object used for getting new die values
            List<int> rolledDice = new List<int>(); // List of dice that are rolled

            roll.IsEnabled = false;
            keep.IsEnabled = true;

            // Get a random value between 1-6 for each rollable die
            if (die1Roll.Visibility == Visibility.Visible)
            {
                dice[0] = r.Next(1, 7);
                die1Roll.Source = new BitmapImage(new Uri(getDieImage(dice[0]), UriKind.Absolute));
                rolledDice.Add(dice[0]);
            }
            if (die2Roll.Visibility == Visibility.Visible)
            {
                dice[1] = r.Next(1, 7);
                die2Roll.Source = new BitmapImage(new Uri(getDieImage(dice[1]), UriKind.Absolute));
                rolledDice.Add(dice[1]);
            }
            if (die3Roll.Visibility == Visibility.Visible)
            {
                dice[2] = r.Next(1, 7);
                die3Roll.Source = new BitmapImage(new Uri(getDieImage(dice[2]), UriKind.Absolute));
                rolledDice.Add(dice[2]);
            }
            if (die4Roll.Visibility == Visibility.Visible)
            {
                dice[3] = r.Next(1, 7);
                die4Roll.Source = new BitmapImage(new Uri(getDieImage(dice[3]), UriKind.Absolute));
                rolledDice.Add(dice[3]);
            }
            if (die5Roll.Visibility == Visibility.Visible)
            {
                dice[4] = r.Next(1, 7);
                die5Roll.Source = new BitmapImage(new Uri(getDieImage(dice[4]), UriKind.Absolute));
                rolledDice.Add(dice[4]);
            }
            if (die6Roll.Visibility == Visibility.Visible)
            {
                dice[5] = r.Next(1, 7);
                die6Roll.Source = new BitmapImage(new Uri(getDieImage(dice[5]), UriKind.Absolute));
                rolledDice.Add(dice[5]);
            }

            // Check if the rolled dice is a farkle
            if (checkDice(rolledDice) == 0)
            {
                endTurn.IsEnabled = false;
                keep.IsEnabled = false;
                rules.IsEnabled = false;
                currentTotal.Text = "0";
                currentPreviousTotal.Text = players[currentPlayer].TotalScore.ToString();
                farkleText.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                farkleText.Visibility = Visibility.Collapsed;
                endTurn_Click(this, new RoutedEventArgs());
                endTurn.IsEnabled = true;
                rules.IsEnabled = true;
            }
            else
            {
                arrow1.Visibility = Visibility.Visible;
                arrow2.Visibility = Visibility.Visible;
            }
        }

        // Keep the dice that have been clicked and save score accordingly
        private async void keep_Click(object sender, RoutedEventArgs e)
        {
            List<int> keptDice = new List<int>(); // List of dice that were chosen to be kept
            bool keepIsValid = true;              // The dice that were kept were a valid score
            bool allDiceKept = true;              // All of the dice have been kept
            int keepScore;                        // Score for the current keep

            // Add each of the keep die to the list of kept dice
            if (die1Keep.Visibility == Visibility.Visible)
                keptDice.Add(dice[0]);
            if (die2Keep.Visibility == Visibility.Visible)
                keptDice.Add(dice[1]);
            if (die3Keep.Visibility == Visibility.Visible)
                keptDice.Add(dice[2]);
            if (die4Keep.Visibility == Visibility.Visible)
                keptDice.Add(dice[3]);
            if (die5Keep.Visibility == Visibility.Visible)
                keptDice.Add(dice[4]);
            if (die6Keep.Visibility == Visibility.Visible)
                keptDice.Add(dice[5]);

            previousTotal.Text = players[currentPlayer].TotalScore.ToString();
            keepScore = checkDice(keptDice);

            // Check if the kept dice are valid scoring
            for (int i = 0; i < 6; i++)
                if (dieCounts[i] != 0)
                    keepIsValid = false;

            // 
            if (keepIsValid && keepScore > 0)
            {
                currentTotal.Text = (keepScore + int.Parse(currentTotal.Text)).ToString();
                currentPreviousTotal.Text = (int.Parse(currentTotal.Text) + players[currentPlayer].TotalScore).ToString();
                keep.IsEnabled = false;
                roll.IsEnabled = true;

                // Reset all of the keep dice
                if (die1Keep.Visibility == Visibility.Visible)
                {
                    dice[0] = 0;
                    die1Keep.Visibility = Visibility.Collapsed;
                }
                if (die2Keep.Visibility == Visibility.Visible)
                {
                    dice[1] = 0;
                    die2Keep.Visibility = Visibility.Collapsed;
                }
                if (die3Keep.Visibility == Visibility.Visible)
                {
                    dice[2] = 0;
                    die3Keep.Visibility = Visibility.Collapsed;
                }
                if (die4Keep.Visibility == Visibility.Visible)
                {
                    dice[3] = 0;
                    die4Keep.Visibility = Visibility.Collapsed;
                }
                if (die5Keep.Visibility == Visibility.Visible)
                {
                    dice[4] = 0;
                    die5Keep.Visibility = Visibility.Collapsed;
                }
                if (die6Keep.Visibility == Visibility.Visible)
                {
                    dice[5] = 0;
                    die6Keep.Visibility = Visibility.Collapsed;
                }

                // Check if all of the dice have been kept
                for (int i = 0; i < 6; i++)
                    if (dice[i] != 0)
                        allDiceKept = false;

                // Set up for a fresh roll of dice
                if (allDiceKept)
                {
                    die1Roll.Source = new BitmapImage(new Uri(getDieImage(dice[0]), UriKind.Absolute));
                    die2Roll.Source = new BitmapImage(new Uri(getDieImage(dice[1]), UriKind.Absolute));
                    die3Roll.Source = new BitmapImage(new Uri(getDieImage(dice[2]), UriKind.Absolute));
                    die4Roll.Source = new BitmapImage(new Uri(getDieImage(dice[3]), UriKind.Absolute));
                    die5Roll.Source = new BitmapImage(new Uri(getDieImage(dice[4]), UriKind.Absolute));
                    die6Roll.Source = new BitmapImage(new Uri(getDieImage(dice[5]), UriKind.Absolute));
                    die1Roll.Visibility = Visibility.Visible;
                    die2Roll.Visibility = Visibility.Visible;
                    die3Roll.Visibility = Visibility.Visible;
                    die4Roll.Visibility = Visibility.Visible;
                    die5Roll.Visibility = Visibility.Visible;
                    die6Roll.Visibility = Visibility.Visible;
                }

                // Reset all of the roll dice
                if (die1Roll.Visibility == Visibility.Visible)
                {
                    dice[0] = 0;
                    die1Roll.Source = new BitmapImage(new Uri(getDieImage(dice[0]), UriKind.Absolute));
                }
                if (die2Roll.Visibility == Visibility.Visible)
                {
                    dice[1] = 0;
                    die2Roll.Source = new BitmapImage(new Uri(getDieImage(dice[1]), UriKind.Absolute));
                }
                if (die3Roll.Visibility == Visibility.Visible)
                {
                    dice[2] = 0;
                    die3Roll.Source = new BitmapImage(new Uri(getDieImage(dice[2]), UriKind.Absolute));
                }
                if (die4Roll.Visibility == Visibility.Visible)
                {
                    dice[3] = 0;
                    die4Roll.Source = new BitmapImage(new Uri(getDieImage(dice[3]), UriKind.Absolute));
                }
                if (die5Roll.Visibility == Visibility.Visible)
                {
                    dice[4] = 0;
                    die5Roll.Source = new BitmapImage(new Uri(getDieImage(dice[4]), UriKind.Absolute));
                }
                if (die6Roll.Visibility == Visibility.Visible)
                {
                    dice[5] = 0;
                    die6Roll.Source = new BitmapImage(new Uri(getDieImage(dice[5]), UriKind.Absolute));
                }
                arrow1.Visibility = Visibility.Collapsed;
                arrow2.Visibility = Visibility.Collapsed;
            }
            else
            {
                notValidKeep.Visibility = Visibility.Visible;
                await Task.Delay(3000);
                notValidKeep.Visibility = Visibility.Collapsed;
            }
        }

        // Get the image of a die 1-6
        public string getDieImage(int die)
        {
            string image;

            switch (die)
            {
                case 1:
                    image = "ms-appx://Snarkle/Assets/die1.png";
                    break;
                case 2:
                    image = "ms-appx://Snarkle/Assets/die2.png";
                    break;
                case 3:
                    image = "ms-appx://Snarkle/Assets/die3.png";
                    break;
                case 4:
                    image = "ms-appx://Snarkle/Assets/die4.png";
                    break;
                case 5:
                    image = "ms-appx://Snarkle/Assets/die5.png";
                    break;
                case 6:
                    image = "ms-appx://Snarkle/Assets/die6.png";
                    break;
                default:
                    image = "ms-appx://Snarkle/Assets/dieX.png";
                    break;
            }
            return image;
        }

        #region PointerPressed Die Click Methods
        private void die1_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (dice[0] != 0)
            {
                die1Roll.Visibility = Visibility.Collapsed;
                die1Keep.Source = die1Roll.Source;
                die1Keep.Visibility = Visibility.Visible;
            }
        }
        private void die2_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (dice[1] != 0)
            {
                die2Roll.Visibility = Visibility.Collapsed;
                die2Keep.Source = die2Roll.Source;
                die2Keep.Visibility = Visibility.Visible;
            }
        }
        private void die3_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (dice[2] != 0)
            {
                die3Roll.Visibility = Visibility.Collapsed;
                die3Keep.Source = die3Roll.Source;
                die3Keep.Visibility = Visibility.Visible;
            }
        }
        private void die4_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (dice[3] != 0)
            {
                die4Roll.Visibility = Visibility.Collapsed;
                die4Keep.Source = die4Roll.Source;
                die4Keep.Visibility = Visibility.Visible;
            }
        }
        private void die5_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (dice[4] != 0)
            {
                die5Roll.Visibility = Visibility.Collapsed;
                die5Keep.Source = die5Roll.Source;
                die5Keep.Visibility = Visibility.Visible;
            }
        }
        private void die6_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (dice[5] != 0)
            {
                die6Roll.Visibility = Visibility.Collapsed;
                die6Keep.Source = die6Roll.Source;
                die6Keep.Visibility = Visibility.Visible;
            }
        }
        private void die1Keep_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            die1Roll.Visibility = Visibility.Visible;
            die1Roll.Source = die1Keep.Source;
            die1Keep.Visibility = Visibility.Collapsed;
        }
        private void die2Keep_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            die2Roll.Visibility = Visibility.Visible;
            die2Roll.Source = die2Keep.Source;
            die2Keep.Visibility = Visibility.Collapsed;
        }
        private void die3Keep_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            die3Roll.Visibility = Visibility.Visible;
            die3Roll.Source = die3Keep.Source;
            die3Keep.Visibility = Visibility.Collapsed;
        }
        private void die4Keep_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            die4Roll.Visibility = Visibility.Visible;
            die4Roll.Source = die4Keep.Source;
            die4Keep.Visibility = Visibility.Collapsed;
        }
        private void die5Keep_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            die5Roll.Visibility = Visibility.Visible;
            die5Roll.Source = die5Keep.Source;
            die5Keep.Visibility = Visibility.Collapsed;
        }
        private void die6Keep_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            die6Roll.Visibility = Visibility.Visible;
            die6Roll.Source = die6Keep.Source;
            die6Keep.Visibility = Visibility.Collapsed;
        }
        #endregion

        // End the current turn, save the current players score, check if a player won, and reset the game board for the next player
        private void endTurn_Click(object sender, RoutedEventArgs e)
        {
            roll.IsEnabled = true;
            keep.IsEnabled = false;

            // Update the current players score with their round total and display it
            players[currentPlayer].TotalScore += int.Parse(currentTotal.Text);
            switch (currentPlayer)
            {
                case 0: player1Score.Text = players[currentPlayer].TotalScore.ToString();
                    break;
                case 1: player2Score.Text = players[currentPlayer].TotalScore.ToString();
                    break;
                case 2: player3Score.Text = players[currentPlayer].TotalScore.ToString();
                    break;
                case 3: player4Score.Text = players[currentPlayer].TotalScore.ToString();
                    break;
            }
            player1Marker.Visibility = Visibility.Collapsed;
            player2Marker.Visibility = Visibility.Collapsed;
            player3Marker.Visibility = Visibility.Collapsed;
            player4Marker.Visibility = Visibility.Collapsed;

            // Reset the die counts and the dice to 0 and reset the die images for the next player
            for (int i = 0; i < 6; i++)
            {
                dice[i] = 0;
                dieCounts[i] = 0;
            }
            die1Roll.Source = new BitmapImage(new Uri(getDieImage(dice[0]), UriKind.Absolute));
            die2Roll.Source = new BitmapImage(new Uri(getDieImage(dice[1]), UriKind.Absolute));
            die3Roll.Source = new BitmapImage(new Uri(getDieImage(dice[2]), UriKind.Absolute));
            die4Roll.Source = new BitmapImage(new Uri(getDieImage(dice[3]), UriKind.Absolute));
            die5Roll.Source = new BitmapImage(new Uri(getDieImage(dice[4]), UriKind.Absolute));
            die6Roll.Source = new BitmapImage(new Uri(getDieImage(dice[5]), UriKind.Absolute));
            die1Roll.Visibility = Visibility.Visible;
            die2Roll.Visibility = Visibility.Visible;
            die3Roll.Visibility = Visibility.Visible;
            die4Roll.Visibility = Visibility.Visible;
            die5Roll.Visibility = Visibility.Visible;
            die6Roll.Visibility = Visibility.Visible;
            die1Keep.Visibility = Visibility.Collapsed;
            die2Keep.Visibility = Visibility.Collapsed;
            die3Keep.Visibility = Visibility.Collapsed;
            die4Keep.Visibility = Visibility.Collapsed;
            die5Keep.Visibility = Visibility.Collapsed;
            die6Keep.Visibility = Visibility.Collapsed;

            // Determine if someone has won the game or not
            if ((currentPlayer + 1) == players.Count)
            {
                Player topPlayer = players[0];
                for (int i = 1; i < players.Count; i++ )
                    if (topPlayer.TotalScore < players[i].TotalScore)
                        topPlayer = players[i];
                if (topPlayer.TotalScore >= 5000)
                    finishGame(topPlayer);
            }

            // Set up the on screen information for the next player
            currentPlayer = (currentPlayer + 1) % players.Count;
            previousTotal.Text = players[currentPlayer].TotalScore.ToString();
            currentTotal.Text = "0";
            currentPreviousTotal.Text = players[currentPlayer].TotalScore.ToString();
            arrow1.Visibility = Visibility.Collapsed;
            arrow2.Visibility = Visibility.Collapsed;

            // Indicate which player's turn it is using a marker
            switch (currentPlayer)
            {
                case 0:
                    player1Marker.Visibility = Visibility.Visible;
                    break;
                case 1:
                    player2Marker.Visibility = Visibility.Visible;
                    break;
                case 2:
                    player3Marker.Visibility = Visibility.Visible;
                    break;
                case 3:
                    player4Marker.Visibility = Visibility.Visible;
                    break;
            }
        }

        // Display the winning player and their score
        private void finishGame(Player topPlayer)
        {
            winningPlayer.Text = topPlayer.Name;
            winningScore.Text = topPlayer.TotalScore.ToString();
            endGame.Visibility = Visibility.Visible;
            winningBorder.Visibility = Visibility.Visible;
            winningPlayer.Visibility = Visibility.Visible;
            winningScore.Visibility = Visibility.Visible;
            winningText1.Visibility = Visibility.Visible;
            winningText2.Visibility = Visibility.Visible;
            roll.IsEnabled = false;
            endTurn.IsEnabled = false;
        }

        // Return to the main welcome page
        private void endGame_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        // Display the rules of the game on screen
        private void rules_Click(object sender, RoutedEventArgs e)
        {
            rulesGrid.Visibility = Visibility.Visible;
            roll.IsEnabled = false;
            endTurn.IsEnabled = false;
            keep.IsEnabled = false;
            rules.IsEnabled = false;
        }

        // Collapse the rules on screen and return to gameplay
        private void closeRules_Click(object sender, RoutedEventArgs e)
        {
            rulesGrid.Visibility = Visibility.Collapsed;
            roll.IsEnabled = true;
            endTurn.IsEnabled = true;
            keep.IsEnabled = true;
            rules.IsEnabled = true;
        }
    }
}
