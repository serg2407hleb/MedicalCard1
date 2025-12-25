using MedicalCard1.Controls;
using MedicalCard1.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;

namespace MedicalCard1.Windows
{
    public partial class DoctorWindow : Window
    {
        private readonly AppContext _context;
        public User CurrentDoctor { get; set; }
        public DoctorWindow(User user)
        {
            InitializeComponent();
            _context = new AppContext();
            Loaded += async (sender, args) => await LoadPatients();
            Loaded += async (sender, args) => await LoadAppointments();
            CurrentDoctor = user;
            VisitListView.SelectionChanged += VisitListView_SelectionChanged;
        }
        public async Task LoadPatients()
        {
            var patients = await _context.Patients.OrderBy(p => p.LastName).ThenBy(p => p.Name).ToListAsync();
            PatientListView.ItemsSource = patients;
        }
        public async Task LoadAppointments()
        {
            var appointments = await _context.Appointments
                                 .Include(a => a.Patient)
                                 .Include(a => a.Doctor)
                                 .OrderBy(a => a.VisitData)
                                 .ThenBy(a => a.Patient.Name)
                                 .ToListAsync();

            VisitListView.ItemsSource = appointments;
        }
        private async void OpenAddPatientForm_Click(object sender, RoutedEventArgs e)
        {
            var addPatientControl = new Controls.AddPatientControl(() =>
            {
                Dispatcher.BeginInvoke((Action)(async () => await LoadPatients()));
            });
            RightDocPanelContent.Content = addPatientControl;
        }
        private async void PatientListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PatientListView.SelectedItem is not Patient selectedPatient)
                return;
            VisitListView.UnselectAll();
            RightDocPanelContent.Content = null;
            var infoControl = new PatientInfoControl(_context, CurrentDoctor);
            infoControl.PatientRemoved += async () => await LoadPatients();
            RightDocPanelContent.Content = infoControl;
            infoControl.DataContext = selectedPatient;
            var visits = await _context.Appointments
                                       .Include(v => v.Doctor)
                                       .Where(v => v.Patient.Id == selectedPatient.Id)
                                       .OrderByDescending(v => v.VisitData)
                                       .ToListAsync();
            VisitListView.ItemsSource = visits;
        }
        private void VisitListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VisitListView.SelectedItem is Appointment selectedVisit)
            {
                var viewRecordControl = new ViewAppointmentControl(_context, selectedVisit);
                RightDocPanelContent.Content = viewRecordControl;
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
    }
}
