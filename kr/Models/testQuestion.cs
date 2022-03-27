using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kr.Models
{
    public class testQuestion
    {
        public test test { get; set; }
        public IEnumerable<question> questions { get; set; }
    }
}