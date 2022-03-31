using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kr.Models
{
    public class test_insAnswer
    {
        public test_instance test_Instance { get; set; }
        public IEnumerable<answer> answers { get; set; }
    }
}