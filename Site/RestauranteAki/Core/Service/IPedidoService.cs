using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPedidoService
    {
       int create(Pedido pedido);
       void edit(Pedido pedido);
       void delete(int id);
       Pedido? get(int id);
       IEnumerable<Pedido> getAll();
    }
}
