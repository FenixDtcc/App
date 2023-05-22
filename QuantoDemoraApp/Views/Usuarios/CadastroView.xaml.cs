using QuantoDemoraApp.Models;
using QuantoDemoraApp.ViewModels.Usuarios;

namespace QuantoDemoraApp.Views;

public partial class CadastroView : ContentPage
{
	UsuarioViewModel viewModel;

	public CadastroView()
	{
		InitializeComponent();

		viewModel= new UsuarioViewModel();
		BindingContext = viewModel;
        Title = "Novo Usuário";
    }
}