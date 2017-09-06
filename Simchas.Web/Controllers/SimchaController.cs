using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simchas.Data;
using Simchas.Web.Models;

namespace Simchas.Web.Controllers
{
    public class SimchaController : Controller
    {
        public ActionResult Index()
        {
            SimchasDB SDB = new SimchasDB(Properties.Settings.Default.ConStr);
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            SimchasViewModel svm = new SimchasViewModel();
            IEnumerable<Simcha> simchas = SDB.GetAllSimchas();
            svm.Simchas = SimchaToSimchaPlus(simchas);
            svm.ContributorsCount = CDB.TotalNumberContributors();
            if(TempData["SimchaAdded"] != null)
            {
                svm.NewSimcha = (Simcha)TempData["SimchaAdded"];
            }
            return View(svm);
        }
        public ActionResult NewSimcha()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addSimcha(Simcha s)
        {
            SimchasDB SDB = new SimchasDB(Properties.Settings.Default.ConStr);
            SDB.AddSimcha(s);
            TempData["SimchaAdded"] = s;
            return RedirectToAction("/index");
        }
        public ActionResult Contributors(int SimchaId)
        {
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            SimchasDB SDB = new SimchasDB(Properties.Settings.Default.ConStr);
            UpdateViewModel uvm = new UpdateViewModel();
            uvm.Simcha = SDB.GetById(SimchaId);
            IEnumerable<Contributor> contributors = CDB.GetAllContributors();
            List<SimchaContributor> result = new List<SimchaContributor>();
            foreach (Contributor c in contributors)
            {
                SimchaContributor sc = new SimchaContributor
                {
                    ContributorId = c.Id,
                    Name = c.FirstName + " " + c.LastName,
                    AlwaysInclude = c.AlwaysInclude,
                    Balance = CDB.GetBalance(c.Id),
                    Include = CDB.ContributedAlready(SimchaId, c.Id),
                    Amount = CDB.ContributedAmount(SimchaId, c.Id)
                };
                result.Add(sc);
            }
            uvm.SimchaContributorList = result;
            return View(uvm);
        }
        public ActionResult updateContributions(List<Contributing> contributing, int SimchaId)
        {
            ContributorsDB cdb = new ContributorsDB(Properties.Settings.Default.ConStr);
            cdb.UpdateContribution(contributing, SimchaId);
            return Redirect("/simcha/index");
        }

        public ActionResult List(int SimchaId)
        {
            SimchasDB SDB = new SimchasDB(Properties.Settings.Default.ConStr);
            ContributorsViewModel cvm = new ContributorsViewModel();
            cvm.Contributors = ContributorToContributorPlus(SDB.GetContributorsForSimcha(SimchaId));
            cvm.Simcha = SDB.GetById(SimchaId);
            return View(cvm);
        }

        private IEnumerable<SimchaPlus> SimchaToSimchaPlus(IEnumerable<Simcha> simchas)
        {
            SimchasDB SDB = new SimchasDB(Properties.Settings.Default.ConStr);
            ContributorsDB CDB = new ContributorsDB(Properties.Settings.Default.ConStr);
            List<SimchaPlus> result = new List<SimchaPlus>();
            foreach (Simcha s in simchas)
            {
                SimchaPlus sp = new SimchaPlus
                {
                    Id = s.Id,
                    Name = s.Name,
                    Date = s.Date,
                    TotalDonations = SDB.GetTotalDeposits(s.Id),
                    Contributors = CDB.NumberContributorsForSimcha(s.Id)
                };
                result.Add(sp);
            };
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
            return result.OrderByDescending(cp => cp.DateCreated);
        }

    }
}