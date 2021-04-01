using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<MyDoc>> GetProject()
        {
            var projects = ProjectsManager.Instance.Projects;
            return projects;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MyDoc project = ProjectsManager.Instance.Projects.FirstOrDefault(x => x.Id == id);
            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPut("{id}")]
        [SwaggerRequestExample(typeof(MyDoc.Create), typeof(MyDoc.Create.Example))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] MyDoc.Create project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var projects = ProjectsManager.Instance;

            if (projects.Projects.FirstOrDefault(x => x.Id == id) == null)
                return BadRequest();

            
            
            try
            {
                if (projects.Projects[id].Id == id)
                {
                    projects.Projects[id].Name = project.Name;
                    projects.Projects[id].Lines = project.Lines;
                    projects.Projects[id].Polylines = project.Polylines;
                    projects.Projects[id].Circles = project.Circles;
                    projects.Projects[id].LastModificationTime = DateTime.Now;
                }
                    
                else
                    return NotFound();

            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }

            return NoContent();
        }

        
        [HttpPost]
        [SwaggerRequestExample(typeof(MyDoc.Create), typeof(MyDoc.Create.Example))]
        public IActionResult PostProject([FromBody] MyDoc.Create project)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            DateTime now = DateTime.Now;

            var projects = ProjectsManager.Instance;
            var newProject = new MyDoc();
            newProject.Id = projects.Projects.Count();
            newProject.Name = project.Name;
            newProject.Lines = project.Lines;
            newProject.Polylines = project.Polylines;
            newProject.Circles = project.Circles;
            newProject.CreationTime = now;
            newProject.LastModificationTime = now;
            projects.Projects.Add(newProject);

            return CreatedAtAction("GetProject", new { id = newProject.Id }, newProject);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            MyDoc toDelete = ProjectsManager.Instance.Projects.FirstOrDefault(x => x.Id == id);

            if (toDelete == null)
                NotFound();

            int index = ProjectsManager.Instance.Projects.IndexOf(toDelete);
            ProjectsManager.Instance.Projects.RemoveAt(index);

            return Accepted();
        }
    }
}
