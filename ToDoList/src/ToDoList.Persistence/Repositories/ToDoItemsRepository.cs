namespace ToDoList.Persistence.Repositories
{
    using ToDoList.Domain.Models;

    public class ToDoItemsRepository : IRepository<ToDoItem>
    {
        private readonly ToDoItemsContext context;
        public ToDoItemsRepository(ToDoItemsContext context)
        {
            this.context = context;
        }

        public void Create(ToDoItem item)
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();
        }

        public bool DeletById(int id)
        {
            var item = context.ToDoItems.SingleOrDefault(i => i.ToDoItemId == id);
            if (item != null)
            {
                context.ToDoItems.Remove(item);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
