using System;
using System.Collections.Generic;
using System.Text;

namespace NETCoreSignalR.Domain.Model.PokeAPI
{

    public class PokeAPIPokemon
    {
        public List<Ability> abilities { get; set; }
        public int base_experience { get; set; }
        public List<InnerProp> forms { get; set; }
        public List<Game_Indices> game_indices { get; set; }
        public int height { get; set; }
        public List<Held_Items> held_items { get; set; }
        public int id { get; set; }
        public bool is_default { get; set; }
        public string location_area_encounters { get; set; }
        public List<Move> moves { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public List<object> past_types { get; set; }
        public InnerProp species { get; set; }
        public Sprites sprites { get; set; }
        public List<Stat> stats { get; set; }
        public List<Type> types { get; set; }
        public int weight { get; set; }
    }


    public class Sprites
    {
        public string back_default { get; set; }
        public object back_female { get; set; }
        public string back_shiny { get; set; }
        public object back_shiny_female { get; set; }
        public string front_default { get; set; }
        public object front_female { get; set; }
        public string front_shiny { get; set; }
        public object front_shiny_female { get; set; }
    }

    public class Icons
    {
        public string front_default { get; set; }
        public object front_female { get; set; }
    }

    public class Ability
    {
        public InnerProp ability { get; set; }
        public bool is_hidden { get; set; }
        public int slot { get; set; }
    }

    public class Game_Indices
    {
        public int game_index { get; set; }
        public InnerProp version { get; set; }
    }


    public class Held_Items
    {
        public InnerProp item { get; set; }
    }


    public class Move
    {
        public InnerProp move { get; set; }
        public List<Version_Group_Details> version_group_details { get; set; }
    }


    public class Version_Group_Details
    {
        public int level_learned_at { get; set; }
        public InnerProp move_learn_method { get; set; }
        public InnerProp version_group { get; set; }
    }
    public class Stat
    {
        public int base_stat { get; set; }
        public int effort { get; set; }
        public InnerProp stat { get; set; }
    }


    public class Type
    {
        public int slot { get; set; }
        public InnerProp type { get; set; }
    }


}
