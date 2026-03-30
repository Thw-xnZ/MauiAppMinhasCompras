using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

// Aqui na ListaProduto.cs adicionamso um ItemSource para a ListView, que é a propriedade que define a fonte de dados para a exibição dos itens na lista.
public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    List<Produto> listaOriginal = new List<Produto>();

    public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    // O método OnAppearing é um método de ciclo de vida da página que é chamado quando a página está prestes a aparecer na tela, assim
    // mantendo a pagina sempre atualizada quando ela é recarregada
    protected async override void OnAppearing()
    {
        // foi adicionado o lista.Clear para resolver o Bug de duplicação de itens na lista, onde cada vez que a página é recarregada,
        // os itens são adicionados novamente à lista sem limpar os itens anteriores, resultando em uma duplicação dos itens exibidos na ListView.
        try
        { 
        lista.Clear();

            lista.Clear();

            listaOriginal = await App.Db.GetAll();

            listaOriginal.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    // Botão que foi criado para a utilização do ToolbarItem, para navegar para a página de cadastro de produto
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
        } 
        catch (Exception ex)
		{
			DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
			return;
        }
    }

    // O método txt_search_TextChanged é um manipulador de eventos que é acionado sempre que o texto do campo de busca (txt_search) é alterado,
    // permitindo que a lista de produtos seja atualizada dinamicamente com base na consulta de busca fornecida pelo usuário.
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try {
            lst_produtos.IsRefreshing = true;
            string busca = e.NewTextValue;
            string categoria = pckFiltroCategoria.SelectedItem?.ToString() ?? "Todos";

            AtualizarLista(busca, categoria);
        }  
        catch (Exception ex)
            {
                await DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    // Aqui é o botão para calcular o valor total dos produtos.
    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try { 
		double soma = lista.Sum(i => i.Total);

		string msg = $"O valor total é: {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "Ok");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    // Usando o comando pushAsync para navegar para a página de edição de produto, passando o produto selecionado como
    // contexto de ligação (BindingContext) para a nova página, permitindo que os detalhes do produto sejam exibidos e
    // editados na página de edição.

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto; 

            Navigation.PushAsync(new Views.EditarProduto { BindingContext = p });

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    // Botão de exclusão de um produto, onde o usuário pode clicar no item da lista e escolher a opção de excluir o produto selecionado.
    private async void MenuItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir o produto {p.Descricao}?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    // Novo menu de evento de refreshing para recarregar a pagina com o movimento de arrastar para cima
    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            lista.Clear();

            lst_produtos.IsRefreshing = true;

            listaOriginal = await App.Db.GetAll();

            listaOriginal.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private async void pckFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            {
                lst_produtos.IsRefreshing = true;

                string categoria = pckFiltroCategoria.SelectedItem?.ToString() ?? "Todos";
                string busca = txt_search.Text;

                AtualizarLista(busca, categoria);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", $"Erro no filtro: {ex.Message}", "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
    void AtualizarLista(string busca = "", string categoria = "Todos")
    {
        try
        {
            lst_produtos.IsRefreshing = true;
            var resultado = listaOriginal.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(busca))
        {
                resultado = resultado.Where(p =>
                (p.Descricao ?? "").ToLower().Contains(busca.ToLower()));
            }

        if (!string.IsNullOrEmpty(categoria) && categoria != "Todos")
        {
                resultado = resultado.Where(p =>
                (p.Categoria ?? "").ToLower() == categoria.ToLower());
            }

        lista.Clear();

        foreach (var item in resultado)
            lista.Add(item);
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", $"Erro no filtro: {ex.Message}", "OK");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
    string GerarRelatorioPorCategoria()
    {
        var grupos = listaOriginal
            .GroupBy(p => p.Categoria ?? "Sem categoria")
            .Select(g => new
            {
                Categoria = g.Key,
                Total = g.Sum(p => p.Total)
            })
            .OrderBy(g => g.Categoria)
            .ToList();

        string relatorio = "";

        foreach (var item in grupos)
        {
            relatorio += $"{item.Categoria}: {item.Total:C}\n";
        }

        return relatorio;
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            string relatorio = GerarRelatorioPorCategoria();

            await DisplayAlert("Relatório por Categoria", relatorio, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", $"Erro: {ex.Message}", "OK");
        }
    }
}