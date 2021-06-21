using System;
using System.Collections.Generic;

namespace QuizApp_API.Models
{
    public partial class QuestionCategory
    {
        public QuestionCategory()
        {
            Question = new HashSet<Question>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Question { get; set; }
    }
}
