using SwiperModeration.Pages;
using SwiperModeration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SwiperModeration
{
    /// <summary>
    /// Interaktionslogik für LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginBtnClick(object sender, RoutedEventArgs e)
        {
            APIService api = new();

            this.LogIn(email.Text, password.Text, (bool)rememberMe.IsChecked);
        }

        public async Task LogIn(string email, string password, bool rememberMe)
        {
            APIService api = new();
            var res = await api.LoginAsync(email, password, rememberMe);

            if (res is not null)
            {
                NavigationService.Navigate(new UserViewPage());
            }
        }
    }
}
