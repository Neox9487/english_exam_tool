using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiteDB;

namespace EnglishApp.Views
{
    public partial class WordsPage : Page
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<WordItem> _wordsCollection;
        private readonly ILiteCollection<TagItem> _tagsCollection;

        // Search
        private List<string> _tags = new List<string> { };
        private List<string> _sort_options = new List<string> { "A-Z", "Z-A", "POS", "Tags"};
        private string _keyword;

        public WordsPage()
        {
            InitializeComponent();
            _db = new LiteDatabase(@"Filename=database.db;Connection=shared");
            _wordsCollection = _db.GetCollection<WordItem>("words");
            _tagsCollection = _db.GetCollection<TagItem>("tags");

            SortComboBox.ItemsSource = _sort_options;
            SortComboBox.SelectedIndex = 0;

            LoadTags();
            LoadWords();
        }

        // Update UI
        private void LoadTags()
        {
            _tags = new List<string> { "All" };

            var dbTags = _tagsCollection.FindAll()
                         .Select(t => t.Tag)
                         .Distinct()
                         .OrderBy(t => t)
                         .ToList();

            _tags.AddRange(dbTags);

            TagFilterComboBox.ItemsSource = _tags;
            TagFilterComboBox.SelectedIndex = 0;
        }

        private void LoadWords()
        {
            string selectedTag = TagFilterComboBox.SelectedItem as string ?? "All";
            string sortOption = SortComboBox.SelectedItem as string ?? "A-Z";

            var query = _wordsCollection.FindAll();

            if (selectedTag != "All")
            {
                query = query.Where(w => w.Tags != null && w.Tags.Contains(selectedTag));
            }

            if (!string.IsNullOrEmpty(_keyword))
            {
                query = query.Where(w =>
                    (!string.IsNullOrEmpty(w.Word) && w.Word.Contains(_keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(w.Meaning) && w.Meaning.Contains(_keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (w.Tags != null && w.Tags.Any(t => t.Contains(_keyword, StringComparison.OrdinalIgnoreCase)))
                );
            }

            List<WordItem> wordList = query.ToList();

            switch (sortOption)
            {
                case "A-Z":
                    wordList = wordList.OrderBy(w => w.Word).ToList();
                    break;
                case "Z-A":
                    wordList = wordList.OrderByDescending(w => w.Word).ToList();
                    break;
                case "POS":
                    wordList = wordList.OrderBy(w => w.POS).ToList();
                    break;
                case "Tag":
                    wordList = wordList.OrderBy(w => w.Tags).ToList();
                    break;
                default:
                    break;
            }
            
            WordsDataGrid.ItemsSource = wordList;
        }

        // Button actions
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            _keyword = SearchTextBox.Text.Trim();
            LoadWords();
        }

        private void AddWordButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddWordPage());
        }
        
        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTagPage());
        }

        private void BackToMenu_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }

    public class WordItem
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public string POS { get; set; }
        public List<string> Tags { get; set; }
    }

    public class TagItem
    {
        public int Id { get; set; }
        public string Tag { get; set; }
    }
}