using HospitalAppointmentAPI.Data;
using HospitalAppointmentAPI.DTOs;
using HospitalAppointmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentResponseDto>> BookAppointment([FromBody] AppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = await _context.Doctors.FindAsync(appointmentDto.DoctorId);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            var patient = await _context.Patients.FindAsync(appointmentDto.PatientId);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.DoctorId == appointmentDto.DoctorId &&
                    a.AppointmentDate.Date == appointmentDto.AppointmentDate.Date &&
                    a.TimeSlot == appointmentDto.TimeSlot &&
                    a.Status != "Cancelled");

            if (existingAppointment != null)
                return Conflict(new { message = "This time slot is already booked" });

            var appointment = new Appointment
            {
                AppointmentDate = appointmentDto.AppointmentDate,
                TimeSlot = appointmentDto.TimeSlot,
                Reason = appointmentDto.Reason,
                DoctorId = appointmentDto.DoctorId,
                PatientId = appointmentDto.PatientId,
                Status = "Scheduled"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var response = new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status,
                Reason = appointment.Reason,
                DoctorName = doctor.Name,
                PatientName = patient.Name
            };

            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.AppointmentId }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentResponseDto>> GetAppointmentById(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            var response = new AppointmentResponseDto
            {
                AppointmentId = appointment.AppointmentId,
                AppointmentDate = appointment.AppointmentDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status,
                Reason = appointment.Reason,
                DoctorName = appointment.Doctor.Name,
                PatientName = appointment.Patient.Name
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                return NotFound(new { message = "Appointment not found" });

            appointment.Status = "Cancelled";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetAllAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status,
                    Reason = a.Reason,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.Name
                })
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
