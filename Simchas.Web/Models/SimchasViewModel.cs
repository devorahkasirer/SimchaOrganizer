using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simchas.Data;

namespace Simchas.Web.Models
{
    public class SimchasViewModel
    {
        public IEnumerable<SimchaPlus> Simchas { get; set; }
        public int ContributorsCount { get; set; }
        public Simcha NewSimcha { get; set; }
    }
}