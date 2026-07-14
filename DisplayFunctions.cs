using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using realNEA;
using RealNEA;
using SQLitePCL;
namespace RealNEABackUp.Services;

public class DisplayFunctions
{
    private HospitalsContext DbContext;
    private PriorityQueue patientQueue;
    public DisplayFunctions(HospitalsContext dbContext, PriorityQueue PatientQueue)
    {
        DbContext = dbContext;
        patientQueue = PatientQueue;
    }

    public void PrintAllDoctorPatientScores()
    {
        // Load all doctors with their compatibilities and patients
        var doctors = DbContext.Doctors
            .Include(d => d.linkedPatients)
            .ThenInclude(c => c.Patient)
            .ToList();

        Console.WriteLine(" Doctor-Patient Compatibility Scores ");

        foreach (var doctor in doctors)
        {
            Console.WriteLine($"\nDr {doctor.Name} (id: {doctor.DoctorId})");
            Console.WriteLine($"Hours Worked: {doctor.HoursWorked}");

            if (doctor.linkedPatients.Any() == true)
            {
                Console.WriteLine("Compatible Patients:");
                foreach (var compatibility in doctor.linkedPatients.OrderByDescending(c => c.CompatibilityScore))
                {
                    Console.WriteLine($"  - {compatibility.Patient.FirstName} {compatibility.Patient.LastName} " + $"(Score: {compatibility.CompatibilityScore})");
                }
            }
            else
            {
                Console.WriteLine("  No compatible patients found.");
            }
        }
    }

    public void DisplayDoctorAppointments()
    {
        List<DoctorClass> Doctors = DbContext.Doctors
        .Include(doctor => doctor.Appointments)
        .ThenInclude(appointment => appointment.Patient)
        .ToList();
        System.Console.WriteLine("\n ------ DOCTOR APPOINTMENTS ------");
        foreach (DoctorClass doctor in Doctors)
        {
            System.Console.WriteLine($"Doctor {doctor.Name}:");
            foreach (Appointment appointment in doctor.Appointments)
            {
                System.Console.WriteLine($"Appointment {appointment.AppointmentId} {appointment.StartTime} - {appointment.EndTime} with patient {appointment.Patient.FirstName} {appointment.Patient.LastName}");
            }
        }
    }

    public void DisplayQueue()
    {
        System.Console.WriteLine("\n ------ PATIENT QUEUE ------");
        Console.WriteLine($"Patients in queue: {patientQueue.PatientQueue.Count}");
        if (patientQueue.PatientQueue.Count > 0)
        {
            Console.WriteLine("Queue contents:");
            foreach (var patient in patientQueue.PatientQueue)
            {
                Console.WriteLine($"{patient.FirstName} {patient.LastName} - priority: {patient.PriorityScore} - severity {patient.severityScore}");
            }
        }
    }

    public void TriageNewPatient()
    {
        CreatePatient patientCreator = new CreatePatient(DbContext, patientQueue);
        patientCreator.TriageNewPatient();
    }
    
    public void GetWaitTime()
    {
        int Time = 0;

        if (patientQueue.PatientQueue.Count == 0)
        {
            System.Console.WriteLine("There are no patients in the queue");
            return;
        }

        System.Console.WriteLine($"\nFull Patient Name: ");
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            System.Console.WriteLine("Invalid name entered");
            return;
        }

        foreach(char character in input)
        {
            if(!char.IsLetter(character) && character != ' ')
            {
                System.Console.WriteLine("Invalid name entered");
                return;
            }
        }

        string[] fullName = input.Split(' ');

        if (fullName.Length != 2)
        {
            System.Console.WriteLine("invalid name entered");
            return;
        }
        
        string firstName = fullName[0].ToLower();
        string lastName = fullName[1].ToLower(); 


        PatientClass patient = DbContext.Patients.FirstOrDefault(patient => patient.FirstName.ToLower() == firstName && patient.LastName.ToLower() == lastName);

        if (patient == null)
        {
            System.Console.WriteLine("Patient not found");
            return;
        }

        if (patientQueue.PatientQueue.Contains(patient))
        {
            //looks at all patients before target patient
            foreach (PatientClass Patient in patientQueue.PatientQueue)
            {
                if (Patient != patient)
                {
                    Time += HelperClass.CalculateDuration(Patient);
                }

                else
                {
                    break;
                }
            }
        }

        Time = Time / DbContext.Doctors.ToList().Count;

        System.Console.WriteLine($"{patient.FirstName} {patient.LastName} has an estimated {Time} minutes left until appointment");
    }
}