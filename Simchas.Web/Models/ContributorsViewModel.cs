using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simchas.Data;

namespace Simchas.Web.Models
{
    public class ContributorsViewModel
    {
        public IEnumerable<ContributorPlus> Contributors { get; set; }
        public Simcha Simcha { get; set; }
    }
}