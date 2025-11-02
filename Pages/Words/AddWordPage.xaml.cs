using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiteDB;

namespace EnglishApp.Views
{
    public partial class AddWordPage : Page
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<TagItem> _tagsCollection;
        private readonly ILiteCollection<WordItem> _wordsCollection;
        private readonly List<string> POS = new List<string> { "Noun", "Pronoun", "Verb", "Adjective", "Adverb", "Preposition", "Conjunction", "Interjection" };
        private List<string> Tags = new List<string> { "-" };
        public AddWordPage()
        {
            InitializeComponent();
            _db = new LiteDatabase(@"Filename=database.db;Connection=shared");
            _tagsCollection = _db.GetCollection<TagItem>("tags");
            _wordsCollection = _db.GetCollection<WordItem>("words");

            POSComboBox.ItemsSource = POS;
            POSComboBox.SelectedIndex = 0;

            Tags.AddRange(
                _tagsCollection.FindAll()
                .ToList()
                .Select(t => t.Tag)
                .ToList()
            );
            TagsComboBox.ItemsSource = Tags;
            TagsComboBox.SelectedIndex = 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var new_word = WordTextBox.Text.Trim();
            var meaning = MeaningTextBox.Text.Trim();
            var pos = POSComboBox.SelectedItem as string;
            var tag = TagsComboBox.SelectedItem as string;

            _wordsCollection.Insert(
                new WordItem
                {
                    Word = new_word,
                    Meaning = meaning,
                    POS = pos,
                    Tag = tag
                }
            );
            
            MessageBox.Show("Add a new word sucessfully!");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}