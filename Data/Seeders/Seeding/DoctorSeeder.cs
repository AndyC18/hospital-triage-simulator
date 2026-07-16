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
        static Random random = new Random();
        
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
        private static List<DoctorClass> addDoctorsToList(List<DoctorClass> doctors, HospitalsContext context)
            {
                int numberOfDoctors = random.Next(6,12); 

                for(int i = 0; i < numberOfDoctors; i++)
                {
                    string doctorName = HelperClass.randomName();
                    DoctorClass doctor = new DoctorClass(doctorName, randomDoctorHours());
                    doctors.Add(doctor);

                    foreach(SymptomClass symptom in GetRandomSymptoms(context))
                    {
                        doctor.AddSymptom(symptom);
                    }
                    doctors.Add(doctor);
                }
                return doctors;
                
            }
        private static int randomDoctorHours()
        {
            bool valid = false;
            while(valid == false)
            {
  
                int randomHours = random.Next(0, 8);
                if(randomHours <= 4)
                {
                    valid = true;
                    return randomHours;
                }

                else
                {
                    int number = random.Next(1, 3);
                    {
                        if(number == 1)
                        {
                            valid = true;
                            return randomHours;
                        }
                    }
                }
            }
           return 1;
        }

        private static List<SymptomClass> GetRandomSymptoms(HospitalsContext context)
        {
            List<SymptomClass> allSymptoms = context.Symptoms.ToList();
            int numberOfSymptoms = 0;
            bool valid = false;

            while(valid == false)
            {
                numberOfSymptoms = random.Next(1, context.Symptoms.Count() + 1);
                if(numberOfSymptoms <= 9)
                {
                    valid = true;
                }
                else
                {
                    int number = random.Next(1, 4);
                    {
                        if(number == 1)
                        {
                            valid = true;
                        }
                    }
                }
            }
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