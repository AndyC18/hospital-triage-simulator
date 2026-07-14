using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
namespace RealNEA
{
    public class DoctorClass
    {
        public int DoctorId { get; private set; }
        public double HoursWorked { get; private set; }
        public string Name { get; private set; }
        // public bool IsAvailable { get; private set; }
        public List<Appointment> Appointments { get; private set; }
        public List<DoctorSymptomClass> doctorSymptoms { get; private set; } //treatable symptoms

        public List<CompatibilityClass> linkedPatients { get; private set; }

        public DoctorClass()
        {
            
        }
        public DoctorClass(string name, int hoursWorked)
        {
            Name = name;
            // IsAvailable = isAvailable;
            HoursWorked = hoursWorked;

            Appointments = new List<Appointment>();
            doctorSymptoms = new List<DoctorSymptomClass>();
            linkedPatients = new List<CompatibilityClass>();
        }

        public void AddSymptom(SymptomClass symptom)
        {
            DoctorSymptomClass Symptom = new DoctorSymptomClass(this, symptom);
            doctorSymptoms.Add(Symptom); //unsure
        }

        public void AddHoursWorked(int duration)
        {
            HoursWorked += (duration / 60);
        }

    }
}