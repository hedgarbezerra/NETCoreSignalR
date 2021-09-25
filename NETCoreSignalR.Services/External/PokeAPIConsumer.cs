using LanguageExt;
using NETCoreSignalR.Domain.Model.PokeAPI;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Services.External.PokeAPI
{
    public interface IPokeAPIConsumer
    {
        Task<Option<PokeAPIList>> GetPaginatedList(string query, CancellationToken cancellationToken = default);
        Task<Option<PokeAPIPokemon>> GetPokemon(int id, CancellationToken cancellationToken = default);
        Task<Option<PokeAPIPokemon>> GetPokemon(string name, CancellationToken cancellationToken = default);
        Task<Option<PokeAPIAbility>> GetPokemonAbility(int pokemonId, CancellationToken cancellationToken = default);
        Task<Option<T>> GetPokemonAPI<T>(string endpoint, CancellationToken cancellationToken = default);
    }

    public class PokeAPIConsumer : IPokeAPIConsumer
    {
        private readonly IHttpConsumer _httpConsumer;
        private const string _baseUrl = "https://pokeapi.co/api/v2";
        public PokeAPIConsumer(IHttpConsumer httpConsumer)
        {
            _httpConsumer = httpConsumer;
        }

        public async Task<Option<PokeAPIList>> GetPaginatedList(string query, CancellationToken cancellationToken = default) =>
            await _httpConsumer.GetAsync<PokeAPIList>($"{_baseUrl}/pokemon?{query}", cancellationToken);

        public async Task<Option<PokeAPIPokemon>> GetPokemon(string name, CancellationToken cancellationToken = default) =>
            await _httpConsumer.GetAsync<PokeAPIPokemon>($"{_baseUrl}/pokemon/{name}", cancellationToken);

        public async Task<Option<PokeAPIPokemon>> GetPokemon(int id, CancellationToken cancellationToken = default) =>
           await _httpConsumer.GetAsync<PokeAPIPokemon>($"{_baseUrl}/pokemon/{id.ToString()}", cancellationToken);

        public async Task<Option<PokeAPIAbility>> GetPokemonAbility(int pokemonId, CancellationToken cancellationToken = default) =>
         await _httpConsumer.GetAsync<PokeAPIAbility>($"{_baseUrl}/ability/{pokemonId.ToString()}", cancellationToken);


        public async Task<Option<T>> GetPokemonAPI<T>(string endpoint, CancellationToken cancellationToken = default) =>
         await _httpConsumer.GetAsync<T>($"{_baseUrl}/{endpoint}", cancellationToken);
    }
}
