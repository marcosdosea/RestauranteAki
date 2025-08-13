using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IContumService
    {
        int Create(Contum contum);
        void Edit(Contum contum);
        void Delete(int id);
        Contum? Get(int id);
        IEnumerable<Contum> GetAll();
    }
}
