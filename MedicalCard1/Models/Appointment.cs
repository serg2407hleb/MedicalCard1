namespace MedicalCard1.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime VisitData { get; set; }
        public string Complaints { get; set; }
        public string Anamnesis { get; set; }
        public string Diagnosis { get; set; }
        public string DoctorsOrder { get; set; }
        public virtual Patient Patient { get; set; }
        public int PatientId { get; set; }
        public virtual User Doctor { get; set; }
        public int DoctorId { get; set; }
    }
}
