using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IRestauranteService
    {  
        int Create(Restaurante restaurante);
        void Edit(Restaurante restaurante);
        void Delete(int id);
        Restaurante? Get(int id);
        IEnumerable<Restaurante> GetAll();
    }
}
