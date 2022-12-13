using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using my.race.connect;

namespace my.race.api.Controllers;

[ApiController]
public class QueryController : ControllerBase
{
    private readonly IDataBases _databases;
    private readonly IDataAPI _dataapi;
    private readonly ILogger<QueryController> _logger;
    public QueryController(IDataBases databases,
        IDataAPI dataapi,
        ILogger<QueryController> logger)
    {
        _databases = databases;
        _dataapi = dataapi;
        _logger = logger;
    }

    [HttpGet("queries")]    
    public async Task<IActionResult> GetQuery()
    {
        var result = await _databases.SelectAllSqls().ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("queries/{id}")]
    public async Task<IActionResult> GetQuery(string id, [FromBody]object jsonParameter )
    {
        var result = await _databases.SelectAsync<dynamic>(id, jsonParameter).ConfigureAwait(false);
        return Ok(result);
    }

    // [HttpGet("apis/horse-result")]
    // public async Task<IActionResult> GetHorseResult([FromQuery]string meet, [FromQuery] string rank)
    // {
    //     var result = await _dataapi.GetHorceResult(meet, rank).ConfigureAwait(false);
    //     return Ok(result);
    // }

    // [HttpPut("migrate/update-horse")]
    // public async Task<IActionResult> UpdateHorse()
    // {
    //     var meet = "1";
    //     var rankList = new List<string>(){"외1","외2","외3","외4","국1","국2","국3","국4","국5","국6","국미검","외미검"};

    //     foreach (var rank in rankList){
    //         var result = await _dataapi.GetHorceResult(meet, rank).ConfigureAwait(false);

    //         if (result.Any()) 
    //         {
    //             using (var connection = _databases.Connect()) 
    //             {
    //                 await connection.OpenAsync().ConfigureAwait(false);

    //                 foreach (var item in result) 
    //                 {
    //                     var sql = $"insert into HORSE_RESULT (age,chulYn,hrName,hrNo,jun,meet,"+
    //                                                         "minRcDate,nameSp,prdCty,prizeYear,prizetsum," +
    //                                                         "rank,rating1,rating2,rating3,rating4," +
    //                                                         "sex,spTerm,stDate) values " + 
    //                                 $"('{item.age}', '{item.chulYn}', '{item.hrName}', '{item.hrNo}', '{item.jun}', '{item.meet}', " +
    //                                 $"'{item.minRcDate}', '{item.nameSp}', '{item.prdCty}', '{item.prizeYear}', '{item.prizetsum}', "+ 
    //                                 $"'{item.rank}', '{item.rating1}', '{item.rating2}', '{item.rating3}', '{item.rating4}'," +
    //                                 $"'{item.sex}', '{item.spTerm}', '{item.stDate}');";
    //                     await _databases.ExecuteAsync(sql, connection).ConfigureAwait(false);
    //                 }
    //             }   
    //         }
    //     }
        
    //     return Ok(true);
    // }
}
