using System;
using System.Collections.Generic;

namespace QuizApp_API.Models
{
    public partial class Participant
    {
        public int ParticipantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Score { get; set; }
        public int? TimeSpent { get; set; }
    }
}
