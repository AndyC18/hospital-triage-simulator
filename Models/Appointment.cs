using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace RealNEA
{
    public class Appointment
    {
        public int AppointmentId { get; private set; }
        public int PatientId { get; private set; }
        public int DoctorId { get;private set; }
        public DateTime StartTime { get ;private set; }
        public DateTime EndTime { get;private set; }
        public int RoomNumber { get;private set; }
        public int EstimatedDuration { get;private set; }
        public PatientClass Patient { get;private set; }
        public DoctorClass Doctor { get; private set; }
        public Appointment()
        {
            
        }
        public Appointment(PatientClass patient, DoctorClass doctor, DateTime startTime, int duration )
        {
            Patient = patient;
            Doctor = doctor;
            PatientId = Patient.PatientId;
            DoctorId = Doctor.DoctorId;
            StartTime = startTime;
            EstimatedDuration = duration;
            EndTime = startTime.AddMinutes(duration);
        }
    }
}