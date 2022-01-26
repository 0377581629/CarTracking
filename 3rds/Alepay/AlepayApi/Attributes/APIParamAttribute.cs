using System;
using System.Collections.Generic;
using System.Text;

namespace alepay.Attributes
{
    public class APIParamAttribute : Attribute
    {
        public string Name { get; set; }

        public bool Required { get; set; }
    }
}
