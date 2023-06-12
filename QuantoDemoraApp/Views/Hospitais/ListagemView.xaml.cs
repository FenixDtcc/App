using QuantoDemoraApp.ViewModels.Hospitais;
using QuantoDemoraApp.ViewModels.Usuarios;

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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.ObterHospital();
    }

    //private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    //{

    //}
}