using MedicalCard1.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace MedicalCard1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void OnLoginClick(object sender, RoutedEventArgs e)
        {
            using var context = new AppContext();
            try
            {
                string login = LoginBox.Text.Trim();
                string password = PasswordBox.Password;

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }
                var user = await context.Users.Include(u => u.Role)
                                               .FirstOrDefaultAsync(u =>
                                               u.Login.ToLower().Equals(login.ToLower()) &&
                                               u.Password.Equals(password));
                if (user != null && user.Role != null)
                {
                    switch (user.Role.Name)
                    {
                        case "Супер-Администратор":
                            OpenSuperAdminWindow(user);
                            break;
                        case "Администратор":
                            OpenAdminWindow(user);
                            break;
                        case "Врач":
                            OpenDoctorWindow(user);
                            break;
                        default:
                            MessageBox.Show($"Ошибка: неизвестная роль '{user.Role.Name}'");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void OpenSuperAdminWindow(User user)
        {
            var adminWindow = new Windows.SuperAdminWindow();
            adminWindow.DataContext = user;
            adminWindow.Show();
            this.Close();
        }
        private void OpenAdminWindow(User user)
        {
            var adminWindow = new Windows.AdminWindow();
            adminWindow.DataContext = user;
            adminWindow.Show();
            this.Close();
        }
        private void OpenDoctorWindow(User user)
        {
            var doctorWindow = new Windows.DoctorWindow(user);
            doctorWindow.DataContext = user;
            doctorWindow.Show();
            this.Close();
        }
    }
}