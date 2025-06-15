using System.ComponentModel.DataAnnotations;

namespace Service.ViewModels.Setting
{
    public class SettingUpdateVM
    {
        public string Key { get; set; }
        [Required(ErrorMessage = "Value cannot be empty!")]
        public string? Value { get; set; }
    }
}
