using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.Contracts.Entity
{
    public class PurchaseHistory :  BaseEntity
    {
        public User User { get; set; }
        public long UserId { get; set; }

        public List<Book> Books { get; set; }
    }
}
