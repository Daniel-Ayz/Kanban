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
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardTasksView.xaml
    /// </summary>
    public partial class BoardTasksView : Window
    {
        private BoardTasksViewModel viewModel;
        private UserModel user;
        public BoardTasksView(UserModel user, BoardModel board)
        {
            InitializeComponent();
            this.user = user;
            this.viewModel = new BoardTasksViewModel(user, board);
            this.DataContext = viewModel;
        }

        public void Back_Click(object sender, RoutedEventArgs e)
        {
            UserBoardsView boardView = new UserBoardsView(user);
            boardView.Show();
            this.Close();
        }
    }
}
