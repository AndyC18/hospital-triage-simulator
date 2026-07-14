using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealNEA;

namespace RealNEABackUp.Services
{
    public class PriorityQueue
    {
        private HospitalsContext DbContext = new HospitalsContext();
        public List<PatientClass> PatientQueue { get; private set; } = new List<PatientClass>();
        public PriorityQueue(HospitalsContext context)
        {
            DbContext = context;
        }


        //updates the priority score of the patient
        public void UpdatePriorityScore(PatientClass patient)
        {
            double priorityScore = 0;

            //base priority based on severity score, time of registration, age
            priorityScore += patient.severityScore * 0.6;

            TimeSpan time = DateTime.Now - patient.RegistrationTime;
            priorityScore += time.TotalMinutes * 0.2;

            int age = DateTime.Now.Year - patient.DateOfBirth.Year;

            if (age < 6 || age > 65)
            {
                priorityScore += 10;
            }

            patient.SetPriorityScore(priorityScore); 
            DbContext.SaveChanges();
        }

        //adds new patients to queue
        public void AddPatientToQueue(PatientClass patient)
        {
            //if not severe, dont add to queue
            if (patient.severityScore <= 5)
            {
                System.Console.WriteLine($"\n{patient.FirstName} {patient.LastName} Refer To GP, severity score <= 5");
            }

            else
            {
                UpdatePriorityScore(patient);
                PatientQueue.Add(patient);
                UpdateQueuePriorities();
            }
            
        }

        //sends the next patient for appointment, readjusts new queue
        public PatientClass GetNextPatient(PatientClass bestPatient)
        {
            if (PatientQueue.Count == 0)
            {
                Console.WriteLine("Queue is empty return null");
                return null;
            }

            PatientQueue.Remove(bestPatient);

            UpdateQueuePriorities();
            Console.WriteLine($"\n ");

            return bestPatient;
        }

        //returns next patient
        public PatientClass PeekNextPatient()
        {
            if (PatientQueue.Count > 0)
            {
                return PatientQueue.First();
            }

            return null;
        }

        //refreshes all paitients priority
        public void UpdateQueuePriorities()
        {
            foreach (PatientClass patient in PatientQueue)
            {
                UpdatePriorityScore(patient);
            }
            SortQueue();
        }

        //sorts queue by priority
        public void SortQueue()
        {
            PatientQueue = PatientQueue
            .OrderByDescending(patient => patient.PriorityScore)
            .ThenByDescending(patient => patient.severityScore) //order by severity score if the priority score is the same
            .ToList();
        }
    }
}         