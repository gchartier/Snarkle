// Authors: Gabriel Chartier and Jacob Stevens
// Date Started: 4.12.16
// Team Name: Snarkle

using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Snarkle
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Set the 1 Player option to true as a default choice
            players1.IsChecked = true;
        }

        private async void playFarkle_Click(object sender, RoutedEventArgs e)
        {
            List<string> names = new List<string>();
            int numPlayers = 0;

            // Get the names according to how many players are chosen
            if(players1.IsChecked == true)
            {
                getPlayer1Name(ref names, ref numPlayers);
            }
            if (players2.IsChecked == true)
            {
                getPlayer1Name(ref names, ref numPlayers);
                getPlayer2Name(ref names, ref numPlayers);
            }
            if (players3.IsChecked == true)
            {
                getPlayer1Name(ref names, ref numPlayers);
                getPlayer2Name(ref names, ref numPlayers);
                getPlayer3Name(ref names, ref numPlayers);
            }
            if (players4.IsChecked == true)
            {
                getPlayer1Name(ref names, ref numPlayers);
                getPlayer2Name(ref names, ref numPlayers);
                getPlayer3Name(ref names, ref numPlayers);
                getPlayer4Name(ref names, ref numPlayers);
            }

            // If all names that must be entered are entered
            if(names.Count == numPlayers)
            {
                // Navigate to the actual game page passing the list of names
                Frame.Navigate(typeof(GamePage), names);
            }
            else
            {
                // Empty the list of names and print an error message
                names.Clear();
                var dialog = new MessageDialog("You must enter a name for every player.");
                await dialog.ShowAsync();
            }
        }

        #region GetNameMethods
        public void getPlayer1Name(ref List<string> names, ref int numPlayers)
        {
            if (player1Name.Text != string.Empty)
                names.Add(player1Name.Text);
            numPlayers = 1;
        }
        public void getPlayer2Name(ref List<string> names, ref int numPlayers)
        {
            if (player2Name.Text != string.Empty)
                names.Add(player2Name.Text);
            numPlayers = 2;
        }
        public void getPlayer3Name(ref List<string> names, ref int numPlayers)
        {
            if (player3Name.Text != string.Empty)
                names.Add(player3Name.Text);
            numPlayers = 3;
        }
        public void getPlayer4Name(ref List<string> names, ref int numPlayers)
        {
            if (player4Name.Text != string.Empty)
                names.Add(player4Name.Text);
            numPlayers = 4;
        }
        #endregion
    }
}
