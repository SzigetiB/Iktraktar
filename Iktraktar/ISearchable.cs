using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iktraktar.Models.Interfaces
{
    internal interface ISearchable<T>
    {
        T? FindById(int id);
        IEnumerable<T> FindAll(string name);
    }
}
