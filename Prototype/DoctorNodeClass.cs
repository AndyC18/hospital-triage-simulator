namespace RealNEA;
using System.Data.Common;
using Microsoft.Win32.SafeHandles;
public class DoctorNode
{
    private int doctorID;
    public int DoctorID
    {
        get { return doctorID; }
        set { doctorID = value; }
    }
    private string doctorName;
    public string DoctorName
    {
        get { return doctorName; }
        set { doctorName = value; }
    }
    private List<string> specialisation = new List<string>();
    public List<string> Specialisation 
    {
        get { return specialisation; }
        set { specialisation = value; }
    }

    private List<string> treatableSymptoms = new List<string>();
    public List<string> TreatableSymptoms
    {
        get { return treatableSymptoms; }
        set { treatableSymptoms = value; }
    }
    private double hoursWorked;
    public double HoursWorked
    {
        get { return hoursWorked; }
        set { hoursWorked = value; }
    }
    public DoctorNode(int DoctorID, string doctorName)
    {
        doctorID = DoctorID; 
        this.doctorName = doctorName;

        // System.Console.WriteLine("Enter qualifications, type done when done: ");
        // while (true)
        // {
        //     if (Console.ReadLine().ToLower() == "done")
        //     {
        //         break;
        //     }
        //     else
        //     {
        //         specialisation.Add(Console.ReadLine());
        //     }
        // }

        // System.Console.WriteLine("How many hours worked today: "); //gather from database, update after each shift
        // hoursWorked = Convert.ToDouble(Console.ReadLine());
    }

}