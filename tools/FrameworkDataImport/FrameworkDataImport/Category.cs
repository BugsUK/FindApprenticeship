namespace FrameworkDataImport
{
    using System.Collections.Generic;

    public class Category
    {
        public string FullName { get; set; }
        public string CodeName { get; set; }
        public int[] Levels { get; set; }
        public List<Category> SubCategories { get; set; }
    }
}