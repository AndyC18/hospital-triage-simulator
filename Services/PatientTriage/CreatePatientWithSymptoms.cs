using Microsoft.EntityFrameworkCore.Internal;
using RealNEA;
using RealNEABackUp.Services;

namespace realNEA;

//creates the patient as well as the details in the patientSymptom table
public class CreatePatient
{
    private HospitalsContext DbContext;
    private PriorityQueue queue;
    public CreatePatient(HospitalsContext dbContext, PriorityQueue Queue)
    {
        DbContext = dbContext;
        queue = Queue;
    }
    
    //creates the new patient and adds to queue
    public void TriageNewPatient()
    {
        //triage
        TriageDecisionTree tree = new TriageDecisionTree(DbContext);
        var triage = tree.RunDecisionTree();

        int severityScore = triage.severityScore;
        List<SymptomClass> patientSymptoms = triage.symptoms;

        //adds details such as name, address etc
        PatientClass newPatient = PatientDetails.GetDetails();
        newPatient.setSeverityScore(severityScore);

        DbContext.Add(newPatient);
        DbContext.SaveChanges();

        AddPatientSymptoms(newPatient.PatientId, patientSymptoms);

        queue.AddPatientToQueue(newPatient);
        queue.UpdateQueuePriorities();

        System.Console.WriteLine($"{newPatient.FirstName} {newPatient.LastName} with priority {newPatient.PriorityScore} has been added to queue");
    }

    //adds data to the patientSymptom table
    private void AddPatientSymptoms(int patientId, List<SymptomClass> Symptoms)
    {
        if (Symptoms != null)
        {
            foreach (SymptomClass symptom in Symptoms)
            {
                PatientSymptom Symptom = new PatientSymptom(patientId, symptom.SymptomId, DateTime.Now);
                DbContext.Add(Symptom);
                DbContext.SaveChanges();
            }
        }
    }
}