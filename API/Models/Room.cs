using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_rooms")]

public class Room
{
    [Key]
    [Column("guid")]
    public Guid Guid { get; set; }

    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    [Column("floor")]
    public int Floor { get; set; }

    [Column("capacity")]
    public int Capacity { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
