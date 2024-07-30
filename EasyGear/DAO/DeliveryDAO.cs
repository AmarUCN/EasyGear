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
        void AddOrder(Delivery delivery);
        bool DeleteOrder(int orderNumber);
        IEnumerable<Delivery> GetAllOrders();
        Delivery? GetOrderById(int orderNumber);
        bool UpdateOrder(Delivery delivery);
    }
}
