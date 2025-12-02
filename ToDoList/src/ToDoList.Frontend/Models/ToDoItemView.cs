namespace ToDoList.Frontend.Models;

public class ToDoItemView(int id, string name, string description, bool isCompleted, string category)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public bool IsCompleted { get; set; } = isCompleted;
    public string Category { get; set; } = category;
}
