﻿using System;
using System.Collections.Generic;
namespace SistemaBase.Models
{
    public partial class Pasatiempo
    {
        public string? CodPersona { get; set; }
        public string? Descripcion { get; set; }
        public Guid Rowid { get; set; }
    }
}