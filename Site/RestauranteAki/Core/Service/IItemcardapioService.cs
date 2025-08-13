using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IItemcardapioService
    {
        int Create(Itemcardapio itemcardapio);
        void Edit(Itemcardapio itemcardapio);
        void Delete(int id);
        Itemcardapio? Get(int id);
        IEnumerable<Itemcardapio> GetAll();
    }
}
