using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
namespace RealNEA
{
    //overview: Decision tree traversing the question nodes, if response is not N, adds the severity score from that question and symptom
    //goes to the next question from the NextQuestion in the answer, changes current node, repeats until root node

    public class TriageDecisionTree
    {
        private HospitalsContext triageContext;
        public TriageDecisionTree(HospitalsContext context)
        {
            triageContext = context;
        }
        
        //returns severity score and all associated symptoms
        public (int severityScore, List<SymptomClass> symptoms) RunDecisionTree() 
        {
            //each node is a symptomQuestion, starting from the first node
            SymptomQuestion? currentNode = triageContext.SymptomQuestions 
            .Include(q => q.SymptomAnswers) 
                .ThenInclude(a => a.NextQuestion)
            .Include(q => q.SymptomAnswers)
            .ThenInclude(a => a.Symptom)
            .FirstOrDefault(q => q.SymptomQuestionId == 1); //root node

            int severityScore = 0;

            //list of symptoms for the patient
            List<SymptomClass> Symptoms = new List<SymptomClass>(); 


            if (currentNode == null)
            {
                throw new Exception("No root node found");
            }

            if (!currentNode.SymptomAnswers.Any())
            {
                throw new Exception("Root node has no answers");
            }

            while (currentNode != null)
            {
                //if leaf node reached (leaf node has no question but has symptoms)
                if (string.IsNullOrEmpty(currentNode.Question)) 
                {
                    foreach (SymptomAnswer answer in currentNode.SymptomAnswers)
                    {
                        Symptoms.Add(answer.Symptom);
                        severityScore += currentNode.SeverityScore;
                    }
                    break; //leave the loop as leaf node is reached
                }

                //Prints questions
                System.Console.WriteLine($"{currentNode.Question}");
                int index = 1;
                foreach (SymptomAnswer answer in currentNode.SymptomAnswers)
                {
                    System.Console.WriteLine($"{index}. {answer.Answer}");
                    index++;
                }

                //Gets answer
                Console.Write("Response: ");
                string response = Console.ReadLine()?.ToLower() ?? string.Empty; //null check ?? means if left is null use right

                //Traverses to the next question
                bool validAnswer = false;

                while (!validAnswer)
                {
                    SymptomAnswer? selectedAnswer = null;

                    //finds matching answer
                    foreach (SymptomAnswer answer in currentNode.SymptomAnswers)
                    {
                        if (answer.Answer?.ToLower() == response)
                        {
                            selectedAnswer = answer;
                            validAnswer = true;
                            break;
                        }
                    }

                    //only runs if the input matched
                    if (selectedAnswer != null)
                    {
                        //if the patient has the symptom, add the symptom and increase score
                        if (response != "n" && selectedAnswer.Symptom != null)
                        {
                            Symptoms.Add(selectedAnswer.Symptom);
                            severityScore += currentNode.SeverityScore;
                        }

                        if (selectedAnswer.Emergency == true)
                        {
                            severityScore += 100;
                        }

                        //traverses the next node
                        currentNode = selectedAnswer.NextQuestion;
                    }

                    if (!validAnswer)
                    {
                        Console.Write("Not a valid answer. Retry: ");
                        response = Console.ReadLine()?.ToLower() ?? string.Empty;
                    }
                }
            }

            System.Console.WriteLine("Weight / Kg or press enter to skip");
            string weight = Console.ReadLine();
            if (double.TryParse(weight, out double patientWeight)) //if weight can be a double, stored as patientWeight
            {
                if (patientWeight < 50)
                {
                    severityScore += 1;
                }
                else if (patientWeight > 90)
                {
                    severityScore += 2;
                }
            }

            System.Console.WriteLine("Blood pressure / mmHG or enter to skip");
            string bloodPressure = Console.ReadLine();
            if (double.TryParse(bloodPressure, out double patientBloodPressure))
            {
                if (patientBloodPressure > 80)
                {
                    severityScore += 2;
                }
                else if (patientBloodPressure < 120)
                {
                    severityScore += 2;
                }
            }

            return (severityScore, Symptoms);
        }
    }
}