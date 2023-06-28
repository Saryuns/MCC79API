namespace API.DTOs.Universities;

public class UpdateUniversityDto
{
    public Guid Guid { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}