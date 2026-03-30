using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    // usamos o BindingContext para pegar o produto que foi clicado na lista, e atualizar os campos, com o metodo try catch para evitar um
    // crash do app, usando a mesma logica do cadastro, porem usando o metodo Update para atualizar o produto
    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txtDescricao.Text,
                Quantidade = Convert.ToDouble(txtQuantidade.Text),
                Preco = Convert.ToDouble(txtPreco.Text),
                Categoria = txtCategoria.Text
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}