using QuantoDemoraApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Services.Especialidades
{
    public class AtendimentoService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://quantodemora.somee.com/api/atendimentos";

        private string _token;
        public AtendimentoService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<Atendimento>> GetAtendimentosByIdHospital(int hospitalId)
        {
            string urlComplementar = string.Format("{0}", hospitalId);
            ObservableCollection<Models.Atendimento> listaAtendimentos = await
                _request.GetAsync<ObservableCollection<Models.Atendimento>>(apiUrlBase + urlComplementar, _token);
            return listaAtendimentos;
        }
    }
}

