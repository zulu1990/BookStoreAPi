using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.Contracts.Requests
{
    public class AddBook
    {
        public Guid BookIdentifier { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public int Count { get; set; }
    }
}
