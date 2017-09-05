using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simchas.Data;
using Simchas.Web.Models;

namespace Simchas.Web.Controllers
{
    public class ContributorController : Controller
    {
        public ActionResult Index()
        {
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            ContributorsViewModel cvm = new ContributorsViewModel();
            IEnumerable<Contributor> contributors = CDB.GetAllContributors();
            cvm.Contributors = ContributorToContributorPlus(contributors);
            return View(cvm);
        }
        public ActionResult NewContributor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddContributor(Contributor c, int deposit)
        {
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            int cId = CDB.AddContributor(c);
            CDB.AddDeposit(deposit, cId);
            return RedirectToAction("/index");
        }
        public ActionResult NewDeposit(int ContributorId)
        {
            DepositViewModel dvm = new DepositViewModel();
            dvm.ContributorId = ContributorId;
            return View(dvm);
        }
        [HttpPost]
        public ActionResult AddDeposit(int deposit, int ContributorId)
        {
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            CDB.AddDeposit(deposit, ContributorId);
            return RedirectToAction("/index");
        }
        public ActionResult showHistory(int ContributorId)
        {
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            HistoryViewModel hvm = new HistoryViewModel();
            IEnumerable<Deposit> d = CDB.GetDepositsForContributor(ContributorId);
            IEnumerable<Contribution> c = CDB.GetContributionsForContributor(ContributorId);
            hvm.Transactions = Transactions(c, d).OrderBy(t=>t.Date);
            hvm.Balance = CDB.GetBalance(ContributorId);
            hvm.Contributor = CDB.GetById(ContributorId);
            return View(hvm);
        }
        private IEnumerable<Transaction> Transactions(IEnumerable<Contribution> contributions, IEnumerable<Deposit> depostis)
        {
            List<Transaction> result = new List<Transaction>();
            foreach(Contribution c in contributions)
            {
                result.Add(new Transaction
                {
                    Amount = c.Amount *-1,
                    Date = c.Date,
                    Description = "Contribution for " + c.Name
                });
            }
            foreach (Deposit d in depostis)
            {
                result.Add(new Transaction
                {
                    Amount = d.Amount,
                    Date = d.Date,
                    Description = "Deposit"
                });
            }
            return result;
        }
        private IEnumerable<ContributorPlus> ContributorToContributorPlus(IEnumerable<Contributor> contributors)
        {
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            List<ContributorPlus> result = new List<ContributorPlus>();
            foreach (Contributor c in contributors)
            {
                ContributorPlus cp = new ContributorPlus
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DateCreated = c.DateCreated,
                    AlwaysInclude = c.AlwaysInclude,
                    PhoneNumber = c.PhoneNumber,
                    Balance = CDB.GetBalance(c.Id)
                };
                result.Add(cp);
            };
            result.OrderByDescending(cp => cp.DateCreated);
            return result;
        }

    }
}