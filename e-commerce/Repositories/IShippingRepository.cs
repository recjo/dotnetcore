using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using ecom.Entities;

namespace ecom.Repositories
{
    public interface IShippingRepository
    {
        List<ShippingCostsByQties> GetShipCosts();
        dynamic GetShipChargesPerCarrier(int items);
    }
}