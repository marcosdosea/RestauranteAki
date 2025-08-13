using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IRestaurante
    {
        public int Id { get; set; }

        public string Nome { get; set; } 

        public string NomeFantasia { get; set; } 

        public string Cnpj { get; set; } 

        public string Endereco { get; set; }

        public string Bairro { get; set; } 

        public string Estado { get; set; } 

        public string Cidade { get; set; } 

        public string? Complemento { get; set; }

        public  ICollection<Cardapio> Cardapios { get; set; } 

        public  ICollection<Pessoa> Pessoas { get; set; } 
    }
}
