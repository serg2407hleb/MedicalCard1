using MedicalCard1.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MedicalCard1.Controls
{
    public partial class AddPatientControl : UserControl
    {
        private readonly AppContext _context;
        private readonly Action refreshCallback;
        public AddPatientControl(Action callback)
        {
            InitializeComponent();
            _context = new AppContext();
            refreshCallback = callback;
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool passportExists = await CheckIfPassportExists(PassportNumberTextBox.Text.Trim());
                if (!passportExists)
                {
                    var patient = new Patient
                    {
                        Name = FirstNameTextBox.Text.Trim(),
                        LastName = LastNameTextBox.Text.Trim(),
                        Passport = int.Parse(PassportNumberTextBox.Text.Trim()),
                        Birth = BirthDatePicker.SelectedDate.Value.Date
                    };
                    _context.Patients.Add(patient);
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Новый пациент успешно зарегистрирован.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    refreshCallback?.Invoke();
                    ClearForm();
                    var contentPresenter = GetParentContentPresenter();
                    if (contentPresenter != null)
                    {
                        contentPresenter.Content = null;
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь с указанным номером паспорта уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пациента: некорректно заполненные поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task<bool> CheckIfPassportExists(string passportNumber)
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Passport.ToString().Equals(passportNumber));
            return existingPatient != null;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
            var contentPresenter = GetParentContentPresenter();
            if (contentPresenter != null)
            {
                contentPresenter.Content = null;
            }
        }
        private void ClearForm()
        {
            FirstNameTextBox.Clear();
            LastNameTextBox.Clear();
            PassportNumberTextBox.Clear();
            BirthDatePicker.SelectedDate = null;
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
