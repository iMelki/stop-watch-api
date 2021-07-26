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
        public async Task<ActionResult<RunDoneDto>> StopStopwatch(string runToken)
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

            

            return Ok(runToReturn);
            /*return new RunDoneDto{

            };*/
        }
    }
}