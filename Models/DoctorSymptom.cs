using Microsoft.EntityFrameworkCore;
namespace RealNEA
{
    public class DoctorSymptomClass
    {
        public int DoctorId { get; private set; }
        public int SymptomId { get; private set; }
        public DoctorClass Doctor { get; private set; }
        public SymptomClass Symptom { get; private set; }
        public DoctorSymptomClass()
        {
            
        }

        public DoctorSymptomClass(DoctorClass doctor, SymptomClass symptom)
        {
            Doctor = doctor;
            Symptom = symptom;
            DoctorId = doctor.DoctorId;
            SymptomId = symptom.SymptomId;
        }
    }
}