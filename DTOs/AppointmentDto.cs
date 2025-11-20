namespace HospitalAppointmentAPI.DTOs
{
    public class AppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
    }

    public class AppointmentResponseDto
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
    }
}
