using MedicalCard1.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class EditUserControl : UserControl
    {
        private readonly User _originalUser;
        public EditUserControl(User originalUser)
        {
            InitializeComponent();
            _originalUser = originalUser;
            NameTextBox.Text = originalUser.Name;
            LastNameTextBox.Text = originalUser.LastName;
            LoginTextBox.Text = originalUser.Login;
            PasswordBox.Password = originalUser.Password;
            using (var context = new AppContext())
            {
                var roles = context.Roles.Where(role => role.Name != "Супер-Администратор").ToList();
                RolesComboBox.ItemsSource = roles;
                RolesComboBox.DisplayMemberPath = "Name";
                RolesComboBox.SelectedValuePath = "Id";
                RolesComboBox.SelectedValue = originalUser.RoleId;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;
            int roleId = (int)RolesComboBox.SelectedValue;
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                RolesComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Все поля обязательны для заполнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (var context = new AppContext())
            {
                bool duplicateLoginExists = context.Users.Any(u => u.Login == login && u.Id != _originalUser.Id);
                if (duplicateLoginExists)
                {
                    MessageBox.Show("Такой логин уже используется другим пользователем.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var updatedUser = context.Users.Find(_originalUser.Id);
                if (updatedUser != null)
                {
                    updatedUser.Name = name;
                    updatedUser.LastName = lastName;
                    updatedUser.Login = login;
                    updatedUser.Password = password;
                    updatedUser.RoleId = roleId;
                    context.SaveChanges();
                    MessageBox.Show("Данные пользователя успешно обновлены.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    var contentPresenter = GetParentContentPresenter();
                    if (contentPresenter != null)
                    {
                        contentPresenter.Content = null;
                    }
                }
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = null;
            }
        }
        private ContentPresenter GetParentContentPresenter()
        {
            DependencyObject current = this;
            while (current != null)
            {
                if (current is ContentPresenter presenter)
                    return presenter;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}
