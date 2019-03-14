using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecom.Entities;

namespace ecom.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private StoreDbContext _context;

        public ShippingRepository(StoreDbContext context)
        {
            _context = context;
        }

        public List<ShippingCostsByQties> GetShipCosts()
        {
            return _context.ShippingCostsByQties.Select(p => p).OrderBy(o => o.CarrierId).OrderBy(o => o.QtyStart).ToList();
        }

        public dynamic GetShipChargesPerCarrier(int items)
        {
            var shippingcarriers = from carriers in _context.ShippingCarriers.AsEnumerable()
                                   join costs in _context.ShippingCostsByQties.AsEnumerable() 
                                   on carriers.ShippingCarrierId equals costs.CarrierId
                                   where costs.QtyEnd >= items && costs.QtyStart <= items
                                   orderby carriers.Sort ascending
                                   select new {
                                       CarrierName = carriers.CarrierName,
                                       CarrierDesc = carriers.CarrierDesc,
                                       Sort = carriers.Sort,
                                       QtyStart = costs.QtyStart,
                                       QtyEnd = costs.QtyEnd,
                                       Charge = costs.Charge

                                   };
            return shippingcarriers.ToList();
        }
    }
}