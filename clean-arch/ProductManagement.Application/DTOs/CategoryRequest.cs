public record CreateCategoryRequest(string Name, string Description);
public record UpdateCategoryRequest(string id, string Name, string Description);

public record DeleteCategoryRequest(string id);