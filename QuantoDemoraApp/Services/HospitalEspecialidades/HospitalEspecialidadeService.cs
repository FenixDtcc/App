using QuantoDemoraApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Services.HospitalEspecialidades
{
    public class HospitalEspecialidadeService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://quantodemora.somee.com/api/hospitalespecialidades";

        private string _token;
        public HospitalEspecialidadeService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<HospitalEspecialidade>> GetHospitalEspecialidadesAsync()
        {
            string urlComplementar = string.Format("{0}", "/Listar");
            ObservableCollection<Models.HospitalEspecialidade> listaHospitalEspecialidades = await
            _request.GetAsync<ObservableCollection<Models.HospitalEspecialidade>>(apiUrlBase + urlComplementar, _token);
            return listaHospitalEspecialidades;
        }

        public async Task<HospitalEspecialidade> GetHospitalEspecialidadeAsync(int hospitalId)
        {
            string urlComplementar = string.Format("/{0}", hospitalId);
            var hospitalEspecialidades = await _request.GetAsync<Models.HospitalEspecialidade>(apiUrlBase + urlComplementar, _token);
            return hospitalEspecialidades;
        }
    }
}

