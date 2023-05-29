﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantoDemoraApp.Models
{
    public class Associado
    {
        public int IdAssociado { get; set; }
        public string NomeAssociado { get; set; }
        public string SobrenomeAssociado { get; set; }
        public string Cpf { get; set; }
        public char Sexo { get; set; }
        public string DddCelular { get; set; }
        public string NroCelular { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
