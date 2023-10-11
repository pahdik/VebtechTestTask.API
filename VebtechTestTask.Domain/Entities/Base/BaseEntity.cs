using System.ComponentModel.DataAnnotations;

namespace VebtechTestTask.Domain.Entities.Base;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}