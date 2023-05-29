using QuantoDemoraApp.Models.Enuns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Models
{
    public class HospitalEspecialidade
    {
        public int idHospital { get; set; }
        public int idEspecialidade { get; set; }
    }
}
