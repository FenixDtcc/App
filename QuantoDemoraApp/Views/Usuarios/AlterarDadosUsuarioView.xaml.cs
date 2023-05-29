using QuantoDemoraApp.ViewModels.Usuarios;

namespace QuantoDemoraApp.Views.Usuarios;

public partial class AlterarDadosUsuarioView : ContentPage
{
	DadosUsuarioViewModel dadosUsuarioViewModel;
	public AlterarDadosUsuarioView()
	{
		InitializeComponent();

        dadosUsuarioViewModel = new DadosUsuarioViewModel();
        BindingContext = dadosUsuarioViewModel;
        Title = "Alterar Dados";
    }
}