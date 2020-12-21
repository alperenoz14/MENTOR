using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Mentor: User
    {
        public string Description { get; set; }
        public int mentorId { get; set; }
    }
}
