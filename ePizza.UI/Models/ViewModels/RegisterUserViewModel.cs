namespace ePizza.UI.Models.ViewModels
{
    // don't forget to add data annotations
    public class RegisterUserViewModel
    {

        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

    }
}
