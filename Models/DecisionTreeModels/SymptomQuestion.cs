namespace RealNEA
{
    public class SymptomQuestion //SymptomQuestion Table
    {
        public int SymptomQuestionId { get; internal set; } //Primary Key, also forign Key for answer table (next question field)
        public string? Question { get; internal set; }
        public int SeverityScore { get; internal set; }
        public List<SymptomAnswer> SymptomAnswers { get; internal set; } = new List<SymptomAnswer>();//represents a one to many relationship with the SymptomAnswer table
    }
}