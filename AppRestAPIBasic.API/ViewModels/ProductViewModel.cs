using System.ComponentModel.DataAnnotations;

namespace AppRestAPIBasic.Api.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The field {0} is required.")]
        public Guid SupplierId { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(200, ErrorMessage = "The field {0} need to have between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(1000, ErrorMessage = "The field {0} need to have between {2} and {1} characters", MinimumLength = 2)]
        public string Description { get; set; }
        public string ImageUpload { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "The field {0} is required.")]
        public decimal Value { get; set; }
        [ScaffoldColumn(false)]
        public DateTime RegisteredDate { get; set; }
        public bool Active { get; set; }
        [ScaffoldColumn(false)]
        public string SupplierName { get; set; }
    }
}