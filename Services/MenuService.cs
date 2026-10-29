using Blanza_Midterm.Models;

namespace Blanza_Midterm.Services
{
    // Owns the menu data and the search/filter logic so pages stay purely presentational.
    public class MenuService
    {
        private static readonly List<MenuItem> Items = new()
        {
            // Filipino
            new MenuItem { Id = 1, Name = "Pork Sisig",       Category = "Filipino", Price = 250, Rating = 5.0, ReviewCount = 340, ImageUrl = "images/sisig.png" },
            new MenuItem { Id = 2, Name = "Beef Kaldereta",   Category = "Filipino", Price = 320, Rating = 4.8, ReviewCount = 210, ImageUrl = "images/kaldereta.png" },
            new MenuItem { Id = 4, Name = "Chicken Inasal",   Category = "Filipino", Price = 220, Rating = 4.7, ReviewCount = 180, ImageUrl = "images/cinasal.png" },
            new MenuItem { Id = 5, Name = "Chicken Adobo",    Category = "Filipino", Price = 200, Rating = 4.6, ReviewCount = 250, ImageUrl = "images/adobo.png" },

            // Dessert
            new MenuItem { Id = 3, Name = "Halo-Halo",        Category = "Dessert",  Price = 150, Rating = 4.9, ReviewCount = 500, ImageUrl = "images/halohalo.png" },

            // Italian
            new MenuItem { Id = 6, Name = "Classic Lasagna",  Category = "Italian",  Price = 350, Rating = 4.9, ReviewCount = 150, ImageUrl = "images/lasagna.png" },
            new MenuItem { Id = 8, Name = "Margherita Pizza", Category = "Italian",  Price = 450, Rating = 4.7, ReviewCount = 310, ImageUrl = "images/pizza.png" }
        };

        public IReadOnlyList<MenuItem> GetAll() => Items;

        public IReadOnlyList<string> GetCategories() =>
            Items.Select(i => i.Category).Distinct().OrderBy(c => c).ToList();

        // Filters by an optional category and an optional free-text term (name or category).
        public IReadOnlyList<MenuItem> Search(string? term, string? category)
        {
            IEnumerable<MenuItem> results = Items;

            if (!string.IsNullOrWhiteSpace(category))
                results = results.Where(i => i.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(term))
            {
                var t = term.Trim();
                results = results.Where(i =>
                    i.Name.Contains(t, StringComparison.OrdinalIgnoreCase) ||
                    i.Category.Contains(t, StringComparison.OrdinalIgnoreCase));
            }

            return results.ToList();
        }
    }
}
