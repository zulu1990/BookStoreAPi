using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.Contracts.Requests
{
    public class SearchBook
    {
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string BookIdentifier { get; set; } = string.Empty;

    }
}
