using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    // Botão que foi criado para a utilização do ToolbarItem, para salvar o produto no banco de dados
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			Produto p = new Produto
			{
				Descricao = txtDescricao.Text,
				Quantidade = Convert.ToDouble(txtQuantidade.Text),
				Preco = Convert.ToDouble(txtPreco.Text),
				Categoria = txtCategoria.Text
			};

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso!", "Produto Adicionado", "OK");
			await Navigation.PopAsync();

        } catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}
}