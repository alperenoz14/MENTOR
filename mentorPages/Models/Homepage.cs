using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MENTOR.Models
{
    public class Homepage
    {
        public IEnumerable<Question> Questions { get; set; }
        public Mentor MentorInfo { get; set; }
        public Question Question { get; set; }
        public Student Students { get; set; }// bir çok olursa mentor ogrenci bağlantısı Ienumerable<student> olacak...
    }
}
