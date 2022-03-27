using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kr.Models
{
    public class childrenQuestion
    {
        public int id { get; set; }
        public question question { get; set; }
        public IEnumerable<image> images { get; set; }
        public test_instance test_Instance { get; set; }

    }
}