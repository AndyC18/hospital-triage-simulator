using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealNEA;
namespace RealNEABackUp.Services
{
    public class PatientDoctorCompatibility
    {
        HospitalsContext DbContext;
        public PatientDoctorCompatibility(HospitalsContext context)
        {
            DbContext = context;
        }

        public void UpdateCompatibilities()
        {
            //loads patients and doctors with their symptoms
            List<PatientClass> allPatients = DbContext.Patients
            .Include(patient => patient.PatientSymptoms)
            .ThenInclude(patientsymptom => patientsymptom.Symptom)
            .ToList();

            List<DoctorClass> allDoctors = DbContext.Doctors
            .Include(doctor => doctor.doctorSymptoms)
            .ThenInclude(doctorsymptoms => doctorsymptoms.Symptom)
            .ToList();

            //clears the old compatibilities so it can be updated
            DbContext.Compatibilities.RemoveRange(DbContext.Compatibilities); 

            foreach (PatientClass _patient in allPatients)
            {
                foreach (DoctorClass _doctor in allDoctors)
                {
                    double score = calculateCompatibility(_patient, _doctor, allDoctors);
                    CompatibilityClass compatibility = new CompatibilityClass(_patient, _doctor, score);
                    _doctor.linkedPatients.Add(compatibility);
                    _patient.linkedDoctors.Add(compatibility);
                }
            }
            DbContext.SaveChanges();

        }

        private double calculateCompatibility(PatientClass patient, DoctorClass doctor, List<DoctorClass> allDoctors)
        {
            //if patients have 0 symptoms
            if (patient.PatientSymptoms.Count == 0)
            {
                return 0;
            }

            //check if doctor can treat patient

            List<int> PatientSymptomIds = patient.PatientSymptoms.Select(ps => ps.SymptomId).ToList();
            List<int> DoctorSymptomIds = doctor.doctorSymptoms.Select(ts => ts.SymptomId).ToList();

            bool perfectMatch = true; //doctor can treat all the symptoms
            bool PotentialMatch = false; //doctor can treat some symptoms
            int matchingSymptoms = 0;
            double score = 0;

            //check if its a perfect match
            foreach (int symptom in PatientSymptomIds)
            {
                if (!DoctorSymptomIds.Contains(symptom))
                {
                    perfectMatch = false;
                }
            }

            //gets number of matching symptoms
            foreach (int symptom in PatientSymptomIds)
            {
                if (DoctorSymptomIds.Contains(symptom))
                {
                    matchingSymptoms++;
                    PotentialMatch = true;
                }
            }

            if (PotentialMatch == false)
            {
                return 0; //docor not qualified to treat patient AT ALL
            }

            if (perfectMatch)
            {
                score += 80;
            }
            else
            {
                score += 40;
            }

            if (!perfectMatch)
            {
                //how well is doctor qualified
                double matchingBonus = (matchingSymptoms / patient.PatientSymptoms.Count) * 30;
                score += matchingBonus;
            }

            //adjust for doctor availability
            if (doctor.HoursWorked <= 4)
            {
                score += 15;
            }
            else if (doctor.HoursWorked >= 8)
            {
                score -= 10;
            }

            //consider how many doctors can perfectly treat the patient
            int specialistCount = CalculateDoctorsQualified(patient, allDoctors);

            if (specialistCount <= 2)
            {
                score += 10;
            }

            return Math.Max(0, Math.Min(score, 100)); //score between 0-100
        }
        
        //calculates how many perfect match doctors there are
        private int CalculateDoctorsQualified(PatientClass patient, List<DoctorClass> allDoctors)
        {
            int qualifiedDoctors = 0;
            
            List<int> patientSymptomIds = patient.PatientSymptoms.Select(ps => ps.SymptomId).ToList();

            foreach (DoctorClass doctor in allDoctors)
            {
                bool perfectMatch = true;

                List<int> doctorSymptomIds = doctor.doctorSymptoms.Select(ds => ds.SymptomId).ToList();

                foreach (int symptom in patientSymptomIds)
                {
                    if (!doctorSymptomIds.Contains(symptom))
                    {
                        perfectMatch = false;
                    }
                }

                if (perfectMatch == true)
                {
                    qualifiedDoctors++;
                }
            }

            return qualifiedDoctors;
        }
    }
}