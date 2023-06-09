﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Umbuchung : BasisEntitaet
    {
        [Key]
        public int Id { get; set; }
        public string Anmerkung { get; set; }
        [Required]
        public LagerOrt Nach { get; set; }
        [Required]
        public Nutzer VerbuchtVon { get; set; }
        public Leiterplatte Leiterplatte { get; set; }
    }
}
