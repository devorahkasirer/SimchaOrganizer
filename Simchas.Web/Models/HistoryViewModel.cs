using Simchas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simchas.Web.Models
{
    public class HistoryViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public int Balance { get; set; }
        public Contributor Contributor { get; set; }
    }
}