using QuantoDemoraApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Services.Especialidades
{
    public class EspecialidadeService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://quantodemora.somee.com/api/especialidades";

        private string _token;
        public EspecialidadeService (string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<Especialidade>> GetEspecialidadesAsync()
        {
            string urlComplementar = string.Format("{0}", "/Listar");
            ObservableCollection<Models.Especialidade> listaEspecialidades = await
            _request.GetAsync<ObservableCollection<Models.Especialidade>>(apiUrlBase + urlComplementar, _token);
            return listaEspecialidades;
        }

        public async Task<Especialidade> GetEspecialidadeAsync(int especialidadeId)
        {
            string urlComplementar = string.Format("/{0}", especialidadeId);
            var especialidade = await _request.GetAsync<Models.Especialidade>(apiUrlBase + urlComplementar, _token);
            return especialidade;
        }
    }
}

