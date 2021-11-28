using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.Contracts.Requests
{
    public class AddToCart
    {
        public Guid BookIdentifier { get; set; }
        public int Count { get; set; }
    }
}
