namespace w1.Models.dto;

public class CourseVeiwModel
{
    public int CourseId { get; set; }
    public required string Title { get; set; }
    public int Credits { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime DepartmentDate { get; set; }
}