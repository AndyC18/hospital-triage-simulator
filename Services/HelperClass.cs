using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealNEA
{
    public static class HelperClass
    {
        public static int CalculateDuration(PatientClass patient) //usually would be longer, but shorter to display algorithms
        {
            int baseTime = 3; 
            //severity^0.7
            double Time = Math.Pow(patient.severityScore, 0.7);  
            return baseTime + (int)Time + 1;
        }
    }
}