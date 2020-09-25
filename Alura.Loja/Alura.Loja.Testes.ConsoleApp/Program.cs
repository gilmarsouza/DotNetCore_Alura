using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cliclano = new Cliente();
            cliclano.Nome = "Ciclano de Tal";
            cliclano.EnderecoDeEntrega = new Endereco()
            {
                Numero = 13,
                Logradouro = "Rua dos Bobos",
                Complemento = "sobrado",
                Bairro = "Centro",
                Cidade = "Pindamonhagaba"
            };

            using (var contexto = new LojaContext())
            {
                VerSqlRodando(contexto);

                contexto.Clientes.Add(cliclano);
                contexto.SaveChanges();
            }
         
        }

        private static void MuitosParaMuitos()
        {
            var p1 = new Produto() { Nome = "Suco de Laranja", Categoria = "Bebidas", PrecoUnitario = 8.79, Unidade = "Litros" };
            var p2 = new Produto() { Nome = "Café", Categoria = "Bebidas", PrecoUnitario = 12.45, Unidade = "Gramas" };
            var p3 = new Produto() { Nome = "Macarrão", Categoria = "Alimentos", PrecoUnitario = 4.23, Unidade = "Gramas" };

            var promocaoDePascoa = new Promocao();
            promocaoDePascoa.Descricao = "Páscoa Du Mal";
            promocaoDePascoa.DataInicial = DateTime.Now;
            promocaoDePascoa.DataTermino = DateTime.Now.AddMonths(3);

            promocaoDePascoa.IncluiProduto(p1);
            promocaoDePascoa.IncluiProduto(p2);
            promocaoDePascoa.IncluiProduto(p3);

            using (var contexto = new LojaContext())
            {
                VerSqlRodando(contexto);

                //contexto.Promocoes.Add(promocaoDePascoa);
                //contexto.SaveChanges();
                var promocao = contexto.Promocoes.Find(1);
                contexto.Promocoes.Remove(promocao);
                contexto.SaveChanges();
            }
        }

        private static void VerSqlRodando(LojaContext contexto)
        {
            var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(SqlLoggerProvider.Create());
        }


        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }

    }
}
