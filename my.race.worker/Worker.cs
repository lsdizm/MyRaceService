using my.race.connect;

namespace my.race.worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IDataService _service;

    private DateTime _lastUpdateDateTime;

    public Worker(ILogger<Worker> logger,
        IDataService service)
    {
        _logger = logger;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var today = DateTime.Now.Date;
            if (_lastUpdateDateTime == null || _lastUpdateDateTime.Date < today)
            {
                var result = await _service.GetRaceResultDetail(today.AddDays(-14), today, false).ConfigureAwait(false);
                _lastUpdateDateTime = today;
                // call
            }

            // 1시간단위
            await Task.Delay((1000*60*60), stoppingToken);
        }
    }
}
