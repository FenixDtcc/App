using QuantoDemoraApp.Models.Enuns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Models
{
    public class Logradouro
    {
        public int IdLogradouro { get; set; }
        public string DsLogradouro { get; set; }
    }
}
