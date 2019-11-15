using System;
using System.Linq;
using System.Collections.Generic;

namespace NgSight.API
{
    public class PaginatedResponse<T>
    {
        public PaginatedResponse(IEnumerable<T> data, int pageNumber, int pageSize)
        {
            //[1] page , 10 results
            Data = data.Skip((pageNumber-1) * pageSize).Take(pageSize).ToList();
            Total = data.Count();
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}