using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using realNEA;
using RealNEA;
using RealNEABackUp.Services;

public class PatientSimulator
{
    private PriorityQueue patientQueue;
    private HospitalsContext dbContext;

    object _lock;
    public PatientSimulator(HospitalsContext DbContext, PriorityQueue PatientQueue, object sharedLock)
    {
        patientQueue = PatientQueue;
        dbContext = DbContext;
        _lock = sharedLock;
        
    }
    Random random = new Random();
    private String[] FirstNames =
    { "Chaya" , "Abdiel", "Jesus", "Virginia", "Nathaniel", "Heidi", "Camron",
    "Fernanda", "Gaven", "Jakob", "Abel", "Finley", "Easton", "Seth", "Spencer", "Cara", "Baron", "Jabari", "Theresa" };

    private String[] LastNames =
    { "Romeo", "Electra","Royce","McCain","Bayard","Sterling","Jenkins","Hennessey","Holland","Lockhart"};

    Timer arrivalTimer;
    
    public void startSimulation()
    {
        for (int i = 0; i <= 8; i++)
        {
            AddPatient(null);
        }
        
        System.Console.WriteLine("Patient simulation started");
        arrivalTimer = new Timer(AddPatient, null, TimeSpan.FromMinutes(0), TimeSpan.FromSeconds(45));
    }
    
    public void stopSimulation()
    {
        arrivalTimer.Dispose();
        System.Console.WriteLine("Stopped simulation");
    }
    private void AddPatient(object a)
    {
        lock(_lock)
        {
            int randomIndexForFirstName = random.Next(0, FirstNames.Length);
            int randomIndexForLastName = random.Next(0, LastNames.Length);
            int randomNumberOfSymptoms = random.Next(1, 3);

            string firstName = FirstNames[randomIndexForFirstName];
            string lastName = LastNames[randomIndexForLastName];

            int age = random.Next(1, 85);
            DateTime DoB = DateTime.Now.AddYears(-age);

            string Address = "12 simulation street";

            List<SymptomClass> allSymptoms = dbContext.Symptoms.ToList();

            List<SymptomClass> symptoms = allSymptoms
            .OrderBy(symptom => random.Next()) //randomises order
            .Take(randomNumberOfSymptoms)
            .ToList();

            PatientClass newPatient = new PatientClass(firstName, lastName, DoB, Address);
            int severityScore = 0;

            foreach (SymptomClass symptom in symptoms)
            {
                newPatient.AddSymptom(symptom);
                severityScore += symptom.severityScore;
            }

            newPatient.setSeverityScore(severityScore);

            dbContext.Add(newPatient);
            dbContext.SaveChanges();

            patientQueue.AddPatientToQueue(newPatient);

            if (patientQueue.PatientQueue.Contains(newPatient))
            {
                System.Console.WriteLine($"\n{firstName} {lastName} age {age} with severity {severityScore} added to queue");
                System.Console.WriteLine($"symptoms: {string.Join(", ", symptoms.Select(symptom => symptom.SymptomName))} \n ");
            }
        }
    }
}