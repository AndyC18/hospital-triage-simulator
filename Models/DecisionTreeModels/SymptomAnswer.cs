namespace RealNEA
{
    public class SymptomAnswer
    {
        public int SymptomAnswerId { get; internal set; } //PK
        public int SymptomQuestionId { get;internal set; } 
        public string? Answer { get; internal set; } //answer to previous question
        public int? NextQuestionID { get; internal set; } //FK to Question table, is nullable as it may be a leaf node
        public bool Emergency { get; internal set; }
        public int? SymptomClassId { get; internal set; } //has a symptom linked which will be added to patients details if traversed
        public SymptomClass? Symptom { get; internal set; }

        //foreign keys
        public SymptomQuestion? SymptomQuestion { get; internal set; } //parent node
        public SymptomQuestion? NextQuestion { get; internal set; } //optional proceeeding question

    }
}