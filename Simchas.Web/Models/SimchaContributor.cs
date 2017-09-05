using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simchas.Web.Models
{
    public class SimchaContributor
    {
        public int ContributorId { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        public bool Include { get; set; }
        public int Amount { get; set; }
        public bool AlwaysInclude { get; set; }

    }
}
