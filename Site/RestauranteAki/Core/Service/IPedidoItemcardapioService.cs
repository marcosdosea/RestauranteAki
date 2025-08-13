using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPedidoItemcardapioService
    {
        int create(PedidoItemcardapio pedidoItemcardapio);
        void edit(PedidoItemcardapio pedidoItemcardapio);
        void delete(int id);
        PedidoItemcardapio? get(int id);
        IEnumerable<PedidoItemcardapio> getAll();
    }
}
