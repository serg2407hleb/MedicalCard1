namespace MedicalCard1.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Passport {  get; set; }
        public string FullName => $"{LastName} {Name}";
        public DateTime Birth { get; set; }
        public int Age => CalculateAge(Birth);
        public virtual ICollection<Appointment> Appointments { get; set; }
        private static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
                age--;
            return age;
        }
    }
}
