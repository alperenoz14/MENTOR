using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Homepage
    {
        public List<Question> Questions { get; set; }
        public Mentor MentorInfo { get; set; }
        public Question Question { get; set; }
        public List<Student> Students { get; set; }
        public Answer Answer { get; set; }
        public Branch Branch { get; set; }
        public List<Branch> Branches { get; set; }
    }
}
