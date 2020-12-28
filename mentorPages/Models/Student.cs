using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Student: User
    {
        public int studentId { get; set; }
        public int branchId { get; set; }
        public string branch { get; set; }
        
        
    }
}
