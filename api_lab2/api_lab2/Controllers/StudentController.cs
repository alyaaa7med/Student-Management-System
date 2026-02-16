using api_lab2.DTOs;
using api_lab2.Models;
using api_lab2.MapperConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AutoMapper;
using api_lab2.Repository;

namespace api_lab2.Controllers
{
    [Route("api/[controller]s")]

    public class StudentController : Controller
    {

        GenericRepo<Student> srepo;
        IMapper mapper;
        public StudentController(GenericRepo<Student> srepo,  IMapper mapper )
        {
            this.srepo = srepo;
            this.mapper = mapper;
        }
        [HttpGet]
        [Produces("application/json")]
      
        public IActionResult GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 4, [FromQuery] string? search = null)
        {


            var query = srepo.getall( s=> s.department);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    (s.St_FName != null && s.St_FName.Contains(search)) 
                );
            }

            var totalRecords = query.Count();

            var pagedStudents = query
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            var stDTOList = mapper.Map<List<StudentDTO>>(pagedStudents);

            var result = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = stDTOList
            };

            return Ok(result);
        }

      
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        public ActionResult getbyid(int id)
        {
            Student s = srepo.getbyid(id);
            if (s == null) return NotFound();
          
            StudentDTO stDTO = mapper.Map<StudentDTO>(s);
            return Ok(stDTO);
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult post(StudentDTO sdto)
        {
            if (sdto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var s = mapper.Map<Student>(sdto);
           
            srepo.Add(s);
            srepo.Save();
            return CreatedAtAction(nameof(getbyid), new { id = s.Id }, sdto);
        }


        [HttpPut("{id:int}")]
        [Consumes("application/json")]

        public IActionResult Update(int id, StudentDTO stdo)
        {
            if (stdo == null)
                return BadRequest();

            var existingStudent = srepo.getbyid(id);  
            if (existingStudent == null)
                return NotFound();

            mapper.Map(stdo, existingStudent);

            srepo.Update(existingStudent);
            srepo.Save();
            return NoContent();
        }



        [HttpDelete("{id:int}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public IActionResult Delete(int id)
        {
            var student = srepo.getbyid(id);
            if (student == null)
                return NotFound();

            srepo.delete(student);
            srepo.Save();

            var deletedStudentDTO = mapper.Map<StudentDTO>(student);
            return Ok(deletedStudentDTO);
        }



    }
}

