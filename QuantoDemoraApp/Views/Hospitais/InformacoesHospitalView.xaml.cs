using QuantoDemoraApp.ViewModels.Hospitais;

namespace QuantoDemoraApp.Views.Hospitais;

public partial class InformacoesHospitalView : ContentPage
{
	private InformacoesHospitalViewModel informacoesViewModel;
	public InformacoesHospitalView()
	{
		InitializeComponent();

		informacoesViewModel = new InformacoesHospitalViewModel();
		BindingContext = informacoesViewModel;
		Title = "Informações do Hospital";
	}
}