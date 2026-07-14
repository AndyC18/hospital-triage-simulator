namespace RealNEA;

public static class Compatibility
{
    //calculates compatibility score between a patient and a doctor
    public static double CalculateCompatibilityScore(DoctorNode doctor, PatientNode patient, int totalDoctors, List<DoctorNode> allDoctors)
    {
        int numberOfDoctorsQualifiedToTreat = 0;
        foreach (string symptom in patient.Symptoms) //if the doctor is not qualified to treat the symptom return 0
        {
            if (!doctor.TreatableSymptoms.Contains(symptom))
            {
                return 0;
            }
        }

        foreach (DoctorNode Doctor in allDoctors) //calculates no. doctors able to treat the condition
        {
            bool treatable = true;
            foreach (string symptom in patient.Symptoms)
            {
                if (!Doctor.TreatableSymptoms.Contains(symptom))
                {
                    treatable = false;
                }
            }
            if (treatable == true)
            {
                numberOfDoctorsQualifiedToTreat++;
            }
        }

        double matchingSymptoms = 0;
        foreach (string symptom in patient.Symptoms)
        {
            if (doctor.TreatableSymptoms.Contains(symptom))
            {
                matchingSymptoms++;
            }
        }

        double symptomMatchingRatio = matchingSymptoms / patient.Symptoms.Count;//the higher the more suitable the doctor is
        double fatigue = doctor.HoursWorked / 12;
        double scarcityOfDoctors = 1 / numberOfDoctorsQualifiedToTreat;
        double score = (1.5 * scarcityOfDoctors) + (symptomMatchingRatio) - (1.2 * fatigue);
        return Math.Max(score, 0); //cant return negative number
    }
    //for the patients that are matched, calculate compatibility score, add this score to adjacent doctors. store as a dictionary

    //Adds adjacent doctors to new patient nodes (builds graph)
    //This only takes in new patients after reading from the db (in db have an attribute to say whether theyve been dealt with or not)
    public static void BuildCompatibilityGraph(List<DoctorNode> allDoctors, List<PatientNode> newPatients)
    {
        int totalDoctors = allDoctors.Count;
        foreach (PatientNode patient in newPatients)
        {
            foreach (DoctorNode doctor in allDoctors)
            {
                double score = CalculateCompatibilityScore(doctor, patient, totalDoctors, allDoctors);
                if (score > 0)
                {
                    patient.AddCompatibleDoctor(doctor, score);
                }
            }
        }
    }
}