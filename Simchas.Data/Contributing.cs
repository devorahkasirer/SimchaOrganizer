﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simchas.Data
{
    public class Contributing : ContributorPlus
    {
        public bool Include { get; set; }
        public int Amount { get; set; }
        public int ContributorId { get; set; }
    }
}
