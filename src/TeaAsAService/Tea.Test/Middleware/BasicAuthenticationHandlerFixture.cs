using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Tea.Core.Data;
using Tea.Core.Domain;
using Tea.Web.Helpers;
using Xunit;

namespace Tea.Test.Middleware
{
    public class BasicAuthenticationHandlerFixture
    {
        private readonly Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _options;
        private readonly Mock<IDataStore> _dataStore;
        private readonly BasicAuthenticationHandler _handler;

        public BasicAuthenticationHandlerFixture()
        {
            _options = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
            _options.Setup(x => x.Get(BasicAuthenticationHandler.SchemeName)).Returns(new AuthenticationSchemeOptions());

            var logger = new Mock<ILogger<BasicAuthenticationHandler>>();
            var loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(logger.Object);

            var encoder = new Mock<UrlEncoder>();
            var clock = new Mock<ISystemClock>();
            _dataStore = new Mock<IDataStore>();

            _handler = new BasicAuthenticationHandler(_options.Object, loggerFactory.Object, encoder.Object, clock.Object, _dataStore.Object);
        }

        [Fact]
        public async Task HandleAuthenticateAsync_NoAuthorizationHeader_ReturnsAuthenticateResultFail()
        {
            var context = new DefaultHttpContext();

            await _handler
                .InitializeAsync(new AuthenticationScheme(BasicAuthenticationHandler.SchemeName, null, typeof(BasicAuthenticationHandler)), context);
            var result = await _handler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.Equal("Missing Authorization Header", result.Failure.Message);
        }

        [Fact]
        public async Task HandleAuthenticateAsync_CredentialsTryParseFails_ReturnsAuthenticateResultFail()
        {
            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues(string.Empty);
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

            await _handler
                .InitializeAsync(new AuthenticationScheme(BasicAuthenticationHandler.SchemeName, null, typeof(BasicAuthenticationHandler)), context);

            var result = await _handler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.Equal("Basic authentication failed. Unable to parse username and password", result.Failure.Message);
        }

        [Fact]
        public async Task HandleAuthenticateAsync_InvalidCredentials_ReturnsAuthenticateResultFail()
        {
            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("Basic VGVzdFVzZXJOYW1lOlRlc3RQYXNzd29yZA==");
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

            await _handler
                .InitializeAsync(new AuthenticationScheme(BasicAuthenticationHandler.SchemeName, null, typeof(BasicAuthenticationHandler)), context);
            
            var result = await _handler.AuthenticateAsync();

            Assert.False(result.Succeeded);
            Assert.Equal("Invalid Username or Password", result.Failure.Message);
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ValidCredentials_ReturnsAuthenticateResultSuccess()
        {
            var user = new User { 
                Id = Guid.NewGuid(), 
                SimpleId = "TestUserName" ,
                EmailAddress = "user@domain.com"
            };

            var context = new DefaultHttpContext();
            var authorizationHeader = new StringValues("Basic VGVzdFVzZXJOYW1lOlRlc3RQYXNzd29yZA==");
            context.Request.Headers.Add(HeaderNames.Authorization, authorizationHeader);

            _dataStore
                .Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            await _handler
                .InitializeAsync(new AuthenticationScheme(BasicAuthenticationHandler.SchemeName, null, typeof(BasicAuthenticationHandler)), context);

            var result = await _handler.AuthenticateAsync();

            Assert.True(result.Succeeded);
            Assert.Equal(BasicAuthenticationHandler.SchemeName, result.Ticket.AuthenticationScheme);
            Assert.True(result.Ticket.Principal.Identity.IsAuthenticated);
            Assert.Equal(2, result.Ticket.Principal.Claims.Count());
        }
    }
}
