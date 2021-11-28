using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.Contracts.Response
{
    public class BookStoreResponse
    {
        public int RemainingCount { get; set; }
        public string Title { get; set; }
        public Guid BookIdentifier { get; set; }
        
    }
}
