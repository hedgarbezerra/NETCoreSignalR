using Moq;
using NETCoreSignalR.Services.External;
using NETCoreSignalR.Services.External.PokeAPI;
using NUnit.Framework;
using LanguageExt.UnsafeValueAccess;
using System;
using System.Threading;
using System.Threading.Tasks;
using NETCoreSignalR.Domain.Model.PokeAPI;
using AutoFixture;
using System.Linq;
using FluentAssertions;
using LanguageExt;
using LanguageExt.UnitTesting;
using NETCoreSignalR.Domain.Interfaces;

namespace NETCoreSignalR.Tests.Services.External
{
    [TestFixture]
    public class PokeAPIConsumerTests
    {
        private MockRepository _mockRepository;
        private Mock<IHttpConsumer> _mockHttpConsumer;
        private PokeAPIConsumer _pokeConsumer;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _mockHttpConsumer = _mockRepository.Create<IHttpConsumer>();
            _pokeConsumer = new PokeAPIConsumer(_mockHttpConsumer.Object);
        }
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public async Task GetPaginatedList_RespondedSuccessfully_ReturnsOptionAsSome()
        {
            // Arrange
            string query = _fixture.Create<string>();
            CancellationToken token = _fixture.Create<CancellationToken>();

            var pokeResultList = _fixture.CreateMany<PokeResult>(5).ToList();
            var pokeListMock = _fixture.Build<PokeAPIList>()
                .With(x => x.results, pokeResultList)
                .With(x => x.count,pokeResultList.Count).Create();

            _mockHttpConsumer.Setup(x => x.GetAsync<PokeAPIList>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(Option<PokeAPIList>.Some(pokeListMock)));

            // Act
            var resultOption = await _pokeConsumer.GetPaginatedList(query, It.IsAny<CancellationToken>());

            // Assert
            resultOption.ShouldBeSome(res => res.Should().NotBeNull().And.Match<PokeAPIList>(op => op.results.Count == 5 && op.count == 5));           
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetPaginatedList_NoReturnFromAPI_ReturnsNone()
        {
            // Arrange
            string query = _fixture.Create<string>();
            var token = _fixture.Create<CancellationToken>();

            // Act
            var resultOption = await _pokeConsumer.GetPaginatedList(query, token);
            var result = resultOption.ValueUnsafe();


            // Assert
            resultOption.ShouldBeNone();
            result.Should().BeNull();
        }

        [Test]
        public async Task GetPokemon_RespondedSuccessfully_ReturnsOptionAsSome()
        {
            // Arrange
            string name = "ditto";
            var mockPokemon = _fixture.Build<PokeAPIPokemon>()
                .With(x => x.name, name)
                .Create();
            _mockHttpConsumer.Setup(x => x.GetAsync<PokeAPIPokemon>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .Returns(Task.FromResult(Option<PokeAPIPokemon>.Some(mockPokemon)));
            // Act
            var resultOption = await _pokeConsumer.GetPokemon(name, It.IsAny<CancellationToken>());

            // Assert
            resultOption.ShouldBeSome(res => res.Should().NotBeNull().And.Match<PokeAPIPokemon>(poke => poke.name == name));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetPokemon_NoValidResponse_ReturnsOptionAsNone()
        {
            // Arrange
            string query = _fixture.Create<string>();
            var token = _fixture.Create<CancellationToken>();

            // Act
            var resultOption = await _pokeConsumer.GetPokemon(query, token);
            var result = resultOption.ValueUnsafe();


            // Assert
            resultOption.ShouldBeNone();
            result.Should().BeNull();
        }
        [Test]
        public async Task GetPokemon_RequestById_ReturnsPokemonOptionSome()
        {
            // Arrange
            int id = _fixture.Create<int>();
            var mockPokemon = _fixture.Build<PokeAPIPokemon>()
                .With(x => x.id, id)
                .Create();

            _mockHttpConsumer.Setup(x => x.GetAsync<PokeAPIPokemon>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .Returns(Task.FromResult(Option<PokeAPIPokemon>.Some(mockPokemon)));
            // Act
            var resultOption = await _pokeConsumer.GetPokemon(id, It.IsAny<CancellationToken>());

            // Assert
            resultOption.ShouldBeSome(res => res.Should().NotBeNull().And.Match<PokeAPIPokemon>(poke => poke.id == id));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetPokemon_RequestByIdNoValidResponse_ReturnsOptionAsNone()
        {
            // Arrange
            string query = _fixture.Create<string>();
            var token = _fixture.Create<CancellationToken>();

            // Act
            var resultOption = await _pokeConsumer.GetPokemon(query, token);
            var result = resultOption.ValueUnsafe();


            // Assert
            resultOption.ShouldBeNone();
            result.Should().BeNull();
        }
        [Test]
        public async Task GetPokemonAbility_RespondedSuccessfully_ReturnsOptionAsSome()
        {
            // Arrange
            int pokemonId = _fixture.Create<int>();
            var mockPokemon = _fixture.Build<PokeAPIAbility>()
               .With(x => x.id, pokemonId)
               .Create();

            _mockHttpConsumer.Setup(x => x.GetAsync<PokeAPIAbility>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
              .Returns(Task.FromResult(Option<PokeAPIAbility>.Some(mockPokemon)));
            // Act
            var result = await _pokeConsumer.GetPokemonAbility(pokemonId, It.IsAny<CancellationToken>());

            // Assert
            result.ShouldBeSome(ability => ability.Should().NotBeNull().And.Match<PokeAPIAbility>(prop => prop.id == pokemonId));
            
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetPokemonAbility_NoValidResponse_ReturnsOptionAsNone()
        {
            // Arrange
            int pokemonId = _fixture.Create<int>();
            var token = _fixture.Create<CancellationToken>();

            // Act
            var resultOption = await _pokeConsumer.GetPokemonAbility(pokemonId, token);
            var result = resultOption.ValueUnsafe();


            // Assert
            resultOption.ShouldBeNone();
            result.Should().BeNull();
        }
        [Test]
        public async Task GetPokemonAPI_RespondedSuccessfully_ReturnsOptionAsSome()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var pokeResultMock = _fixture.Build<PokeResult>()
                .Create();

            _mockHttpConsumer.Setup(x => x.GetAsync<PokeResult>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Option<PokeResult>.Some(pokeResultMock)));

            // Act
            var result = await _pokeConsumer.GetPokemonAPI<PokeResult>(url, It.IsAny<CancellationToken>());

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.BeOfType<PokeResult>());
         
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetPokemonAPI_NoValidResponse_ReturnsOptionAsNone()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var token = _fixture.Create<CancellationToken>();

            // Act
            var resultOption = await _pokeConsumer.GetPokemonAPI<PokeResult>(url, token);
            var result = resultOption.ValueUnsafe();


            // Assert
            resultOption.ShouldBeNone();
            result.Should().BeNull();
        }
       
    }
}
