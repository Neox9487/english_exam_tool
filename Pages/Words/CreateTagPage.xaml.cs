using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using LiteDB;
using System.Linq;

namespace EnglishApp.Views
{
    public partial class CreateTagPage : Page
    {
        private readonly LiteDatabase _db;
        private readonly ILiteCollection<TagItem> _tagsCollection;
        private List<string> _tags = new List<string> { };
        public CreateTagPage()
        {
            InitializeComponent();
            _db = new LiteDatabase(@"Filename=database.db;Connection=shared");
            _tagsCollection = _db.GetCollection<TagItem>("tags");
        }

        // button actions
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var new_tag = TagTextBox.Text.Trim();
            var is_exist = _tagsCollection.FindOne(t => t.Tag.Equals(new_tag, System.StringComparison.OrdinalIgnoreCase));
            if (is_exist != null)
            {
                MessageBox.Show("Tag is already existed!");
                return;
            }

            _tagsCollection.Insert(
                new TagItem
                {
                    Tag = new_tag
                }
            );
            MessageBox.Show("Create a new tag sucessfully!");
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}