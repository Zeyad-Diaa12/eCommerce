using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>
    (ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle Request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();

        timer.Start();

        var response = await next();

        timer.Stop();

        var timeTaken = timer.Elapsed;
        if(timeTaken.Seconds > 6)
            logger.LogWarning("[PERFORMANCE] Handle Request={Request} - TimeTaken={TimeTaken}",
                typeof(TRequest).Name, timeTaken.Seconds);

        logger.LogInformation("[END] Handled Request={Request} - Response={Response} - TimeTaken={TimeTaken}",
            typeof(TRequest).Name, typeof(TResponse).Name, timeTaken.Seconds);
        return response;
    }
}
