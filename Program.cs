using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using realNEA;
using realNEA.Migrations;
using RealNEABackUp.Data.Seeders;
using RealNEABackUp.Services;
namespace RealNEA;

class Program
{
    static void Main(string[] args)
    {

        HospitalsContext db = new HospitalsContext();
        TriageDbInitialiser.Initialise(db);
        DoctorSeeder.SeedDoctors(db);
        object sharedLock = new object();
        
        PriorityQueue PatientQueue = new PriorityQueue(db);
        PatientDoctorCompatibility compatibilities = new PatientDoctorCompatibility(db);
        DisplayFunctions display = new DisplayFunctions(db, PatientQueue);

        Console.WriteLine("PATIENT SIMULATION");

        PatientSimulator simulator = new PatientSimulator(db, PatientQueue, sharedLock);
        RealTimeScheduling scheduling = new RealTimeScheduling(db, PatientQueue, compatibilities, sharedLock);

        simulator.startSimulation();
        
        Console.WriteLine("Press 's' to stop simulation, 'd' to display state, 'q' to quit, 'a' to see doctor appointments, 't' to triage a patient, 'e' to get wait time");

        // Interactive loop
        while (true)
        {
            var key = Console.ReadKey(false);

            if (key.KeyChar == 's' || key.KeyChar == 'S')
            {
                simulator.stopSimulation();
                scheduling.stopScheduling();
                Console.WriteLine("\nSimulation stopped.");
                break;
            }

            else if (key.KeyChar == 'd' || key.KeyChar == 'D')
            {
                display.DisplayQueue();
            }
            else if (key.KeyChar == 'a' || key.KeyChar == 'A')
            {
                display.DisplayDoctorAppointments();
            }

            else if (key.KeyChar == 'q' || key.KeyChar == 'Q')
            {
                List<SymptomClass> allSymptoms = db.Symptoms.ToList();
                List<PatientClass> patients = new List<PatientClass>();
                PatientClass patient1 = new PatientClass("TEST", "ONE", new DateTime(2025, 09, 14), "8 WychWood Cresecent");
                PatientClass patient2 = new PatientClass("TEST", "TWO", new DateTime(2003, 09, 14), "26 James Close");
                patient1.setSeverityScore(100);
                patient2.setSeverityScore(99);

                patient1.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Dizziness"));
                patient1.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Nausea"));
                patient2.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Dizziness"));
                patient2.AddSymptom(allSymptoms.First(symptom => symptom.SymptomName == "Nausea"));
                db.Patients.AddRange(patients);
                PatientQueue.AddPatientToQueue(patient1);
                PatientQueue.AddPatientToQueue(patient2);
                patient1.SetPriorityScore(90);
                patient2.SetPriorityScore(90);
            
            }

            else if (key.KeyChar == 't' || key.KeyChar == 'T')
            {
                display.TriageNewPatient();
            }
            
            
            else if (key.KeyChar == 'e' || key.KeyChar == 'E')
            {
                display.GetWaitTime();
            }
        }
    }
}