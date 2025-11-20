using HospitalAppointmentAPI.Data;
using HospitalAppointmentAPI.DTOs;
using HospitalAppointmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<DoctorResponseDto>> AddDoctor([FromBody] DoctorDto doctorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doctor = new Doctor
            {
                Name = doctorDto.Name,
                Specialization = doctorDto.Specialization,
                Email = doctorDto.Email,
                PhoneNumber = doctorDto.PhoneNumber
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            var response = new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber
            };

            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorId }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetAllDoctors()
        {
            var doctors = await _context.Doctors
                .Select(d => new DoctorResponseDto
                {
                    DoctorId = d.DoctorId,
                    Name = d.Name,
                    Specialization = d.Specialization,
                    Email = d.Email,
                    PhoneNumber = d.PhoneNumber
                })
                .ToListAsync();

            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorResponseDto>> GetDoctorById(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            var response = new DoctorResponseDto
            {
                DoctorId = doctor.DoctorId,
                Name = doctor.Name,
                Specialization = doctor.Specialization,
                Email = doctor.Email,
                PhoneNumber = doctor.PhoneNumber
            };

            return Ok(response);
        }

        [HttpGet("{id}/appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentResponseDto>>> GetDoctorAppointments(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == id)
                .Select(a => new AppointmentResponseDto
                {
                    AppointmentId = a.AppointmentId,
                    AppointmentDate = a.AppointmentDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status,
                    Reason = a.Reason,
                    DoctorName = doctor.Name,
                    PatientName = a.Patient.Name
                })
                .ToListAsync();

            return Ok(appointments);
        }
    }
}
