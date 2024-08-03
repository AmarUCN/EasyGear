using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public interface BasketDAO
    {
        void AddBasket(Basket basket);
        bool RemoveBasket(int basketId);
        Basket? GetBasketById(int basketId);
        IEnumerable<Basket> GetAllBaskets();
    }
}



