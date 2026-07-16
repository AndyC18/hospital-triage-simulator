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
            int randomNumberOfSymptoms = HelperClass.weightedRandomSymptomCount();

            string firstName = HelperClass.randomName().Split(' ')[0];
            string lastName = HelperClass.randomName().Split(' ')[1];

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