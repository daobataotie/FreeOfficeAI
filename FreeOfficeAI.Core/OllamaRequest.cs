using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeOfficeAI.Core
{
    public class OllamaRequest
    {
        //public string Model { get; set; }

        public string Prompt { get; set; }

        public int[] Context { get; set; }

        public List<OllamaMessage> Messages { get; set; }
    }
}
