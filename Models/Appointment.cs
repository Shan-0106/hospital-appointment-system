namespace HospitalAppointmentAPI.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = "Scheduled";
        public string Reason { get; set; } = string.Empty;

        // Foreign Keys
        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        // Navigation properties
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
    }
}
