using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Question
    {
        public int questionId { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public int branchId { get; set; }
        public int studentId { get; set; }
        public int mentorId { get; set; }
        public DateTime date { get; set; }
    }
}
