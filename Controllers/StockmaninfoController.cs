using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTestAtlant.Models;
using WebTestAtlant.ViewModels;

namespace WebTestAtlant.Controllers
{
    [Produces("application/json")]
    [Route("api/stockmaninfo")]
    public class StockmaninfoController : Controller
    {

        ApplicationContext db;

        public StockmaninfoController(ApplicationContext context)
        {
            db = context;
        }


        [HttpGet]
        public IEnumerable<StockmanDetail> Get()
        {

            List<StockmanDetail> listDetail = new List<StockmanDetail>();

            var result = (from p1 in db.Stockmans
                          join p2 in db.Details on p1.Id equals p2.StockmanId
                          where p2.DeleteDate == null
                          group p2 by p2.Stockman.FIO
                          into g
                          select new { g.Key, SumKol = g.Sum(s => s.Quantity) });


            foreach (var item in result)
            {
                listDetail.Add(new StockmanDetail(item.Key, item.SumKol));
            }

            return listDetail;
        }

    }
}