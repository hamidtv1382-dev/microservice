namespace Catalog_Service.src._03_Endpoints.DTOs.Responses.Admin
{
    public class CategoryTreeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public List<CategoryTreeResponse> SubCategories { get; set; } = new();
    }

}
