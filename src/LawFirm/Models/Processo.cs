using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LawFirm.Models
{
    public class Processo
    {
        [Key]
        public int ProcessoId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NumeroUnico { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }

    }
}
