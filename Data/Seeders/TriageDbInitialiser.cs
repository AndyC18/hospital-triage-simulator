namespace RealNEA;

using System.Formats.Asn1;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
public static class TriageDbInitialiser
{
    public static void Initialise(HospitalsContext DBcontext)
    {
        //ensures its a fresh database
        DBcontext.Database.EnsureDeleted();
        DBcontext.Database.EnsureCreated();

        //if there arent any symptom questions in Db yet
        if (!DBcontext.SymptomQuestions.Any())
        {
            List<SymptomClass> symptoms = new List<SymptomClass>();
            if (!DBcontext.Symptoms.Any()) //if there are no symptoms defined in Db
            {
                //populates symptoms in
                symptoms.Add(new SymptomClass { SymptomName = "Fever", severityScore = 10 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Head Pain", severityScore = 15 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Chest Pain", severityScore = 30 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Abdomen Pain", severityScore = 10 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Limbs Pain", severityScore = 6 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Emergency Symptoms", severityScore = 100 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Dizziness", severityScore = 4 });
                symptoms.Add(new SymptomClass { SymptomName = "Nausea", severityScore = 4 }); 
                symptoms.Add(new SymptomClass { SymptomName = "Vomiting", severityScore = 15}); 

                //extra symptoms for simulation but not in triage tree
                symptoms.Add(new SymptomClass { SymptomName = "Cough", severityScore = 5 });
                symptoms.Add(new SymptomClass { SymptomName = "Sore Throat", severityScore = 3 });
                symptoms.Add(new SymptomClass { SymptomName = "Shortness of Breath", severityScore = 40 });
                symptoms.Add(new SymptomClass { SymptomName = "Back Pain", severityScore = 12 });
                symptoms.Add(new SymptomClass { SymptomName = "Rash", severityScore = 8 });
                symptoms.Add(new SymptomClass { SymptomName = "Fatigue", severityScore = 6 });
                symptoms.Add(new SymptomClass { SymptomName = "Diarrhoea", severityScore = 10 });
                symptoms.Add(new SymptomClass { SymptomName = "Joint Pain", severityScore = 9 });
                symptoms.Add(new SymptomClass { SymptomName = "Blurred Vision", severityScore = 20 });
                symptoms.Add(new SymptomClass { SymptomName = "Confusion", severityScore = 45 });
                symptoms.Add(new SymptomClass { SymptomName = "Swelling", severityScore = 15 });
                symptoms.Add(new SymptomClass { SymptomName = "Difficulty Speaking", severityScore = 50 });
                symptoms.Add(new SymptomClass { SymptomName = "Numbness", severityScore = 25 });
                symptoms.Add(new SymptomClass { SymptomName = "Ear Pain", severityScore = 5 });
                symptoms.Add(new SymptomClass { SymptomName = "Toothache", severityScore = 4 });
                symptoms.Add(new SymptomClass { SymptomName = "Muscle Cramps", severityScore = 5 });
                symptoms.Add(new SymptomClass { SymptomName = "Palpitations", severityScore = 30 });
                symptoms.Add(new SymptomClass { SymptomName = "Anxiety", severityScore = 7 });
            }
            DBcontext.Symptoms.AddRange(symptoms);
            DBcontext.SaveChanges();
        }
        
        //loads symptoms from db to use as references
        SymptomClass fever = DBcontext.Symptoms.First(a => a.SymptomName == "Fever"); 
        SymptomClass headPain = DBcontext.Symptoms.First(a => a.SymptomName == "Head Pain");
        SymptomClass chestPain = DBcontext.Symptoms.First(a => a.SymptomName == "Chest Pain");
        SymptomClass abdomenPain = DBcontext.Symptoms.First(a => a.SymptomName == "Abdomen Pain");
        SymptomClass limbsPain = DBcontext.Symptoms.First(a => a.SymptomName == "Limbs Pain");
        SymptomClass emergencySymptoms = DBcontext.Symptoms.First(a => a.SymptomName == "Emergency Symptoms");
        SymptomClass dizziness = DBcontext.Symptoms.First(a => a.SymptomName == "Dizziness");
        SymptomClass nausea = DBcontext.Symptoms.First(a => a.SymptomName == "Nausea");
        SymptomClass vomiting = DBcontext.Symptoms.First(a => a.SymptomName == "Vomiting");


        //defines questions with their severity scores
        SymptomQuestion q1 = new SymptomQuestion
        { Question = "Are you experiencing any of the following: Shortness of breath, Uncontrollable bleeding, Severe pain in the chest? Y/N", SeverityScore = 100 };
        SymptomQuestion q2 = new SymptomQuestion
        { Question = "Do you have a fever above 37 degrees Y/N", SeverityScore = 5, };
        SymptomQuestion q3 = new SymptomQuestion
        { Question = "Are you experiencing pain Y/N", SeverityScore = 3 };
        SymptomQuestion q5 = new SymptomQuestion
        { Question = "Where is the pain: Chest/Head/Abdomen/Limbs" };
        SymptomQuestion q9 = new SymptomQuestion
        { Question = null, SeverityScore = 4 };
        SymptomQuestion q10 = new SymptomQuestion
        { Question = null, SeverityScore = 5 };
        SymptomQuestion q11 = new SymptomQuestion
        { Question = null, SeverityScore = 3 };
        SymptomQuestion q12 = new SymptomQuestion
        { Question = null, SeverityScore = 2 };

        SymptomQuestion q4 = new SymptomQuestion
        { Question = "Do you feel Dizzy?", SeverityScore = 2 };
        SymptomQuestion q6 = new SymptomQuestion
        { Question = "Do you feel nauseous?", SeverityScore = 2 };
        SymptomQuestion q7 = new SymptomQuestion
        { Question = "Have you vomited?", SeverityScore = 3 };


        //adds the answers to the questions
        q1.SymptomAnswers = new List<SymptomAnswer>
            {
                new SymptomAnswer
                {
                    Answer = "Y",
                    Emergency = true,
                    Symptom = emergencySymptoms
                },
                new SymptomAnswer
                {
                    Answer = "N",
                    NextQuestion = q2
                }
            };

        q2.SymptomAnswers = new List<SymptomAnswer>
                {
                    new SymptomAnswer
                    {
                        Answer = "Y",
                        NextQuestion = q3,
                        Symptom = fever
                    },

                    new SymptomAnswer
                    {
                        Answer = "N",
                        NextQuestion = q3 
                    }
                };


        q3.SymptomAnswers = new List<SymptomAnswer>
                {
                    new SymptomAnswer
                    {
                        Answer = "Y",
                        NextQuestion = q5
                    },

                    new SymptomAnswer
                    {
                        Answer = "N",
                        NextQuestion = q4
                    }
                };

        q4.SymptomAnswers = new List<SymptomAnswer> //dizzy
        {
            new SymptomAnswer
            {
                Answer = "Y",
                NextQuestion = q6,
                Symptom = dizziness
            },

            new SymptomAnswer
            {
                Answer = "N",
                NextQuestion = q6
            }
        };

        q6.SymptomAnswers = new List<SymptomAnswer>//nausea
        {
            new SymptomAnswer
            {
                Answer = "Y",
                NextQuestion = q7,
                Symptom = nausea
            },

            new SymptomAnswer
            {
                Answer = "N",
                // NextQuestion = q8
            }
        };

        q7.SymptomAnswers = new List<SymptomAnswer> //vomiting
        {
            new SymptomAnswer
            {
                Answer = "Y",
                Symptom = vomiting
            },

            new SymptomAnswer
            {
                Answer = "N"
            }
        };

        q5.SymptomAnswers = new List<SymptomAnswer>
                {
                    new SymptomAnswer
                    {
                        Answer = "Chest",
                        NextQuestion = q9
                    },

                    new SymptomAnswer
                    {
                        Answer = "Head",
                        NextQuestion = q10
                    },
                    new SymptomAnswer
                    {

                        Answer = "Abdomen",
                        NextQuestion = q11
                    },
                    new SymptomAnswer
                    {

                        Answer = "Limbs",
                        NextQuestion = q12
                    }
                };


        q9.SymptomAnswers = new List<SymptomAnswer>
            {
                new SymptomAnswer
                {
                    Symptom = chestPain,
                    Answer = "chest pain"
                }
            };

        q10.SymptomAnswers = new List<SymptomAnswer>
            {
                new SymptomAnswer
                {
                    Symptom = headPain,
                    Answer = "head pain"
                }
            };
        q11.SymptomAnswers = new List<SymptomAnswer>
            {
                new SymptomAnswer
                {
                    Symptom = abdomenPain,
                    Answer = "abdomen pain"
                }
            };

        q12.SymptomAnswers = new List<SymptomAnswer>
            {
                new SymptomAnswer
                {
                    Symptom = limbsPain,
                    Answer = "limb pain"
                }
            };

        //adds data to db
        var questions = new List<SymptomQuestion> { q1, q2, q3, q4, q5, q6, q7, q9, q10, q11, q12 };
        DBcontext.SymptomQuestions.AddRange(questions);
        DBcontext.SaveChanges();
    }

  
}



