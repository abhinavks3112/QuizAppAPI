using System;
using System.Collections.Generic;

namespace QuizApp_API.Models
{
    public partial class Question
    {
        public int QnID { get; internal set; }
        public int CategoryId { get; set; }
        public string Qn { get; set; }
        public string ImageName { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public int? Answer { get; set; }
        public string Comment { get; set; }

        public virtual QuestionCategory Category { get; set; }
    }
}
