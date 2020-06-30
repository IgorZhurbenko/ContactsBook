using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForms
{
    public class ContactInformation
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public List<string> Numbers { get; set; }
    }
}
