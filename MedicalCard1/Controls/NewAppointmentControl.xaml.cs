using MedicalCard1.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class NewAppointmentControl : UserControl
    {
        private readonly AppContext _context;
        private readonly Patient _selectedPatient;
        private readonly User _doctor;
        public NewAppointmentControl(AppContext context, Patient patient, User doctor)
        {
            InitializeComponent();
            _context = context;
            _selectedPatient = patient;
            _doctor = doctor;
            DataContext = this;
        }
        public string PatientFullName => _selectedPatient.FullName;
        public string DoctorFullName => _doctor.FullName;
        private Appointment Record { get; set; } = new Appointment();
        public DateTime VisitData
        {
            get => Record.VisitData;
            set => Record.VisitData = value;
        }
        public string Complaints
        {
            get => Record.Complaints;
            set => Record.Complaints = value;
        }
        public string Anamnesis
        {
            get => Record.Anamnesis;
            set => Record.Anamnesis = value;
        }
        public string Diagnosis
        {
            get => Record.Diagnosis;
            set => Record.Diagnosis = value;
        }
        public string DoctorsOrder
        {
            get => Record.DoctorsOrder;
            set => Record.DoctorsOrder = value;
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(Complaints))
            {
                MessageBox.Show("Введите жалобу пациента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            if (string.IsNullOrEmpty(Anamnesis))
            {
                MessageBox.Show("Введите анамнез заболевания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            if (string.IsNullOrEmpty(Diagnosis))
            {
                MessageBox.Show("Введите диагноз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            if (string.IsNullOrEmpty(DoctorsOrder))
            {
                MessageBox.Show("Введите назначение врача.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                isValid = false;
            }
            if (!isValid)
                return;
            Record.PatientId = _selectedPatient.Id;
            Record.DoctorId = _doctor.Id;
            _context.Appointments.Add(Record);
            await _context.SaveChangesAsync();
            MessageBox.Show("Запись успешно сохранена.");
            var parentContentPresenter = GetParentContentPresenter();
            if (parentContentPresenter != null)
            {
                parentContentPresenter.Content = null;
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            var parentContentPresenter = GetParentContentPresenter();
            if (parentContentPresenter != null)
            {
                parentContentPresenter.Content = null;
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
