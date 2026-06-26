using System.ComponentModel.DataAnnotations;

namespace Library_Loan_System.Models;

public class Book
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Author { get; set; } = string.Empty;

    [Required]
    public int Stock { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}