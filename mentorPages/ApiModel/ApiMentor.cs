using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.ApiModel
{
    public class ApiMentor
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string description { get; set; }
        public int branchId{ get; set; }

        //public List<Branch> allBranchs{ get; set; }
    }
}
