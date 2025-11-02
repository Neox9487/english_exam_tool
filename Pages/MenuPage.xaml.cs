using System.Windows;
using System.Windows.Controls;

namespace EnglishApp.Views
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Quiz_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizPage());
        }

        private void Words_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WordsPage());
        }
    }
}