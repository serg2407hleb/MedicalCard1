using MedicalCard1.Controls;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;

namespace MedicalCard1.Windows
{
    public partial class SuperAdminWindow : Window
    {
        public SuperAdminWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            NewUserBut.Click += NewUserBut_Click;
            DoctorsBut.Click += DoctorsBut_Click;
            AdminsBut.Click += AdminsBut_Click;
        }
        private void NewUserBut_Click(object sender, RoutedEventArgs e)
        {
            RightPanelContent.Content = new AddUserControl();
        }
        private async void DoctorsBut_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppContext())
            {
                var doctors = await context.Users
                                           .Include(u => u.Role)
                                           .Where(u => u.Role.Name == "Врач")
                                           .ToListAsync();
                UsersListView.ItemsSource = doctors;
            }
        }
        private async void AdminsBut_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppContext())
            {
                var admins = await context.Users
                                          .Include(u => u.Role)
                                          .Where(u => u.Role.Name == "Администратор")
                                          .ToListAsync();

                UsersListView.ItemsSource = admins;
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы действительно хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }
        private void UsersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = UsersListView.SelectedItem as dynamic;

            if (selectedItem != null)
            {
                RightPanelContent.Content = new ProfileControl(selectedItem);
            }
        }
    }
}
