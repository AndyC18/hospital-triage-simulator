using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealNEA;

namespace RealNEABackUp.Services
{
    public class AppointmentGenerator
    {
        HospitalsContext DbContext;
        PriorityQueue PatientQueue;
        List<DoctorClass> availibleDoctors;
        Dictionary<DoctorClass, DateTime> availableDoctorTimes;
        PatientDoctorCompatibility Compatibility;
        DateTime startTime;  
        DateTime endTime = DateTime.Today.AddHours(23); //23
        public AppointmentGenerator(HospitalsContext context, PriorityQueue patientQueue, PatientDoctorCompatibility compatibilities)
        {
            DbContext = context;
            PatientQueue = patientQueue;
            Compatibility = compatibilities;
            UpdateDoctorAvailibility();

            //stores the times of when doctors are next available
            availableDoctorTimes = new Dictionary<DoctorClass, DateTime>();

            //all doctors available when program is run
            startTime = DateTime.Now;
            availibleDoctors = context.Doctors.ToList();
            
            foreach (DoctorClass doctor in availibleDoctors)
            {
                availableDoctorTimes.Add(doctor, startTime);
            }
        }


        //makes appointments throughout the day
        public void GenerateBatchOfAppointments()
        {
            Console.WriteLine($"\n--- Starting appointment generation at {DateTime.Now:HH:mm:ss} ---");
            Console.WriteLine($"Patients in queue: {PatientQueue.PatientQueue.Count}");

            UpdateDoctorAvailibility();

            //if all doctors busy, skip cycle
            if (availibleDoctors.Count == 0)
            {
                Console.WriteLine("No doctors available - skipping cycle");
                return;
            }

            DateTime currentTime = DateTime.Now;

            //foreach available doctor
            foreach (DoctorClass doctor in availibleDoctors.OrderBy(doctor => availableDoctorTimes[doctor]))
            {
                DateTime doctorAvailableTime = availableDoctorTimes[doctor];

                if (doctorAvailableTime <= currentTime)
                {
                    //sets the available time to now, so it doesnt schedule for the past
                    doctorAvailableTime = currentTime;
                }

                if (doctorAvailableTime >= endTime)
                {
                    //if the doctor isnt available until after closing time
                    Console.WriteLine($"Dr {doctor.Name} not available today (after end time)");
                    continue;
                }

                PatientClass bestPatient = GetBestPatientForTimeSlot(doctor, doctorAvailableTime);

                if (bestPatient != null)
                {
                    //updates the end time of next appointment
                    int duration = HelperClass.CalculateDuration(bestPatient);
                    DateTime appointmentEnd = doctorAvailableTime.AddMinutes(duration);

                    if (appointmentEnd <= endTime)
                    {
                        //schedules appointment
                        ScheduleAppointment(bestPatient, doctor, doctorAvailableTime);
                        availableDoctorTimes[doctor] = appointmentEnd;
                        Console.WriteLine($"Scheduled {bestPatient.FirstName} with Dr {doctor.Name} at {doctorAvailableTime:HH:mm:ss} for {duration} minutes");

                        //updates availability after scheduling for the next doctor
                        UpdateDoctorAvailibility();
                    }

                    else
                    {
                        Console.WriteLine($"Cannot schedule - appointment would end after closing time");
                    }
                }

            }
        }
        
        
        private PatientClass GetBestPatientForTimeSlot(DoctorClass doctor, DateTime timeSlot)
        {
            //all patients in queue
            List<int> PatientIdsInQueue = new List<int>();
            foreach (PatientClass patient in PatientQueue.PatientQueue)
            {
                PatientIdsInQueue.Add(patient.PatientId);
            }

            List<PatientClass> compatiblePatients = DbContext.Compatibilities
            .Where(c => PatientIdsInQueue.Contains(c.PatientId)) //only patients in queue
            .Where(c => c.DoctorId == doctor.DoctorId && c.CompatibilityScore > 0) //must match this doctor, and be compatible
            .Select(c => c.Patient) //only want patient
            .OrderByDescending(patient => patient.PriorityScore) //order
            .ThenByDescending(patient => patient.severityScore)
            .ToList();

            return compatiblePatients.FirstOrDefault();
        }


        private void UpdateDoctorAvailibility()
        {
            Console.WriteLine($"Current time: {DateTime.Now}");

            //all doctors, loads appointments
            List<DoctorClass> allDoctors = DbContext.Doctors
            .Include(doctor => doctor.Appointments)
            .ToList();

            Console.WriteLine($"Total doctors in system: {allDoctors.Count}");
            
            //availible if they have no appointments scheduled starting before now and ending after now
            List<DoctorClass> AvailableDoctors = allDoctors
                .Where(doctor => !doctor.Appointments.Any(Appointment => Appointment.StartTime <= DateTime.Now && Appointment.EndTime > DateTime.Now))
                .ToList();

            Console.WriteLine($"Available doctors found: {AvailableDoctors.Count}");

            availibleDoctors = AvailableDoctors;
        }

        private void ScheduleAppointment(PatientClass bestPatient, DoctorClass doctor, DateTime time)
        {
            int duration = HelperClass.CalculateDuration(bestPatient);

            //patient, doctor, startTime, duration
            Appointment appointment = new Appointment(bestPatient, doctor, time, duration);
            DbContext.Add(appointment);
            doctor.AddHoursWorked(duration);
            //removes patient from the queue, reorders queue
            PatientQueue.GetNextPatient(bestPatient);
            DbContext.SaveChanges();   
        }


    }
}