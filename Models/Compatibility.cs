using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealNEA;

namespace RealNEA
{
    public class CompatibilityClass
    {
        public int PatientId { get; private set; }
        public int DoctorId { get; private set; }
        public double CompatibilityScore { get; private set; }

        public PatientClass Patient { get; private set; }
        public DoctorClass Doctor { get; private set; }
        public CompatibilityClass()
        {
            
        }
        public CompatibilityClass(PatientClass patient, DoctorClass doctor, double compatibilityScore)
        {
            PatientId = patient.PatientId;
            DoctorId = doctor.DoctorId;
            Patient = patient;
            Doctor = doctor;
            CompatibilityScore = compatibilityScore;
        }
    }
}