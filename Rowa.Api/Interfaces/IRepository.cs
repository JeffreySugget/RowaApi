using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rowa.Api.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Add(T newItem);

        void Update(T updatedItem);
    }
}
