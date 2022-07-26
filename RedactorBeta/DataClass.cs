using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedactorBeta
{
    //instance of this class is serializable via json
    public class DataClass
    {
        public bool isVisible { get; set; } = false;
        public bool hasPassword { get; set; } = false;
        public List<MyTuple> loginsAndPasswords { get; set; } = new List<MyTuple>();
    }
    public class MyTuple
    {
        public string Log { get; set; } = "";
        public string Pas { get; set; } = "";
    }
}
