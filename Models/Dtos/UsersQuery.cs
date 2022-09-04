using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class UsersQuery
    {
        public string SearchPhrase { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string SortBy { get; set; } = string.Empty;
        public bool SortDirection { get; set; } = false;
    }
}
