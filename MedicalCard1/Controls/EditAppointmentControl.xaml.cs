using MedicalCard1.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class EditAppointmentControl : UserControl
    {
        private readonly AppContext _context;
        private readonly Appointment _appointment;
        public EditAppointmentControl(AppContext context, Appointment appointment)
        {
            InitializeComponent();
            _context = context;
            _appointment = appointment;
            DataContext = _appointment;
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_appointment.Complaints) ||
                string.IsNullOrEmpty(_appointment.Anamnesis) ||
                string.IsNullOrEmpty(_appointment.Diagnosis) ||
                string.IsNullOrEmpty(_appointment.DoctorsOrder))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                await _context.SaveChangesAsync();
                MessageBox.Show("Запись успешно обновлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                var contentPresenter = GetParentContentPresenter();
                if (contentPresenter != null)
                {
                    contentPresenter.Content = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
