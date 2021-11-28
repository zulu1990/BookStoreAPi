using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.Contracts.Entity
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public Guid BookIdentifier { get; set; }

        public bool Locked { get; set; }
       

        public long? CartId { get; set; }
        public Cart Cart { get; set; }

        public long? PurchaseHistoryId { get; set; }
        public PurchaseHistory PurchaseHistory { get; set; }
    }
}
