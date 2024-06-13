using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using w1.Models;
using w1.Models.dto;

namespace w1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosoUniversityContext _context;

        public CoursesController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var data = await _context.Courses.ToListAsync();
            return data;
        }

        // GET: api/Courses
        [HttpGet("page/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetCoursesByPage(int pageIndex = 1, int pageSize = 2)
        {
            // 1.一定要先排序
            var data = _context.Courses.OrderBy(c => c.CourseId).AsQueryable();

            // 2. 計算總筆數
            var total = await data.CountAsync();
            // 3. 計算總頁數
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            // 4. 計算要取得的資料
            var items = await data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            // 5. 回傳
            return Ok(new GetCoursesByPageViewModel
            {
                Total = total,
                TotalPages = totalPages,
                Data = items.Select(c => new CourseVeiwModel()
                            {
                                CourseId = c.CourseId,
                                Title = c.Title,
                                Credits = c.Credits
                            })
                            .ToList()
            });
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseUpdate course)
        {
            // if (id != course.CourseId)
            // {
            //     return BadRequest();
            // }

            // 這段意思是
            // 把 course 透過 entry ，attach 到 context ，並且改變狀態變成 modified
            // ef 只要狀態被修改，就會自動把資料產出 update set where 的語法更改狀態
            // 這代表資料表內的每一筆資料、每一筆欄位都會被更新
            // _context.Entry(course).State = EntityState.Modified;
            var entity = await _context.Courses.FindAsync(id);
            entity!.Title = course.Title;
            entity.Credits = course.Credits;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(CourseCreate course)
        {
            var dto = new Course()
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Credits = course.Credits,
                DepartmentId = 1
            };
            _context.Courses.Add(dto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}