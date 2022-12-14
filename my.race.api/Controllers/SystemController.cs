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

    [HttpPut("upgrade")]
    public IActionResult Upgrade()
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "/bin/sh",
                Arguments = "/home/opc/MyRaceService/myrace.h",
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
       return Ok("2022-12-14-버전체크 03");
    }
}
