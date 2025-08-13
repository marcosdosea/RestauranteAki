using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IMesaService
    {
        int Create(Mesa mesa);
        void Edit(Mesa mesa);
        void Delete(int id);
        Mesa? Get(int id);
        IEnumerable<Mesa> GetAll();

    }
}
