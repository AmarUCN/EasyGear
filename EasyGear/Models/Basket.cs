using DAL.DB;
using DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        public Basket(int id, DateTime createdAt )
        {
            Id = id;
            CreatedAt = createdAt;
        }

        public Basket(DateTime createdAt)
        {
            CreatedAt = createdAt;
        }

        public Basket() { }
    }
}


