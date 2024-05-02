using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comunicaciones.modelo.whatsapp;

public class FormData
{
    public string Type { get; set; }
    public string MessagingProduct { get; set; }
    public byte[] File { get; set; }
}
