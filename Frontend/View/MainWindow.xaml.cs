using Frontend.Model;
using Frontend.View;
using Frontend.ViewModel;
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

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public UserViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new UserViewModel();
            this.viewModel = (UserViewModel)DataContext;
        }


        public void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                UserBoardsView boardView = new UserBoardsView(u);
                boardView.Show();
                this.Close();
            }
        }

        public void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Register();
            if (u != null)
            {
                UserBoardsView boardView = new UserBoardsView(u);
                boardView.Show();
                this.Close();
            }
        }
    }
}
