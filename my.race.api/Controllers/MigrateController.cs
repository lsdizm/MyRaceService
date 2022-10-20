using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using my.race.connect;

namespace my.race.api.Controllers;
[ApiController]
[Route("migrate")]
public class MigrateController : ControllerBase
{
    private readonly IDataBases _databases;
    private readonly IDataAPI _dataapi;
    private readonly ILogger<QueryController> _logger;
    public MigrateController(IDataBases databases,
        IDataAPI dataapi,
        ILogger<QueryController> logger)
    {
        _databases = databases;
        _dataapi = dataapi;
        _logger = logger;
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

        var result = await _dataapi.GetRaceResult(fromDate.Value, toDate.Value).ConfigureAwait(false);

         if (result.Any()) 
        {
            using (var connection = _databases.Connect()) 
            {
                await connection.OpenAsync().ConfigureAwait(false);

                // foreach (var item in result) 
                // {
                //     var sql = $"insert into HORSE_RESULT (age,chulYn,hrName,hrNo,jun,meet,"+
                //                                         "minRcDate,nameSp,prdCty,prizeYear,prizetsum," +
                //                                         "rank,rating1,rating2,rating3,rating4," +
                //                                         "sex,spTerm,stDate) values " + 
                //                 $"('{item.age}', '{item.chulYn}', '{item.hrName}', '{item.hrNo}', '{item.jun}', '{item.meet}', " +
                //                 $"'{item.minRcDate}', '{item.nameSp}', '{item.prdCty}', '{item.prizeYear}', '{item.prizetsum}', "+ 
                //                 $"'{item.rank}', '{item.rating1}', '{item.rating2}', '{item.rating3}', '{item.rating4}'," +
                //                 $"'{item.sex}', '{item.spTerm}', '{item.stDate}');";
                //     await _databases.ExecuteAsync(sql, connection).ConfigureAwait(false);
                // }
            }   
        }
        return Ok(result);
    }

        [HttpGet("migrate/horse")]
    public IActionResult GetHorse([FromQuery]string keyword)
    {
        //var result = await _dataapi.GetHorceResult(meet, rank).ConfigureAwait(false);
        return Ok(true);
    }
}
