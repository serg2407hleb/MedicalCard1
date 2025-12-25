using MedicalCard1.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class ProfileControl : UserControl
    {
        private readonly User _selectedUser;
        public ProfileControl(User selectedUser)
        {
            InitializeComponent();
            _selectedUser = selectedUser;
            FullNameTextBlock.Text = selectedUser.FullName;
            LoginTextBlock.Text = selectedUser.Login;
            PasswordTextBlock.Text = selectedUser.Password;
            RoleTextBlock.Text = selectedUser.Role.Name;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пользователя \"{_selectedUser.FullName}\" ?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            if (result == MessageBoxResult.Yes)
            {
                using (var context = new AppContext())
                {
                    var userToDelete = context.Users.Find(_selectedUser.Id);
                    if (userToDelete != null)
                    {
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();
                        MessageBox.Show("Пользователь успешно удалён.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        var contentPresenter = GetParentContentPresenter();
                        if (contentPresenter != null)
                        {
                            contentPresenter.Content = null;
                        }
                    }
                }
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
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
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editControl = new EditUserControl(_selectedUser);
            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = editControl;
            }
        }
    }
}
