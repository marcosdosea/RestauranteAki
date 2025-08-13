using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPessoa
    {
        public int Id { get; set; }

        public string NomeCompleto { get; set; }

        public string Cpf { get; set; } 

        public string? Email { get; set; }

        public string Telefone { get; set; } 

        public DateTime DataNascimento { get; set; }

        public string Cep { get; set; } 

        public string Logradouro { get; set; }

        public string Bairro { get; set; } 

        public string Cidade { get; set; } 

        public string Estado { get; set; } 

        public string? Complemento { get; set; }

        public byte[]? Foto { get; set; }

        public int IdRestaurante { get; set; }

        /// <summary>
        /// F - FUNCIONARIO
        /// G - GESTOR
        /// </summary>
        public string TipoPessoa { get; set; } 

        public  Restaurante IdRestauranteNavigation { get; set; } 

        public  ICollection<Pedido> Pedidos { get; set; } 
    }
}
