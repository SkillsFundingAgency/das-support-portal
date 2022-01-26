using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Shared.Tests
{
    public class StrategyTestBase<T> where T : class
    {
        protected HttpClient _httpClient;
        protected Mock<ILog> MockLogger;
        protected Mock<HttpMessageHandler> MockHttpMessageHandler;
        protected Exception TestException = new Exception("A test exception");
        protected T Unit;

        [SetUp]
        public void Setup()
        {
            MockHttpMessageHandler = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(MockHttpMessageHandler.Object);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "value");
            MockLogger = new Mock<ILog>();
            Unit = Activator.CreateInstance(typeof(T), MockLogger.Object) as T;
        }
    }
}