using MedicalCard1.Models;
using MedicalCard1.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class EditPatientControl : UserControl
    {
        private readonly AppContext _context;
        private Patient _originalPatient;
        public EditPatientControl(AppContext context, Patient originalPatient)
        {
            InitializeComponent();
            _context = context;
            _originalPatient = originalPatient;
            FirstNameTextBox.Text = originalPatient.Name;
            LastNameTextBox.Text = originalPatient.LastName;
            PassportNumberTextBox.Text = originalPatient.Passport.ToString();
            BirthDatePicker.SelectedDate = originalPatient.Birth;
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _originalPatient.Name = FirstNameTextBox.Text.Trim();
                _originalPatient.LastName = LastNameTextBox.Text.Trim();
                _originalPatient.Passport = int.TryParse(PassportNumberTextBox.Text.Trim(), out var passport) ? passport : _originalPatient.Passport;
                _originalPatient.Birth = BirthDatePicker.SelectedDate.HasValue ? BirthDatePicker.SelectedDate.Value : _originalPatient.Birth;
                _context.Update(_originalPatient);
                await _context.SaveChangesAsync();
                MessageBox.Show("Данные пациента успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                var contentPresenter = GetParentContentPresenter();
                if (contentPresenter != null)
                {
                    contentPresenter.Content = null;
                }
                var window = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is DoctorWindow && w.IsActive);
                if (window is DoctorWindow doctorWindow)
                {
                    await doctorWindow.LoadPatients();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
