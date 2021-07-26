using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class WatchController : BaseApiController
    {
        //private DateTime startTime;
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public WatchController(DataContext context
                            , IMapper mapper
                            , ITokenService tokenService)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _context = context;
        }

        [HttpGet]
        // api/watch
        public ActionResult<DateTime> GetCurrentTime()
        {
            return DateTime.Now;
        }

        //[Route("/active-items-count/")]
        [HttpPost("start")]
        public async Task<ActionResult<RunDto>> StartStopwatch()
        {
            var run = new Run
            {
                Status = "running"
            };

            var added = _context.Runs.Add(run);

            var runToken = _tokenService.CreateToken(added.Entity);
            run.RunToken = runToken;
            await _context.SaveChangesAsync();

            return new RunDto
            {
                RunToken = runToken
            };
        }

        [HttpPut("lap/{runToken}")]
        public async Task<ActionResult<Run>> LapStopwatch(string runToken)
        {
            Run run = _context.Runs
                    .Include(run => run.LapTimes)
                    .FirstOrDefault(run => run.RunToken.Equals(runToken));

            Lap lap = new Lap{
                RunId = run.Id
            };
            run.LapTimes.Add(lap);
            run.Status = "laps";

            if(await _context.SaveChangesAsync() > 0) return Ok();

            return BadRequest("Failed to follow the user");
        }

        [HttpPut("stop/{runToken}")]
        public async Task<ActionResult<ResultsDto>> StopStopwatch(string runToken)
        {
            Run run = _context.Runs
                    //.Include(run => run.LapTimes)
                    .FirstOrDefault(run => run.RunToken.Equals(runToken));
            
            run.EndTime = DateTime.Now;
            run.Status = "done";

            _context.Update(run);
            await _context.SaveChangesAsync();
            
            run.LapTimes = await _context.Laps
                                            .Where(lap => lap.RunId == run.Id)
                                            .ToListAsync();

            var runToReturn = _mapper.Map<RunDoneDto>(run);
            
            var lapsArr = runToReturn.LapTimes.ToArray();
            IEnumerable<string> laps = new List<string>();
            laps = laps.Append((lapsArr[0].LapTime - runToReturn.StartTime).ToString());
            for (int i = 1; i < lapsArr.Length; i++)
            {
                laps = laps.Append((lapsArr[i].LapTime - lapsArr[i-1].LapTime).ToString());
                //Console.WriteLine(laps.ElementAt(i));
            }
            ResultsDto results = new ResultsDto{
                Laps = laps,
                TotalTime = (runToReturn.EndTime-runToReturn.StartTime).ToString()
            };
            
            return Ok(results);
        }
    }
}