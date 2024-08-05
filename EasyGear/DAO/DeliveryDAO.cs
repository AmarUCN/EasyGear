using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public interface DeliveryDAO
    {
        int AddOrder(Delivery delivery);
        bool DeleteOrder(int id);
        IEnumerable<Delivery> GetAllOrders();
        Delivery? GetOrderById(int id);
        bool UpdateOrder(Delivery delivery);
    }
}

