using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simchas.Data
{
    public class Contribution : Simcha
    {
        public int SimchaId { get; set; }
        public int ContributorId { get; set; }
        public int Amount { get; set; }
    }
}
