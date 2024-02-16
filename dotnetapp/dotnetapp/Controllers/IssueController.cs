using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnetapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Controllers
{
    [Route("api/issue")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IssueController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Issue
        [HttpPost]
        public async Task<ActionResult> ReportIssue([FromBody] Issue newIssue)
        {
            try
            {
                // Add the new issue to the context and save changes
                await _context.Issues.AddAsync(newIssue);
                await _context.SaveChangesAsync();

                // Return a 201 Created response with the created issue in the body
                return CreatedAtAction(nameof(ReportIssue), new { id = newIssue.IssueId }, newIssue);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with an error message
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> ViewAllReportedIssues()
        {
            try
            {
                // Retrieve all issues from the context
                // var issues = await _context.Issues
                // .Include(a => a.Assignment)
                // .Include(a => a.User).ToListAsync();

                var issues = await _context.Issues
                 .AsNoTracking()
                .Include(i => i.User)
                .Include(i => i.Assignment)
                    .ThenInclude(a => a.Container)
                // .Include(i => i.Assignment)
                //      .ThenInclude(a => a.User)
                .ToListAsync();


                // Return a 200 OK response with the list of issues in the body
                return Ok(issues);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error response with an error message
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
