using QuantoDemoraApp.ViewModels.Hospitais;

namespace QuantoDemoraApp.Views.Hospitais;

public partial class ListagemView : ContentPage
{
	ListagemHospitalViewModel viewModel;

	public ListagemView()
	{
		InitializeComponent();

		viewModel= new ListagemHospitalViewModel();	
		BindingContext = viewModel;
		Title = "Hospitais";
	}
}