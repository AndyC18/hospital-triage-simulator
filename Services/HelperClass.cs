using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealNEA
{
    public static class HelperClass
    {
        static Random random = new Random();
        public static int CalculateDuration(PatientClass patient) //usually would be longer, but shorter to display algorithms
        {
            int baseTime = 3; 
            //severity^0.7
            double Time = Math.Pow(patient.severityScore, 0.7);  
            return baseTime + (int)Time + 1;
        }

        public static string randomName()
        {
                List<string> FirstNames = new List<string> {"Ava", "Noah", "Olivia", "Liam", "Emma", "Mason", "Sophia", "Lucas", "Isabella", "Ethan", 
                "Mia", "James", "Amelia", "Benjamin", "Harper", "Henry", "Evelyn", "Alexander", "Charlotte", "Jack",
                "Elijah", "Grace", "Daniel", "Chloe", "Michael", "Lily", "Samuel", "Aria", "Matthew", "Scarlett",
                "David", "Zoey", "Joseph", "Hannah", "Carter", "Layla", "Owen", "Mila", "Wyatt", "Nora",
                "John", "Ellie", "Leo", "Riley", "Isaac", "Penelope", "Gabriel", "Lillian", "Julian", "Addison",
                "Anthony", "Aurora", "Dylan", "Stella", "Caleb", "Paisley", "Nathan", "Violet", "Thomas", "Hazel",
                "Joshua", "Savannah", "Ezra", "Brooklyn", "Ryan", "Bella", "Luke", "Claire", "Asher", "Skylar",
                "Christopher", "Lucy", "Andrew", "Anna", "Lincoln", "Natalie", "Hudson", "Leah", "Grayson", "Audrey",
                "Mateo", "Allison", "Elias", "Samantha", "Isaiah", "Maya", "Charles", "Elena", "Josiah", "Kennedy",
                "Adam", "Sarah", "Miles", "Madeline", "Cooper", "Sophie", "Parker", "Eva", "Aaron", "Autumn",
                "Adrian", "Ruby", "Nolan", "Alice", "Cameron", "Sadie", "Christian", "Emilia", "Jeremiah", "Quinn",
                "Easton", "Piper", "Colton", "Josephine", "Robert", "Everly", "Angel", "Cora", "Landon", "Vivian",
                "Jonathan", "Gianna", "Connor", "Clara", "Brayden", "Peyton", "Jordan", "Lydia", "Ian", "Naomi",
                "Carson", "Eliana", "Axel", "Rylee", "Dominic", "Athena", "Brooks", "Maria", "Xavier", "Jade",
                "Jaxon", "Brielle", "Greyson", "Adeline", "Evan", "Liliana", "Gabriella", "Sawyer", "Margaret", "Jason",
                "Valentina", "Bentley", "Jasmine", "Micah", "Rose", "Ryder", "Isla", "Kayden", "Daisy", "Weston",
                "Harmony", "Silas", "Faith", "Vincent", "Alexandra", "Bennett", "Kylie", "Harrison", "Eloise", "Zachary",
                "Madelyn", "Tyler", "Julia", "Ayden", "Reagan", "Max", "Londyn", "Diego", "Arabella", "Wesley",
                "Morgan", "Kaiden", "Andrea", "Giovanni", "Ximena", "Jonah", "Catalina", "Chase", "Alina", "Maverick",
                "Sienna", "Kingston", "Bailey", "Rowan", "Genevieve", "Luis", "Esther", "George", "Amaya", "Ashton",
                "Freya", "Braxton", "Callie", "Carlos", "Brooke", "Ivan", "Rosalie", "Justin", "Athena", "Kevin",
                "Adriana", "Camila", "Damian", "Daniela", "Tristan", "Selena", "Omar", "Bianca", "Jayden", "Mabel",
                "Phoebe", "Marcos", "Tessa", "Hugo", "Zuri", "Rafael", "Noelle", "Emiliano", "Holly", "Francesca",
                "Andre", "Millie", "Enzo", "Sabrina", "Matteo", "Giselle", "Marco", "Anya", "Felix", "Dahlia",
                "Leonardo"};

                List<string> LastNames = new List<String>{"Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", 
                "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", 
                "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", 
                "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright",
                "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera",
                "Campbell", "Mitchell", "Carter", "Roberts", "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker", 
                "Cruz", "Edwards", "Collins", "Reyes", "Stewart", "Morris", "Morales", "Murphy", "Cook", "Rogers", "Gutierrez",
                "Ortiz", "Morgan", "Cooper", "Peterson", "Bailey", "Reed", "Kelly", "Howard", "Ramos", "Kim", "Cox",
                "Ward", "Richardson", "Watson", "Brooks", "Chavez", "Wood", "James", "Bennett", "Gray", "Mendoza",
                "Ruiz", "Hughes", "Price", "Alvarez", "Castillo", "Sanders", "Patel", "Myers", "Long", "Ross",
                "Foster", "Jimenez", "Powell", "Jenkins", "Perry", "Russell", "Sullivan", "Bell", "Coleman", "Butler",
                "Henderson", "Barnes", "Gonzales", "Fisher", "Vasquez", "Simmons", "Romero", "Jordan", "Patterson",
                "Alexander", "Hamilton", "Graham", "Reynolds", "Griffin", "Wallace", "Moreno", "West", "Cole", "Hayes",
                "Bryant", "Herrera", "Gibson", "Ellis", "Tran", "Medina", "Aguilar", "Stevens", "Murray", "Ford", "Castro",
                "Marshall", "Owens", "Harrison", "Fernandez", "Mcdonald", "Woods", "Washington", "Kennedy", "Wells", 
                "Vargas", "Henry", "Chen", "Freeman", "Webb", "Tucker", "Guzman", "Burns", "Crawford", "Olson",
                "Simpson", "Porter", "Hunter", "Gordon", "Mendez", "Silva", "Shaw", "Snyder", "Mason", "Dixon",
                "Munoz", "Hunt", "Hicks", "Holmes", "Palmer", "Wagner", "Black", "Robertson", "Boyd", "Rose",
                "Stone", "Salazar", "Fox", "Warren", "Mills", "Meyer", "Rice", "Schmidt", "Garza", "Daniels",
                "Ferguson", "Nichols", "Stephens", "Soto", "Weaver", "Ryan", "Gardner", "Payne", "Grant", 
                "Dunn", "Kelley", "Spencer", "Hawkins", "Arnold", "Pierce", "Vazquez", "Hansen", "Peters",
                "Santos", "Hart", "Bradley", "Knight", "Elliott", "Cunningham"};

                
            Random random = new Random();
            int index1 = random.Next(0, FirstNames.Count);
            int index2 = random.Next(0, LastNames.Count);
            string randomName = $"{FirstNames[index1]} {LastNames[index2]}";
            return randomName;
            
        }
    public static int weightedRandomSymptomCount( )
    {
        int roll = random.Next(0, 100); // 0-99

        if (roll < 50)       // 50% chance: 1-4 symptoms
            return random.Next(1, 5);
        else if (roll < 90)  // 40% chance: 5-7 symptoms
            return random.Next(5, 8);
        else if (roll < 99)  // 9% chance: 8-9 symptoms
            return random.Next(8, 10);
        else                 // 1% chance: exactly 10
            return 100;
    }
    }


}