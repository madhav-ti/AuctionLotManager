using AuctionLotManager.Models;
using AuctionLotManager.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AuctionLotManager.Controllers
{
    public class LotController : Controller
    {
        private readonly LotRepository _repo = new LotRepository();

        // GET: Lot
        //public ActionResult Index()
        //{
        //    var lots = _repo.GetAllLots();
        //    return View(lots);
        //}

        public ActionResult Index(string searchTerm, bool showActive = false)
        {
            IEnumerable<Lot> lots;

            if (showActive)
            {
                lots = _repo.GetActiveLots();
            }
            else
            {
                lots = _repo.GetAllLots();
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                lots = lots.Where(l => l.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            return View(lots);
        }

        // GET: Lot/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lot/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Lot lot)
        {
            if (ModelState.IsValid)
            {
                _repo.AddLot(lot);
                return RedirectToAction("Index");
            }
            return View(lot);
        }

        public ActionResult Edit(int id)
        {
            var lot = _repo.GetLotById(id);
            if (lot == null) return HttpNotFound();
            return View(lot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lot lot)
        {
            if (ModelState.IsValid)
            {
                _repo.UpdateLot(lot);
                return RedirectToAction("Index");
            }
            return View(lot);
        }

        public ActionResult Delete(int id)
        {
            var lot = _repo.GetLotById(id);
            if (lot == null) return HttpNotFound();
            return View(lot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repo.DeleteLot(id);
            return RedirectToAction("Index");
        }
    }
}