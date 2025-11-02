using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using LiteDB;

namespace EnglishApp.Views
{
    public partial class AddWordPage : Page
    {
        private readonly LiteDatabase _db;

        private readonly List<string> POS = new List<string> {"Noun", "Pronoun", "Verb", "Adjective", "Adverb", "Preposition", "Conjunction", "Interjection"};

        public AddWordPage()
        {
            InitializeComponent();
            _db = new LiteDatabase(@"Filename=database.db;Connection=shared");

            POSComboBox.ItemsSource = POS;
            POSComboBox.SelectedIndex = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}