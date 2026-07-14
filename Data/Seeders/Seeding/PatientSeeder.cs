using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RealNEA;

namespace RealNEABackUp.Data.Seeders
{
    public static class PatientSeeder
    {
        public static void seedPatients(HospitalsContext context) //only works once symptoms have been defined in database
        {

            if (!context.Patients.Any())
            {
                //matches to the database
                SymptomClass fever = context.Symptoms.First(a => a.SymptomName == "Fever");
                SymptomClass headPain = context.Symptoms.First(a => a.SymptomName == "Head Pain");
                SymptomClass chestPain = context.Symptoms.First(a => a.SymptomName == "Chest Pain");
                SymptomClass abdomenPain = context.Symptoms.First(a => a.SymptomName == "Abdomen Pain");
                SymptomClass limbsPain = context.Symptoms.First(a => a.SymptomName == "Limbs Pain");
                SymptomClass emergencySymptoms = context.Symptoms.First(a => a.SymptomName == "Emergency Symptoms");

                List<PatientClass> patients = new List<PatientClass>();

                PatientClass patient1 = new PatientClass("Jaden", "Kang", new DateTime(2003, 09, 14), "8 WychWood Cresecent");
                PatientClass patient2 = new PatientClass("Jess", "Luff", new DateTime(2000, 02, 24), "26 James Close");
                PatientClass patient3 = new PatientClass("Monty", "Tonker", new DateTime(1947, 05, 7), "10 Ball Street");
                PatientClass patient4 = new PatientClass("Rita", "Smith", new DateTime(1999, 07, 04), "6 Elderberry Close");

                patient1.setSeverityScore(11);
                patient2.setSeverityScore(5);
                patient3.setSeverityScore(12);
                patient4.setSeverityScore(100);

                patient1.AddSymptom(fever);
                patient1.AddSymptom(headPain);

                patient2.AddSymptom(fever);

                patient3.AddSymptom(fever);
                patient3.AddSymptom(abdomenPain);

                patient4.AddSymptom(emergencySymptoms);

                patients.AddRange(patient1, patient2, patient3, patient4);
                context.Patients.AddRange(patients);
                context.SaveChanges();

            }
        }
       
    }
}