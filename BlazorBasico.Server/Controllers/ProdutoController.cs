using System.Collections.Generic;
using System.Linq;
using BlazorBasico.Server.Context;
using BlazorBasico.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorBasico.Server.Controllers
{
    public class ProdutoController : Controller
    {
        private BlazorDBContext database;

        public ProdutoController()
        {
            database = new BlazorDBContext();
        }

        [HttpGet]
        [Route("api/produto/todos")]
        public IEnumerable<Produto> Todos()
        {
            return database.Produtos.ToList();
        }
        [HttpPost]
        [Route("api/produto/novo")]
        public void Novo([FromBody] Produto Produto)
        {
            if (ModelState.IsValid)
            {
                database.Produtos.Add(Produto);
                database.SaveChanges();
            }
        }
        [HttpGet]
        [Route("api/produto/detalhes/{id}")]
        public Produto Detalhes(int id)
        {
            return database.Produtos.Find(id);
        }
        [HttpPut]
        [Route("api/produto/atualizar")]
        public void Atualizar([FromBody]Produto Produto)
        {
            if (ModelState.IsValid)
            {
                database.Entry(Produto).State = EntityState.Modified;
                database.SaveChanges();
            }

        }
        [HttpDelete]
        [Route("api/produto/remover/{id}")]
        public void Remover(int id)
        {
            Produto produto = database.Produtos.Find(id);
            database.Produtos.Remove(produto);
            database.SaveChanges();
        }
    }
}