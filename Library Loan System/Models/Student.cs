using System.ComponentModel.DataAnnotations;

namespace Library_Loan_System.Models;

public class Student
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Nim { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Major { get; set; } = string.Empty;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}