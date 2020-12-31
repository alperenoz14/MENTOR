using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class TotalModel
    {
        public User user { get; set; }
        public Mentor mentor { get; set; }
        public Student student { get; set; }
        public List<Branch> branch { get; set; }
    }
}
