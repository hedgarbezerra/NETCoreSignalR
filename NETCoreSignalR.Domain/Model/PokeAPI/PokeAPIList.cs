using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Domain.Model.PokeAPI
{

    public class PokeAPIList
    {
        public int count { get; set; }
        public string next { get; set; }
        public string previous { get; set; }
        public List<PokeResult> results { get; set; }
    }

    public class PokeResult
    {
        public string name { get; set; }
        public string url { get; set; }
    }

}
