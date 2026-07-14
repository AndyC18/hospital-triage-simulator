using Microsoft.EntityFrameworkCore;
namespace RealNEA
{
    public class PatientClass
    {
        public int PatientId { get;private set; }
        public string FirstName { get;private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get;private set; }
        public string Address { get; private set; }

        public int severityScore { get;private set; }
        public double PriorityScore { get;private set; }
        public DateTime RegistrationTime { get;private set; }


        //navigation
        public List<PatientSymptom> PatientSymptoms { get; private set; } = new List<PatientSymptom>(); //navigation
        public List<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public List<CompatibilityClass> linkedDoctors { get; private set; } = new List<CompatibilityClass>();
        


        public void setSeverityScore(int severityscore)
        {
            severityScore = severityscore;
        }

        public PatientClass()
        {
            
        }
        public PatientClass(string firstName, string lastName, DateTime DoB, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = DoB;
            Address = address;
            PriorityScore = 0;
            severityScore = 0;
            RegistrationTime = DateTime.Now;
     
        }

        public void AddSymptom(SymptomClass symptom)
        {
            PatientSymptom Symptom = new PatientSymptom(this.PatientId, symptom.SymptomId, DateTime.Now);
            PatientSymptoms.Add(Symptom);
        }

        public void SetPriorityScore(double priorityScore)
        {
            PriorityScore = priorityScore;
        }
    }   
}