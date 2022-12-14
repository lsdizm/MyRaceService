using System.Diagnostics;
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
        // worker 로드시에 api 서비스 리로드
        this.UpdateApiService();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var today = DateTime.Now.Date;
            if (_lastUpdateDateTime.Date < today)
            {
                var result = await _service.GetRaceResultDetail(today.AddDays(-14), today, false).ConfigureAwait(false);
                if (result != null && result.Any())
                {
                    _logger.LogInformation("Update: ", result.Count());
                }
                _lastUpdateDateTime = today;
                // call
            }

            // 1시간단위
            await Task.Delay((1000*60*60), stoppingToken);
        }
    }

    private void UpdateApiService()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = "/home/opc/MyRaceService/myraceservice.h",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            };

            Process proc = new Process() { StartInfo = psi, };
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            if (string.IsNullOrWhiteSpace(result))
            {
                var resultExit = proc.ExitCode.ToString();
                _logger.LogInformation(resultExit);                

            }
            _logger.LogInformation(result);
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex.ToString());
        }
    }
}
