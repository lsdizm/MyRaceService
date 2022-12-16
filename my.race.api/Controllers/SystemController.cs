using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using my.race.connect;

namespace my.race.api.Controllers;

[ApiController]
[Route("system")]
public class SystemController : ControllerBase
{
    private readonly ILogger<QueryController> _logger;

    public SystemController(ILogger<QueryController> logger)
    {
        _logger = logger;
    }

    /// worker start 
    [HttpPut("worker-upgrade")]
    public IActionResult Upgrade()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = "/home/opc/MyRaceService/myraceworker.h",
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
                return Ok(resultExit);

            }
            return Ok(result);
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPut("test")]
    public IActionResult Test()
    {
       return Ok("2022-12-15-버전체크 05");
    }

     [HttpGet("telegram")]
    public async Task<IActionResult> SendTelegram([FromQuery]string message)
    {
        var token = "5974273292:AAH_dQslxH-pj78N-PffAwkYoVmbnPtj3bM";
        var chatId = "415767607";
        var url = string.Format(@"https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", token, chatId, message);
        var httpClient = new HttpClient();
 
        try
        {

            var response = await httpClient.GetAsync(url).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return Ok(result);
        }
        catch (Exception ex) 
        { 
            return BadRequest(ex.ToString());
        }
    }
}
