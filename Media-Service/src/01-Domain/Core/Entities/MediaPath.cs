using Media_Service.src._01_Domain.Core.Common;

namespace Media_Service.src._01_Domain.Core.Entities
{
    public class MediaPath : ValueObject
    {
        public string RootPath { get; private set; }
        public string BrandPath { get; private set; }
        public string CategoryPath { get; private set; }
        public string SubCategoryPath { get; private set; }
        public string FileName { get; private set; }

        public MediaPath(string root, string brand, string category, string subCategory, string fileName)
        {
            RootPath = root;
            BrandPath = brand;
            CategoryPath = category;
            SubCategoryPath = subCategory;
            FileName = fileName;
        }

        public string BuildFullPath()
        {
            var parts = new List<string> { RootPath };

            if (!string.IsNullOrWhiteSpace(BrandPath)) parts.Add(BrandPath);
            if (!string.IsNullOrWhiteSpace(CategoryPath)) parts.Add(CategoryPath);
            if (!string.IsNullOrWhiteSpace(SubCategoryPath)) parts.Add(SubCategoryPath);

            // Product files go into 'products' folder
            // Assuming FileName logic handles if it's a folder or a file

            return Path.Combine(parts.ToArray());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RootPath;
            yield return BrandPath;
            yield return CategoryPath;
            yield return SubCategoryPath;
            yield return FileName;
        }
    }
}
