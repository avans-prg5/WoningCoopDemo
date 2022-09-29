using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WoningCoopDotNet6.Models
{
    public class Woning
    {

        public Woning()
        {
            Bewoners = new List<Bewoner>();
        }
        public List<Bewoner> Bewoners { get; set; }
        public int Id { get; set; }
        public string Naam { get; set; }
    }
}
