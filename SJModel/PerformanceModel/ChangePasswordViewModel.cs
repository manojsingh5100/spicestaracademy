using SJModel.PTAModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SJModel.PerformanceModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old Password is required.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Minimum 6 characters required")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "Minimum 6 characters required")]
        [Compare("NewPassword", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ReEnterNewPassword { get; set; }

        public string Email { get; set; }
        public string IsOldPasswordExist { get; set; }
        public string Message { get; set; }
    }
}
