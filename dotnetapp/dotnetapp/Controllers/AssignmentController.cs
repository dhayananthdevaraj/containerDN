using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    [Route("api/assignment")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAllAssignments()
        {
            try
            {
                var assignments = await _context.Assignments
                .Include(a => a.Container)
                .Include(a => a.User).ToListAsync();
                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("{assignmentId}")]
        public async Task<ActionResult<Assignment>> GetAssignmentById(long assignmentId)
        {
            try
            {
                var assignment = await _context.Assignments
                    .Include(a => a.Container)
                    .Include(a => a.User)
                    .Include(a => a.Issues)
                    .FirstOrDefaultAsync(a => a.AssignmentId == assignmentId);

                if (assignment == null)
                {
                    return NotFound(new { message = "Cannot find the assignment" });
                }

                return Ok(assignment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignmentsByUserId(long userId)
        {
            try
            {
                var assignments = await _context.Assignments
                    .Where(a => a.UserId == userId)
                    .Include(a => a.Container)
                    .ToListAsync();

                return Ok(assignments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddAssignment([FromBody] Assignment newAssignment)
        {
            try
            {
                _context.Assignments.Add(newAssignment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAssignmentById), new { assignmentId = newAssignment.AssignmentId }, newAssignment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{assignmentId}")]
        public async Task<ActionResult> UpdateAssignment(long assignmentId, [FromBody] Assignment updatedAssignment)
        {
            try
            {
                var existingAssignment = await _context.Assignments.FindAsync(assignmentId);

                if (existingAssignment == null)
                {
                    return NotFound(new { message = "Cannot find the assignment" });
                }

                existingAssignment.Status = updatedAssignment.Status;
                existingAssignment.UpdateTime = updatedAssignment.UpdateTime;
                existingAssignment.Route = updatedAssignment.Route;
                existingAssignment.Shipment = updatedAssignment.Shipment;
                existingAssignment.Destination = updatedAssignment.Destination;

                await _context.SaveChangesAsync();

                return Ok(existingAssignment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{assignmentId}")]
        public async Task<ActionResult> DeleteAssignment(long assignmentId)
        {
            try
            {
                var assignment = await _context.Assignments.FindAsync(assignmentId);

                if (assignment == null)
                {
                    return NotFound(new { message = "Cannot find the assignment" });
                }

                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Assignment deleted successfully", assignment });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
