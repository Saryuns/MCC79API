using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_educations")]

public class Education : BaseEntity
{
    [Column("university_guid")]
    public Guid UniversityGuid { get; set; }

    [Column("major", TypeName = "nvarchar(100)")]
    public string Code { get; set; }

    [Column("degree", TypeName = "nvarchar(100)")]
    public string Degree { get; set; }

    [Column("gpa")]
    public double GPA { get; set; }

    //Cardinality
    public University? University { get; set; }
    public Employee? Employee { get; set; }
}