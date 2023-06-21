using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_roles")]

public class Role
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }

    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}