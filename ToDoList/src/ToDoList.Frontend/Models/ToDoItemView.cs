namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItemView
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Je třeba zadat název úkolu.")]
    public required string Name { get; set; }
    [Required(ErrorMessage = "Je třeba vyplnit popis úkolu.")]
    [StringLength(250, ErrorMessage = "Popis úkolu nesmí být delší než 250 znaků.")]
    public required string Description { get; set; }
    public bool IsCompleted { get; set; }
    public string? Category { get; set; }
}
