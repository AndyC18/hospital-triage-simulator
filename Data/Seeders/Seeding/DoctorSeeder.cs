using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace RealNEA
{
    public static class DoctorSeeder
    {
        public static void SeedDoctors(HospitalsContext context)
        {
            if (!context.Doctors.Any())
            {
                List<DoctorClass> doctors = new List<DoctorClass>();
                addDoctorsToList(doctors, context);
                context.Doctors.AddRange(doctors);
                context.SaveChanges();
            }
        }

        // private static List<DoctorClass> addDoctorsToList(List<DoctorClass> doctors, HospitalsContext context)
        // {
        //     List<SymptomClass> allSymptoms = context.Symptoms.ToList();

        //     DoctorClass doctor1 = new DoctorClass("Dulce Yang", new Random().Next(0, 8));
        //     DoctorClass doctor2 = new DoctorClass("Adam New", new Random().Next(0, 8));
        //     DoctorClass doctor3 = new DoctorClass("Holly Miller", new Random().Next(0, 8));
        //     DoctorClass doctor4 = new DoctorClass("Jess Smith", new Random().Next(0, 8));
        //     DoctorClass doctor5 = new DoctorClass("Albert Sting", new Random().Next(0, 8));
 
        //     doctor1.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Fever"));
        //     doctor1.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Head Pain"));
        //     doctor1.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Vomiting"));
        //     doctor2.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Fever"));
        //     doctor2.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Limbs Pain"));
        //     doctor2.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Emergency Symptoms"));
        //     doctor3.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Abdomen Pain"));
        //     doctor3.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Limbs Pain"));
        //     doctor3.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Emergency Symptoms"));
        //     doctor4.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Dizziness"));
        //     doctor4.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Nausea"));
        //     doctor4.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Chest Pain"));
        //     doctor5.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Head Pain"));
        //     doctor5.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Dizziness"));
        //     doctor5.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Emergency Symptoms"));
        //     doctors.AddRange(new[] { doctor1, doctor2, doctor3, doctor4, doctor5});
        //     return doctors;
        // }
        
    

        private static List<DoctorClass> addDoctorsToList(List<DoctorClass> doctors, HospitalsContext context)
            {
                Random random = new Random();
                int numberOfDoctors = random.Next(6,12); 

                for(int i = 0; i < numberOfDoctors; i++)
                {
                    string doctorName = HelperClass.randomName();
                    DoctorClass doctor = new DoctorClass(doctorName, random.Next(0, 8));
                    doctors.Add(doctor);

                    foreach(SymptomClass symptom in GetRandomSymptoms(context))
                    {
                        doctor.AddSymptom(symptom);
                    }
                    doctors.Add(doctor);
                }
                return doctors;
                
            }

        private static List<SymptomClass> GetRandomSymptoms(HospitalsContext context)
        {
            Random random = new Random();
            List<SymptomClass> allSymptoms = context.Symptoms.ToList();
            int numberOfSymptoms = random.Next(1, context.Symptoms.Count() + 1); 
            List<SymptomClass> selectedSymptoms = new List<SymptomClass>();

            while (selectedSymptoms.Count < numberOfSymptoms)
            {
                SymptomClass randomSymptom = allSymptoms[random.Next(allSymptoms.Count)];
                if (!selectedSymptoms.Contains(randomSymptom))
                {
                    selectedSymptoms.Add(randomSymptom);
                }
            }

            return selectedSymptoms;
        }
    }
    


}