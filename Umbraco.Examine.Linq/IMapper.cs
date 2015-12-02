using Examine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umbraco.Examine.Linq
{
    public interface IMapper<T>
    {
        IEnumerable<T> Map(IEnumerable<SearchResult> results);
    }
}
