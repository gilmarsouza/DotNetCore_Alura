using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Moq;

namespace Alura.CoisasAFazer.Testes
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void Dada_Tarefa_Com_Info_Validas_Deve_Incluir_No_DB()
        {
            //arrange
            var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;

            var contexto = new DbTarefasContext(options);
            var repo = new RepositorioTarefa(contexto);

            var handler = new CadastraTarefaHandler(repo);

            //act
            handler.Execute(comando); //SUT >> CadastraTarefaHandler

            //assert
            var tarefa = repo.ObtemTarefas(t => t.Titulo == "Estudar Xunit").FirstOrDefault();
            Assert.NotNull(tarefa);

            //Criar comando
            //Executar o comando
        }

        [Fact]
        public void Quando_Exception_for_lancada_ResutadoIsSucess_Eh_Falso()
        {
            //arrange
            var comando = new CadastraTarefa("Estudar Xunit", new Categoria("Estudo"), new DateTime(2019, 12, 31));

            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro na inclusão de tarefas"));
            
            var repo = mock.Object;
            var handler = new CadastraTarefaHandler(repo);

            //act
            CommandResult resultado = handler.Execute(comando); //SUT >> CadastraTarefaHandler

            Assert.False(resultado.IsSuccess);
        }


    }
}
