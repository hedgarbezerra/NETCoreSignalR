using LanguageExt;
using NETCoreSignalR.Domain.Model.PokeAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Domain.Interfaces
{
    public interface IPokeAPIConsumer
    {
        Task<Option<PokeAPIList>> GetPaginatedList(string query, CancellationToken cancellationToken = default);
        Task<Option<PokeAPIPokemon>> GetPokemon(int id, CancellationToken cancellationToken = default);
        Task<Option<PokeAPIPokemon>> GetPokemon(string name, CancellationToken cancellationToken = default);
        Task<Option<PokeAPIAbility>> GetPokemonAbility(int pokemonId, CancellationToken cancellationToken = default);
        Task<Option<T>> GetPokemonAPI<T>(string endpoint, CancellationToken cancellationToken = default);
    }
}
