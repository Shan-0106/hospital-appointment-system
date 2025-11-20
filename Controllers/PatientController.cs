using HospitalAppointmentAPI.Data;
using HospitalAppointmentAPI.DTOs;
using HospitalAppointmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAppointmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatientController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PatientResponseDto>> RegisterPatient([FromBody] PatientDto patientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patient = new Patient
            {
                Name = patientDto.Name,
                Email = patientDto.Email,
                PhoneNumber = patientDto.PhoneNumber,
                DateOfBirth = patientDto.DateOfBirth,
                Address = patientDto.Address
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var response = new PatientResponseDto
            {
                PatientId = patient.PatientId,
                Name = patient.Name,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address
            };

            return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponseDto>> GetPatientById(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            var response = new PatientResponseDto
            {
                PatientId = patient.PatientId,
                Name = patient.Name,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetAllPatients()
        {
            var patients = await _context.Patients
                .Select(p => new PatientResponseDto
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    DateOfBirth = p.DateOfBirth,
                    Address = p.Address
                })
                .ToListAsync();

            return Ok(patients);
        }
    }
}
