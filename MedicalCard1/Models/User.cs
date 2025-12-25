namespace MedicalCard1.Models
{
    public class User
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName} {Name}";
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual Role Role { get; set; }
        public int RoleId { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
