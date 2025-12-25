using MedicalCard1.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class AddUserControl : UserControl
    {
        public AddUserControl()
        {
            InitializeComponent();
            PopulateRolesComboBox();
        }
        private void PopulateRolesComboBox()
        {
            using (var context = new AppContext())
            {
                var roles = context.Roles.Where(role => role.Name != "Супер-Администратор").ToList();
                foreach (var comboBox in FindAllChildrenOfType<ComboBox>(RootGrid))
                {
                    comboBox.ItemsSource = roles;
                    comboBox.DisplayMemberPath = "Name";
                    comboBox.SelectedValuePath = "Id";
                }
            }
        }
        private IEnumerable<T> FindAllChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; ++i)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    yield return typedChild;
                foreach (T other in FindAllChildrenOfType<T>(child))
                    yield return other;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text.Trim();
            string lastName = LastNameTextBox.Text.Trim();
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password;
            int roleId = RolesComboBox.SelectedIndex >= 0 ? (int)RolesComboBox.SelectedValue : -1;
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                roleId <= 0)
            {
                MessageBox.Show("Заполните все поля и выберите роль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new AppContext())
            {
                bool existingUser = context.Users.Any(u => u.Login == login);

                if (existingUser)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var newUser = new User
                {
                    Name = name,
                    LastName = lastName,
                    Login = login,
                    Password = password,
                    RoleId = roleId
                };

                context.Users.Add(newUser);
                context.SaveChanges();

                MessageBox.Show("Пользователь успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = null;
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
