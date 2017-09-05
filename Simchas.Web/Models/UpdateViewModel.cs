﻿using Simchas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simchas.Web.Models
{
    public class UpdateViewModel
    {
        public Simcha Simcha { get; set; }
        public IEnumerable<SimchaContributor> SimchaContributorList { get; set; }
    }
}