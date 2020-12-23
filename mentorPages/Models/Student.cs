using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Student: User
    {
        public string branch { get; set; }
        public int branchId { get; set; }
        public int studentId { get; set; }
    }
}
