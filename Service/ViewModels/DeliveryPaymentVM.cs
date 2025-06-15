using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels
{
    public class DeliveryPaymentVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title cannot be null!")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Content cannot be null!")]
        public string Content { get; set; }
    }
}
