using MedicalCard1.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class ViewAppointmentControl : UserControl
    {
        private readonly AppContext _context;
        private readonly Appointment _appointment;
        public ViewAppointmentControl(AppContext context, Appointment appointment)
        {
            InitializeComponent();
            _context = context;
            _appointment = appointment;
            DataContext = appointment;
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
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Вы точно хотите удалить запись от {_appointment.VisitData:dd/MM/yyyy}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                using (var localContext = new AppContext())
                {
                    var existingAppointment = localContext.Appointments.Find(_appointment.Id);
                    if (existingAppointment != null)
                    {
                        localContext.Appointments.Remove(existingAppointment);
                        localContext.SaveChanges();

                        MessageBox.Show("Запись успешно удалена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Запись не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                var contentPresenter = GetParentContentPresenter();
                if (contentPresenter != null)
                {
                    contentPresenter.Content = null;
                }
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editControl = new EditAppointmentControl(_context, _appointment);
            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = editControl;
            }
        }
    }
}
