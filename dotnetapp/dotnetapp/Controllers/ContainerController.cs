using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetapp.Models;

namespace dotnetapp.Controllers
{
    [Route("api/container")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Container>>> GetAllContainers()
        {
            try
            {
                var containers = await _context.Containers.ToListAsync();
                return Ok(containers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddContainer([FromBody] Container container)
        {
            try
            {
                _context.Containers.Add(container);
                await _context.SaveChangesAsync();

                return StatusCode(201, container);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{containerId}")]
        public async Task<ActionResult> UpdateContainer(long containerId, [FromBody] Container container)
        {
            try
            {
                var existingContainer = await _context.Containers.FirstOrDefaultAsync(c => c.ContainerId == containerId);

                if (existingContainer == null)
                {
                    return NotFound(new { message = "Cannot find the container" });
                }

                existingContainer.Type = container.Type;
                existingContainer.Status = container.Status;
                existingContainer.Capacity = container.Capacity;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Container updated successfully"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("{containerId}")]
        public async Task<ActionResult> DeleteContainer(long containerId)
        {
            try
            {
                var container = await _context.Containers.FirstOrDefaultAsync(c => c.ContainerId == containerId);

                if (container == null)
                {
                    return NotFound(new { message = "Cannot find the container" });
                }

                _context.Containers.Remove(container);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Container deleted successfully"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
//https://github.com/dhayananthdevaraj/containerDN.git