namespace w1.Models.dto;

public class GetCoursesByPageViewModel
{

    public int Total { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<CourseVeiwModel> Data { get; set; }


}