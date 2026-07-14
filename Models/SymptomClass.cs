namespace RealNEA
{
    public class SymptomClass
    {
        public int SymptomId { get; internal set; }
        public string SymptomName { get; internal set; }

        public int severityScore { get; internal set; }

        public List<PatientSymptom> PatientSymptoms { get; internal set; } = new List<PatientSymptom>(); //navigation
        public List<SymptomAnswer> SymptomAnswers { get; internal set; } = new List<SymptomAnswer>();

        public List<DoctorSymptomClass> DoctorSymptoms { get; internal set; } = new List<DoctorSymptomClass>();
        
    }
}