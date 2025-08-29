using System.Collections.Generic;
using System.Web.Http;
using AuctionLotManager.Models;
using AuctionLotManager.Repositories;

namespace AuctionLotManager.Controllers.Api
{
    public class LotsApiController : ApiController
    {
        private readonly LotRepository repo = new LotRepository();

        // GET api/lots
        [HttpGet]
        [Route("api/lots")]
        public IEnumerable<Lot> GetLots()
        {
            return repo.GetAllLots();
        }

        // GET api/lots/5
        [HttpGet]
        [Route("api/lots/{id}")]
        public IHttpActionResult GetLot(int id)
        {
            var lot = repo.GetLotById(id);
            if (lot == null)
                return NotFound();

            return Ok(lot);
        }
    }
}