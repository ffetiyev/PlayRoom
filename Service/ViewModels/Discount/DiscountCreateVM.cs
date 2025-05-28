using FluentValidation;

namespace Service.ViewModels.Discount
{
    public class DiscountCreateVM
    {
        public decimal Value { get; set; }
    }

    public class DiscountCreateVMValidator : AbstractValidator<DiscountCreateVM>
    {
        public DiscountCreateVMValidator()
        {
            RuleFor(x => x.Value)
                .NotNull().WithMessage("Value cannot be null!")
                .InclusiveBetween(0, 100).WithMessage("Value should be between 0 and 100!");
        }
    }
}
