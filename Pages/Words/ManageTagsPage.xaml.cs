using System.Windows;
using System.Windows.Controls;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace EnglishApp.Views
{
    public partial class ManageTagsPage : Page
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<TagItem> _tagsCollection;

        public ManageTagsPage()
        {
            InitializeComponent();
            _db = new LiteDatabase(@"Filename=database.db;Connection=shared");
            _tagsCollection = _db.GetCollection<TagItem>("tags");

            LoadTags();
        }

        private void LoadTags()
        {
            var tags = _tagsCollection.FindAll().OrderBy(t => t.Tag).ToList();
            TagListBox.ItemsSource = tags;
        }

        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            var newTag = NewTagTextBox.Text.Trim();

            if (string.IsNullOrEmpty(newTag))
            {
                MessageBox.Show("Tag cannot be empty!");
                return;
            }

            var exists = _tagsCollection.FindOne(t => t.Tag.Equals(newTag, System.StringComparison.OrdinalIgnoreCase));
            if (exists != null)
            {
                MessageBox.Show("Tag already exists!");
                return;
            }

            var tagItem = new TagItem { Tag = newTag };
            _tagsCollection.Insert(tagItem);

            MessageBox.Show("Tag added!");
            NewTagTextBox.Clear();
            LoadTags();
        }

        private void DeleteTagButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagListBox.SelectedItem is TagItem selectedTag)
            {
                if (MessageBox.Show($"Delete '{selectedTag.Tag}'?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _tagsCollection.Delete(selectedTag.Id);
                    LoadTags();
                }
            }
            else
            {
                MessageBox.Show("Please select a tag to delete.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
