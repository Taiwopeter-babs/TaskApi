using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

namespace TaskApi.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
public class StudentItemsController : ControllerBase
{
    private readonly TaskContext _context;

    public StudentItemsController(TaskContext context)
    {
        _context = context;
    }

    /// <summary>
    ///  Returns list of student items
    /// </summary>
    /// <returns>A list of items of StudentItemDTO</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentItemDTO>>> GetStudentItems()
    {
        return await _context.StudentItems
        .Select(s => ItemToDTO(s))
        .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentItemDTO>> GetStudentItem(long id)
    {
        var studentItem = await _context.StudentItems.FindAsync(id);
        if (studentItem == null)
        {
            return NotFound();
        }
        return ItemToDTO(studentItem);
    }

    /// <summary>
    /// PUT Request handler for StudentItem
    /// </summary>
    /// <param name="id"></param>
    /// <param name="itemDTO">StudentDTO object type</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> PutStudentItem(long id, StudentItemDTO itemDTO)
    {
        var studentItem = await _context.StudentItems.FindAsync(id);
        if (studentItem == null)
        {
            return NotFound();
        }

        studentItem.FirstName = itemDTO.FirstName;
        studentItem.LastName = itemDTO.LastName;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentItemExists(id))
            {
                return NotFound();
            }
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<StudentItemDTO>> PostStudentItem(StudentItemDTO itemDTO)
    {
        var studentExists = await _context.StudentItems.AnyAsync(stu => stu.Email == itemDTO.Email);
        if (studentExists)
        {
            return BadRequest();
        }

        var studentItem = new StudentItem
        {
            FirstName = itemDTO.FirstName,
            LastName = itemDTO.LastName,
            Email = itemDTO.Email,
            PaymentReference = Random.Shared.Next(1, 5)
        };

        return CreatedAtAction(nameof(GetStudentItem), new { Id = studentItem.Id }, ItemToDTO(studentItem));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudentItem(long id)
    {
        var student = await _context.StudentItems.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        _context.StudentItems.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StudentItemExists(long id)
    {
        return _context.StudentItems.Any(s => s.Id == id);
    }

    private static StudentItemDTO ItemToDTO(StudentItem item) =>
    new StudentItemDTO
    {
        Id = item.Id,
        Email = item.Email,
        FirstName = item.FirstName,
        LastName = item.LastName
    };
}