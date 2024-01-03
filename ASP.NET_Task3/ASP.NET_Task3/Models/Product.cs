using ASP.NET_Task3.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Task3.Models
{
    public class Product : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }
        public List<Tag> Tags { get; set; } = null!;




        public static implicit operator Product(AddProductViewModel viewModel)
        {
            return new()
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                Price = viewModel.Price,
                ImageUrl = viewModel.ImageUrl,
            };
        }
    }
}
