using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using my.race.connect;

namespace my.race.api.Controllers;
[ApiController]
[Route("migrate")]
public class MigrateController : ControllerBase
{
    private readonly IDataService _service;
    private readonly IDataBases _databases;
    private readonly IDataAPI _dataapi;
    private readonly IConfigurations _configurations;
    private readonly ILogger<QueryController> _logger;
    public MigrateController(IDataBases databases,
        IDataAPI dataapi,
        IConfigurations configurations,
        IDataService service,
        ILogger<QueryController> logger)
    {
        _configurations = configurations;
        _service = service;
        _databases = databases;
        _dataapi = dataapi;
        _logger = logger;
    }    

    [HttpGet("api-information")]
    public async Task<IActionResult> GetApiInformation([FromQuery]string id)
    {
        var result = await _databases.SelectApiInformation(id).ConfigureAwait(false);

        if (result != null)
        {

        }
        return Ok(result);
    }

    [HttpGet("data-api-test")]
    public async Task<IActionResult> TestApi([FromQuery]string id, [FromQuery] List<string> parameters)
    {
        // var result = await GetFromAPI<RaceResult>("B551015/API186/SeoulRace", parameters).ConfigureAwait(false);
        // var result = await GetFromAPI<HorseApi>("B551015/racehorselist/getracehorselist", parameters).ConfigureAwait(false);
            
        var result = await _databases.SelectApiInformation(id).ConfigureAwait(false);
        
        if (result != null)
        {
            var serviceKey = _configurations.GetMyRaceEnvironment("API-SERVICE-KEY");        
            var _parameters = result.MakeParameters(parameters);
            var apiResult = await  _dataapi.GetFromAPI<dynamic>(result.UrlAddress, _parameters).ConfigureAwait(false);
            return Ok(apiResult);            
        }
        else 
        {
            return BadRequest();
        }
    }

    [HttpGet("configurations")]
    public IActionResult GetConfiguration([FromQuery]string itemCode)
    {
        var result = _configurations.GetMyRaceEnvironment(itemCode);
        return Ok(result);
    }

    [HttpPost("execute/race-results")]
    public async Task<IActionResult> MigrateRaceResult([FromQuery]DateTime? fromDate, [FromQuery]DateTime? toDate)
    {
        if (fromDate.HasValue == false) 
        {
            fromDate = DateTime.Now.Date;
        }

        if (toDate.HasValue == false)
        {
            toDate = fromDate;
        }

        var result = await _service.GetRaceResultDetail(fromDate.Value, toDate.Value, true).ConfigureAwait(false);
        
        return Ok(result);
    }

    [HttpGet("migrate/horse")]
    public IActionResult GetHorse([FromQuery]string keyword)
    {
        //var result = await _dataapi.GetHorceResult(meet, rank).ConfigureAwait(false);
        return Ok(true);
    }
}
