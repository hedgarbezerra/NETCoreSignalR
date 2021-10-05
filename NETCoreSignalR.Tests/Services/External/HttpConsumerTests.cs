using AutoFixture;
using FluentAssertions;
using LanguageExt.UnitTesting;
using Moq;
using NETCoreSignalR.Services.External;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETCoreSignalR.Tests.Services.External
{

    [TestFixture]
    public class HttpConsumerTests
    {
        private MockRepository _mockRepository;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);
            _fixture = new Fixture();
        }
        [ExcludeFromCodeCoverage]
        private HttpConsumer CreateHttpConsumer(IRestClient client)
        {
            return new HttpConsumer(client);
        }
        [ExcludeFromCodeCoverage]
        private static IRestClient MockRestClient<T>(HttpStatusCode httpStatusCode, object result, bool isAsync) where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(_ => _.StatusCode).Returns(httpStatusCode);
            response.Setup(_ => _.Data).Returns((T)result);
            response.SetupGet(x => x.IsSuccessful).Returns(httpStatusCode == HttpStatusCode.OK);

            var mockIRestClient = new Mock<IRestClient>();

            if (isAsync)
                mockIRestClient
                 .Setup(x => x.ExecuteAsync<T>(It.IsAny<IRestRequest>(), It.IsAny<Method>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.FromResult(response.Object));
            else
                mockIRestClient
                  .Setup(x => x.Execute<T>(It.IsAny<IRestRequest>(), It.IsAny<Method>()))
                  .Returns(response.Object);

            return mockIRestClient.Object;
        }

        [Test]
        public void Get_StatusCodeOk_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);


            // Act
            var result = httpConsumer.Get<ExampleHttpConsumerImp>(url);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Get_NotSucessfulStatusCode_ReturnsExpectedNoneOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.Forbidden, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Get<ExampleHttpConsumerImp>(url);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Post_ObjectAsBodyStatusCodeOk_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Post<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Post_KeypairValueBodyNotSucessfulStatusCode_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.CreateMany<List<KeyValuePair<string, object>>>(2);

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.InternalServerError, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Post<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Post_NotSucessfulStatusCode_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.InternalServerError, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Post<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Put_ObjectAsBodyStatusCodeOk_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Put<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Put_KeypairValueBodyNotSucessfulStatusCode_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.CreateMany<List<KeyValuePair<string, object>>>(2);

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Put<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Put_NotSucessfulStatusCode_ReturnsExpectedObject()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.InternalServerError, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Put<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Delete_NotSucessfulStatusCode_ReturnsNoneOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.InternalServerError, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Delete<ExampleHttpConsumerImp>(url);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public void Delete_SucessfulStatusCode_ReturnsSomeOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, false);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = httpConsumer.Delete<ExampleHttpConsumerImp>(url);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetAsync_CorrectRequest_ReturnsSomeOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.GetAsync<ExampleHttpConsumerImp>(url, cancellationTokenSource.Token);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetAsync_CancelledTask_ThrowsOperationCancelledException()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();
            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            cancellationTokenSource.Cancel();

            // Assert            
            Assert.ThrowsAsync<OperationCanceledException>(() => httpConsumer.GetAsync<ExampleHttpConsumerImp>(url, cancellationTokenSource.Token));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetAsync_InvalidResponse_ReturnsNoneOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.InternalServerError, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.GetAsync<ExampleHttpConsumerImp>(url, cancellationTokenSource.Token);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task PostAsync_ValidSerializedResponse_ReturnsSomeOption()
        {// Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.PostAsync<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }


        [Test]
        public async Task PostAsync_InvalidResponse_ReturnsNoneOption()
        {// Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.NotFound, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.PostAsync<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }


        [Test]
        public async Task PostAsync_CancelledTask_ThrowsOperationCancelledException()
        {// Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            cancellationTokenSource.Cancel();

            // Assert            
            Assert.ThrowsAsync<OperationCanceledException>(() => httpConsumer.PostAsync<ExampleHttpConsumerImp>(url, param, cancellationTokenSource.Token));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task PutAsync_ValidSerializedResponse_ReturnsSomeOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.PutAsync<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }


        [Test]
        public async Task PutAsync_InvalidResponse_ReturnsNoneOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.NotFound, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.PutAsync<ExampleHttpConsumerImp>(url, param);

            // Assert
            result.ShouldBeNone();
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task PutAsync_CancelledTask_ThrowsOperationCancelledException()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var param = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            cancellationTokenSource.Cancel();

            // Assert            
            Assert.ThrowsAsync<OperationCanceledException>(() => httpConsumer.PutAsync<ExampleHttpConsumerImp>(url, param, cancellationTokenSource.Token));
            _mockRepository.VerifyAll();
        }


        [Test]
        public async Task DeleteAsync_CancelledTask_ThrowsOperationCancelledException()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();
            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            cancellationTokenSource.Cancel();

            // Assert            
            Assert.ThrowsAsync<OperationCanceledException>(() => httpConsumer.DeleteAsync<ExampleHttpConsumerImp>(url, cancellationTokenSource.Token));
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task DeleteAsync_SuccessfullRequest_ReturnsSomeOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.DeleteAsync<ExampleHttpConsumerImp>(url, cancellationTokenSource.Token);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }
        [Test]
        public async Task DeleteAsync_SuccessfullFailed_ReturnsNoneOption()
        {
            // Arrange
            string url = _fixture.Create<string>();
            var mqResult = _fixture.Create<ExampleHttpConsumerImp>();
            var cancellationTokenSource = _fixture.Create<CancellationTokenSource>();

            var client = MockRestClient<ExampleHttpConsumerImp>(HttpStatusCode.OK, mqResult, true);
            var httpConsumer = CreateHttpConsumer(client);

            // Act
            var result = await httpConsumer.DeleteAsync<ExampleHttpConsumerImp>(url, cancellationTokenSource.Token);

            // Assert
            result.ShouldBeSome(res => res.Should().NotBeNull().And.Match<ExampleHttpConsumerImp>(x => x.Id != null && !string.IsNullOrEmpty(x.Item)));
            _mockRepository.VerifyAll();
        }       
    }

    public class ExampleHttpConsumerImp
    {
        public Guid Id { get; set; }
        public string Item { get; set; }
    }
}
