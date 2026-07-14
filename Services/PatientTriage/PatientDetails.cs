using System.Security.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace RealNEA;

//used in createPatientClass with Decisiontree
public static class PatientDetails
{
    public static PatientClass GetDetails()
    {
        string firstName = "";
        string lastName = "";
        DateTime DoB = DateTime.MinValue;
        string address = "";

        //gets name
        string[] nameArray = GetName().Split(' ');
        firstName = nameArray[0];
        lastName = nameArray[1];

        //gets DoB
        DoB = GetDoB();

        //gets Address
        address = GetAddress();

        return new PatientClass(firstName, lastName, DoB, address);

    }

    private static string GetName()
    {
        while (true)
        {
            try
            {
                System.Console.Write("Enter your first name followed by your last name: ");
                string fullName = Console.ReadLine();


                if (string.IsNullOrEmpty(fullName))
                {
                    System.Console.WriteLine("Empty Input. Please try again");
                    continue; //next iteration of the loop
                }

                if(!fullName.All(c => char.IsLetter(c) || c == ' '))
                {
                    System.Console.WriteLine("Must only be letters");
                    continue;
                }

                String[] nameArray = fullName.Split(' ');

                if (nameArray.Length < 2)
                {
                    System.Console.WriteLine("Please enter both first name and last name eg. 'John Smith'");
                    continue;
                }

                

                return fullName;
            }

            catch (Exception ex)
            {
                System.Console.WriteLine("Error, try again");
                continue;
            }
        }
    }

    private static DateTime GetDoB()
    {
        while (true)
        {
            try
            {
                System.Console.Write("Enter your Date of birth in the format DDMMYYYY. eg 12022001 for 12th feb 2001:");
                string Dob = Console.ReadLine();

                if (Dob.Length != 8)
                {
                    System.Console.WriteLine("Needs to be 8 numbers long try again");
                    continue;
                }

                if (Dob.All(char.IsDigit) == false)
                {
                    System.Console.WriteLine("Enter only numbers");
                    continue;
                }

                DateTime date = DateTime.ParseExact(Dob, "ddMMyyyy", null);
                return date;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error, try again");
                continue;
            }
        }
    }

    private static string GetAddress()
    {
        while (true)
        {
            try
            {
                System.Console.Write("Enter your Address: ");
                string address = Console.ReadLine();

                if (string.IsNullOrEmpty(address))
                {
                    System.Console.WriteLine("Empty Input, try again");
                    continue;
                }

                if (address.Length < 12)
                {
                    System.Console.WriteLine("Address not long enough, try again");
                    continue;
                }

                return address;
            }

            catch
            {
                System.Console.WriteLine("Error, try again");
                continue;
            }

        }
    }
}