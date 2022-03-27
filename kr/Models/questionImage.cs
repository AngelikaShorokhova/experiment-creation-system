using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kr.Models
{
    public class questionImage
    {
        public question question { get; set; }
        public IEnumerable<image> images { get; set; }

    }
}