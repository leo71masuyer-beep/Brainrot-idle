using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Brainrot_idle.Ressources;

namespace Brainrot_idle.view
{
    public partial class MorpionFrame : Page
    {
        private readonly Random random = new();

        // Variable statique pour détecter le TOUT PREMIER lancement de ce mini-jeu
        private static bool estLePremierLancementMorpion = true;

        public MorpionFrame()
        {
            InitializeComponent();

            // Événement déclenché à l'affichage de la page
            Loaded += MorpionFrame_Loaded;

            UpdateScore();
        }

        private void MorpionFrame_Loaded(object sender, RoutedEventArgs e)
        {
            // Si c'est le tout premier lancement de la session
            if (estLePremierLancementMorpion)
            {
                MessageBox.Show(
                    "Ballerina Capuccina t'a défié pour jouer au morpion montre lui qui est le plus fin stratège\n",
                    "Comment Jouer ?",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                // Désactive l'affichage pour les prochaines fois
                estLePremierLancementMorpion = false;
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (!string.IsNullOrEmpty(button.Content?.ToString()))
                return;

            // Joueur
            button.Content = "X";

            if (CheckWinner("X"))
            {
                // Mise à jour directe du score global dans le GameState
                GameState.MeilleurScoreMorpion++;
                UpdateScore();

                MessageBox.Show("Vous avez gagné !");
                ResetBoard();
                return;
            }

            if (IsBoardFull())
            {
                MessageBox.Show("Match nul !");
                ResetBoard();
                return;
            }

            StatusText.Text = "Tour de l'IA";

            // IA
            PlayAI();

            if (CheckWinner("O"))
            {
                MessageBox.Show("L'IA a gagné !");
                ResetBoard();
                return;
            }

            if (IsBoardFull())
            {
                MessageBox.Show("Match nul !");
                ResetBoard();
                return;
            }

            StatusText.Text = "Votre tour (X)";
        }

        private void PlayAI()
        {
            // 1. L'IA tente de gagner
            if (TryPlayWinningMove("O"))
                return;

            // 2. L'IA tente de bloquer le joueur
            if (TryPlayWinningMove("X"))
                return;

            // 3. Sinon coup aléatoire
            List<Button> emptyButtons = new();

            foreach (UIElement element in GameGrid.Children)
            {
                if (element is Button button &&
                    string.IsNullOrEmpty(button.Content?.ToString()))
                {
                    emptyButtons.Add(button);
                }
            }

            if (emptyButtons.Count == 0)
                return;

            Button selected = emptyButtons[random.Next(emptyButtons.Count)];
            selected.Content = "O";
        }

        private bool TryPlayWinningMove(string symbol)
        {
            foreach (UIElement element in GameGrid.Children)
            {
                if (element is Button button &&
                    string.IsNullOrEmpty(button.Content?.ToString()))
                {
                    button.Content = symbol;

                    bool winningMove = CheckWinner(symbol);

                    button.Content = string.Empty;

                    if (winningMove)
                    {
                        button.Content = "O";
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckWinner(string player)
        {
            string[,] board = new string[3, 3];

            foreach (UIElement element in GameGrid.Children)
            {
                if (element is Button button)
                {
                    int row = Grid.GetRow(button);
                    int col = Grid.GetColumn(button);

                    board[row, col] = button.Content?.ToString();
                }
            }

            for (int i = 0; i < 3; i++)
            {
                // lignes
                if (board[i, 0] == player &&
                    board[i, 1] == player &&
                    board[i, 2] == player)
                    return true;

                // colonnes
                if (board[0, i] == player &&
                    board[1, i] == player &&
                    board[2, i] == player)
                    return true;
            }

            // diagonales
            if (board[0, 0] == player &&
                board[1, 1] == player &&
                board[2, 2] == player)
                return true;

            if (board[0, 2] == player &&
                board[1, 1] == player &&
                board[2, 0] == player)
                return true;

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (UIElement element in GameGrid.Children)
            {
                if (element is Button button &&
                    string.IsNullOrEmpty(button.Content?.ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        private void UpdateScore()
        {
            ScoreText.Text = $"Victoires : {GameState.MeilleurScoreMorpion}";
        }

        private void ResetBoard()
        {
            foreach (UIElement element in GameGrid.Children)
            {
                if (element is Button button)
                {
                    button.Content = string.Empty;
                    button.IsEnabled = true;
                }
            }

            StatusText.Text = "Votre tour (X)";
        }

        private void Return_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MiniGamesFrame());
        }
    }
}