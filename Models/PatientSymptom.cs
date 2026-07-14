namespace RealNEA
{
    public class PatientSymptom
    {
        public int PatientId { get; private set; } //pk
        public int SymptomId { get; private set; } //pk
        public DateTime DateOfReport { get; private set; }

        //navigation
        public PatientClass Patient{ get; private set; }
        public SymptomClass Symptom { get; private set; }
        public PatientSymptom()
        {
            
        }

        public PatientSymptom(int patientId, int symptomId, DateTime timeOfReport)
        {
            PatientId = patientId;
            SymptomId = symptomId;
            DateOfReport = timeOfReport;
        }
    }
}