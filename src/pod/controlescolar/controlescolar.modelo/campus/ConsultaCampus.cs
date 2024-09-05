using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.campi;

public class ConsultaCampus : CampusBase
{
    public Guid Id { get; set; }
    public Guid? CampusPadreId { get; set; }
}
