using System.Windows;
using System.Windows.Controls;
using LiteDB;

namespace EnglishApp.Views
{
    public partial class AddTagPage : Page
    {
        private readonly LiteDatabase _db;

        public AddTagPage()
        {
            InitializeComponent();
            _db = new LiteDatabase(@"Filename=database.db;Connection=shared");
        }

        // Update UI


        // button actions
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}