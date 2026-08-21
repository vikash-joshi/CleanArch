public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsDeleted { get; private set; }

    public Guid? ParentCategoryId { get; private set; }

    public Category(Guid id, string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        Id = id;
        Name = name;
        Description = description;
    }

    public void UpdateCategory(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        Name = name;
        Description = description;
    }

    public void MarkDeleted()
    {
        IsDeleted = true;
    }
}