using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Models
{
    public class Atendimento
    {
        [Key]
        public string SenhaAtendimento { get; set; }
        public int IdAtendimento { get; set; }

        [NotMapped]
        public Hospital Hospital { get; set; }
        public int IdHospital { get; set; }

        [NotMapped]
        public Especialidade Especialidade { get; set; }
        public int IdEspecialidade { get; set; }
        public int TempoAtendimento { get; set; }
        public string TempoMedio { get; set; }
    }
}
