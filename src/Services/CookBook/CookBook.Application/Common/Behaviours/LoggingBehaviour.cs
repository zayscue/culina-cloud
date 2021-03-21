using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Culina.CookBook.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Culina CookBook Request: {Name} {@Request}",
                requestName, request);

            return Task.FromResult(0);
        }
    }
}
