using MedicalCard1.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class PatientInfoControl : UserControl
    {
        private readonly AppContext _context;
        private readonly User _currentDoctor;
        public PatientInfoControl(AppContext context, User currentDoctor)
        {
            InitializeComponent();
            _context = new AppContext();
            _context = context;
            _currentDoctor = currentDoctor;
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
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var patient = DataContext as Patient;
            if (patient == null)
                return;
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить пациента \"{patient.FullName}\"?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );
            if (result == MessageBoxResult.No)
                return;
            using (var context = new AppContext())
            {
                try
                {
                    var freshPatient = await context.Patients.FirstOrDefaultAsync(p => p.Id == patient.Id);

                    if (freshPatient == null)
                    {
                        MessageBox.Show("Пациент не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    context.Patients.Remove(freshPatient);
                    await context.SaveChangesAsync();
                    MessageBox.Show("Пациент успешно удалён.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnPatientRemoved();
                    var contentPresenter = GetParentContentPresenter();
                    if (contentPresenter != null)
                    {
                        contentPresenter.Content = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пациента: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public delegate void PatientRemovedEventHandler();
        public event PatientRemovedEventHandler PatientRemoved;
        protected virtual void OnPatientRemoved()
        {
            PatientRemoved?.Invoke();
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var patient = DataContext as Patient;
            if (patient == null)
                return;
            var editControl = new EditPatientControl(_context, patient);
            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = editControl;
            }
        }
        private void CreateNewAppointment_Click(object sender, RoutedEventArgs e)
        {
            var patient = DataContext as Patient;
            if (patient == null)
                return;
            var newRecordControl = new NewAppointmentControl(_context, patient, _currentDoctor);

            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = newRecordControl;
            }
        }
    }
}
