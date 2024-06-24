using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employee_Portal.Database;
using Employee_Portal.Model;
using Employee_Portal.Core;
using Microsoft.AspNetCore.Cors;

namespace Employee_Portal.Controllers
{
    [EnableCors("MyCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeMastersController : ControllerBase
    {
        private readonly IEmployee _context;
        private IFileService _fileService;
        private readonly ILogger<EmployeeMastersController> _logger;

        public EmployeeMastersController(ILogger<EmployeeMastersController> logger,IEmployee context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
            _logger=logger;
        }

        // GET: api/EmployeeMasters
        [HttpGet("GetAllEmployees")]
        
        public async Task<ActionResult<IEnumerable<EmployeeMaster>>> GetEmployees()
        {
            throw new Exception("Elastic search Exception !!!");
            var employeeslist= await _context.GettingAllEmployees();
            return Ok(employeeslist);
        }
        [HttpGet]
        [Route("Countries")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            var countrylist = await _context.Countries();
            return Ok(countrylist);
        }

        [HttpGet]
        [Route("ById/{id}")]
        public async Task<ActionResult<EmployeeMaster>> GetById([FromRoute] int id)
        {
            var employee = await _context.GetById(id);

            if (employee == null)
            {
                return NotFound(); // Return a 404 Not Found response if the employee with the specified ID is not found.
            }

            return employee;
        }



        [HttpGet]
        [Route("States")]
        public async Task<ActionResult<IEnumerable<State>>> GetStates(int id)
        {
            var stateslist = await _context.States(id);
            //var citylist = await _context.Cities(id);
            return Ok(stateslist);
        }
        [HttpGet]
        [Route("Cities")]
        public async Task<ActionResult<IEnumerable<City>>> GetCities(int id)
        {
            var citylist = await _context.Cities(id);
            return Ok(citylist);
        }
        [HttpGet]
        [Route("Fetch")]
        public async Task<ActionResult<IEnumerable<City>>> EmployeesFetch()
        {
            var list = await _context.FetchEmployees();
            return Ok(list);
        }
        // PUT: api/EmployeeMasters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
         public async Task<IActionResult> PutEmployeeMaster([FromForm] EmployeeMaster employeeMaster)
         {
            UpdatedEmployeResponse response=new UpdatedEmployeResponse();
            if (employeeMaster.Imagefile != null)
            {
                var formFile = employeeMaster.Imagefile;

                // Check the file size against your desired limit (e.g., 10 megabytes)
                if (formFile.Length > 200 * 1024) // 200KB
                {
                    response.IsUpdated = false;
                    response.Message = "File size exceeds the 200KB limit.";
                    return BadRequest(response);
                }

                var fileResult = _fileService.SaveImage(employeeMaster.Imagefile);
                if (fileResult.Item1 == 1)
                {
                    employeeMaster.ProfileImage = fileResult.Item2;//getting file image name
                }
                else
                {
                    response.IsUpdated = false;
                    response.Message = "png,jpg,jpeg Format allowed";
                    return BadRequest(response);
                }
            }
             response = await _context.Update(employeeMaster);
            if (response.IsUpdated)
            {
                return Ok(response);
            }
            return BadRequest(response); 
         }

        // POST: api/EmployeeMasters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<AddingEmployResponse>> PostEmployeeMaster([FromForm] EmployeeMaster employeeMaster)
        {
            AddingEmployResponse response = new AddingEmployResponse();

            if (employeeMaster.Imagefile != null)
            {
                var formFile = employeeMaster.Imagefile;

                // Check the file size against your desired limit (e.g., 10 megabytes)
                if (formFile.Length > 200 * 1024) // 200KB
                {
                    response.IsAdded = false;
                    response.Message = "File size exceeds the 200KB limit.";
                    return BadRequest(response);
                }

                // Check the file extension or format here if needed (e.g., restrict to png, jpg, jpeg)
                // You can use formFile.FileName to get the file name and validate its extension.

                var fileResult = _fileService.SaveImage(formFile);
                if (fileResult.Item1 == 1)
                {
                    employeeMaster.ProfileImage = fileResult.Item2; //getting file image name
                }
                else
                {
                    response.IsAdded = false;
                    response.Message = "png, jpg, jpeg Format allowed";
                    return BadRequest(response);
                }
            }

            response = await _context.AddingEmployee(employeeMaster);

            if (response.IsAdded)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        // DELETE: api/EmployeeMasters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeMaster(int id)
        {
           DeletingEmployResponse response=await _context.DeleteEmploy(id);
            if (response.IsDeleted)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
