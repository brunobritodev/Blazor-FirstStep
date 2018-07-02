using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

namespace BlazorBasico.Client.Pages
{
    public class ProdutoDataModel : BlazorComponent
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Parameter]
        protected string produtoId { get; set; } = "";
        [Parameter]
        protected string action { get; set; }

        protected List<BlazorBasico.Shared.Models.Produto> listaProdutos;
        protected BlazorBasico.Shared.Models.Produto produto = new BlazorBasico.Shared.Models.Produto();
        protected string title { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (action == "listar")
            {
                await ObterProdutos();
                this.StateHasChanged();
            }
            else if (action == "novo")
            {
                title = "Adicionar Produto";
                produto = new BlazorBasico.Shared.Models.Produto();
            }
            else if (int.TryParse(produtoId, out int _))
            {
                if (action == "atualizar")
                {
                    title = "Atualizar Produto";
                }
                else if (action == "remover")
                {
                    title = "Remover Produto";
                }

                produto = await Http.GetJsonAsync<BlazorBasico.Shared.Models.Produto>($"/api/produto/detalhes/{produtoId}");
            }
        }

        protected async Task ObterProdutos()
        {
            title = "Produtos";
            listaProdutos = await Http.GetJsonAsync<List<BlazorBasico.Shared.Models.Produto>>("api/produto/todos");
        }

        protected async Task CriarProduto()
        {
            if (produto.Id != 0)
            {
                await Http.SendJsonAsync(HttpMethod.Put, "api/produto/atualizar", produto);
            }
            else
            {
                await Http.SendJsonAsync(HttpMethod.Post, "/api/produto/novo", produto);
            }
            UriHelper.NavigateTo("/produto/listar");
        }

        protected async Task RemoverProduto()
        {
            await Http.DeleteAsync($"api/produto/remover/{produtoId}");
            UriHelper.NavigateTo("/produto/listar");
        }

        protected void Cancelar()
        {
            title = "Produto";
            UriHelper.NavigateTo("/produto/listar");
        }
    }
}