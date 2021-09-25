using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Domain.Model.PokeAPI
{

    public class PokeAPIAbility
    {
        public List<object> effect_changes { get; set; }
        public List<Effect_Entries> effect_entries { get; set; }
        public List<Flavor_Text_Entries> flavor_text_entries { get; set; }
        public InnerProp generation { get; set; }
        public int id { get; set; }
        public bool is_main_series { get; set; }
        public string name { get; set; }
        public List<Name> names { get; set; }
        public List<Pokemon> pokemon { get; set; }
    }

    public class Effect_Entries
    {
        public string effect { get; set; }
        public InnerProp language { get; set; }
        public string short_effect { get; set; }
    }

    public class InnerProp
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Flavor_Text_Entries
    {
        public string flavor_text { get; set; }
        public InnerProp language { get; set; }
        public InnerProp version_group { get; set; }
    }
    public class Name
    {
        public InnerProp language { get; set; }
        public string name { get; set; }
    }

    public class Pokemon
    {
        public bool is_hidden { get; set; }
        public InnerProp pokemon { get; set; }
        public int slot { get; set; }
    }


}
