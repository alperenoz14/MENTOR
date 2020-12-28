using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Mentor: User
    {
        public string description { get; set; }
        public int mentorId { get; set; }
        public int branchId { get; set; }
        public List<Branch> allBranchs { get; set; }
    }
}
