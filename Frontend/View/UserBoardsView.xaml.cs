using Frontend.Model;
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
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for UserBoardsView.xaml
    /// </summary>
    public partial class UserBoardsView : Window
    {
        private UserBoardsViewModel viewModel;
        private UserModel user;

        internal UserBoardsView(UserModel user)
        {
            InitializeComponent();
            this.user = user;
            this.viewModel = new UserBoardsViewModel(user);
            this.DataContext = viewModel;
        }

        private void SelectBoard_Button(object sender, RoutedEventArgs e)
        {
            BoardModel board = viewModel.SelectBoard();
            if (board != null)
            {
                BoardTasksView boardView = new BoardTasksView(user, board);
                boardView.Show();
                this.Close();
            }
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

    }
}
