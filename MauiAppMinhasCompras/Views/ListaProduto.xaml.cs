using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

// Aqui na ListaProduto.cs adicionamso um ItemSource para a ListView, que é a propriedade que define a fonte de dados para a exibição dos itens na lista.
public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    // O método OnAppearing é um método de ciclo de vida da página que é chamado quando a página está prestes a aparecer na tela, assim
    // mantendo a pagina sempre atualizada quando ela é recarregada
    protected async override void OnAppearing()
    {
        try 
        { 
        lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i));
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
		string q = e.NewTextValue;

		lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

		tmp.ForEach(i => lista.Add(i)); 
        }  
        catch (Exception ex)
            {
                await DisplayAlert("Ops", $"Ocorreu um erro: {ex.Message}", "OK");
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

    // Projeto de exclusão de um produto, onde o usuário pode clicar no item da lista e escolher a opção de excluir o produto selecionado.
    

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
}