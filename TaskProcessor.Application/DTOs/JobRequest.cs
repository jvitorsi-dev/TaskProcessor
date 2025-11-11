using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskProcessor.Application.DTOs
{
    public class JobRequest
    {
        public string Tipo { get; set; } = string.Empty;
        public string Dados { get; set; } = string.Empty;
    }

}