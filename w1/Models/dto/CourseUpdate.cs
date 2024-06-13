namespace w1.Models.dto;

public class CourseUpdate
{
    // public int CourseId { get; set; }
    public required string Title { get; set; }
    public int Credits { get; set; }
}