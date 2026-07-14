namespace RealNEA;

public class PatientNode
{
    private int patientID;
    public int PatientID
    {
        get { return patientID; }
        set { patientID = value; }
    }
    private string patientName;
    public string PatientName
    {
        get { return patientName; }
        set { patientName = value; }
    }
    private double severityScore;
    public double SeverityScore
    {
        get { return severityScore; }
        set { severityScore = value; }
    }
    private List<string> symptoms = new List<string>();
    public List<string> Symptoms
    {
        get { return symptoms; }
        set { symptoms = value; }
    }
    //private List<DoctorNode> compatibleDoctors = new List<DoctorNode>();
    private Dictionary<DoctorNode, double> adajacentDoctors = new Dictionary<DoctorNode, double>();

    public PatientNode(int patientID, string patientName)
    {
        this.patientID = patientID;
        this.patientName = patientName;
        // System.Console.WriteLine("Enter severity score"); //in real thing taken from db
        // severityScore = Convert.ToDouble(Console.ReadLine());
        // System.Console.WriteLine("Enter symptoms, enter done when done");  //also taken from db
        // while (true)
        // {
        //     string input = Console.ReadLine();
        //     if (input == "done")
        //     {
        //         break;
        //     }
        //     symptoms.Add(input);
        // }
    }

    public void AddCompatibleDoctor(DoctorNode compatibleDoctor, double weighting)
    {
        adajacentDoctors.Add(compatibleDoctor, weighting);
    }

    public Dictionary<DoctorNode, double> GetCompatibleDoctors()
    {
        return new Dictionary<DoctorNode, double>(adajacentDoctors);
    }
}