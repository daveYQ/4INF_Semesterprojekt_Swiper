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

namespace SwiperModeration.Pages
{
    /// <summary>
    /// Interaktionslogik für UserViewPage.xaml
    /// </summary>
    public partial class UserViewPage : Page
    {
        public UserViewPage()
        {
            InitializeComponent();

            this.InitializeData();
        }

        private async Task InitializeData()
        {
            APIService api = new();
            var userModDTOs = await api.GetUsersAsync();

            if (userModDTOs is not null)
            {
                listView.ItemsSource = userModDTOs;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BlockBtnClick(object sender, RoutedEventArgs e)
        {
            UserModDTO selectedItem = listView.SelectedItem as UserModDTO;

            this.BlockUser(selectedItem.Id);
        }

        private async Task BlockUser(string id)
        {
            APIService api = new();
            var res = await api.BlockUserAsync(id);

            if(res is not null)
            {
                await Console.Out.WriteLineAsync("suii");
            }
        }
    }
}
