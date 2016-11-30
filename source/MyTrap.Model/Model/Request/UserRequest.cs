using MyTrap.Model.Enums;
using System.Collections.Generic;

namespace MyTrap.Model.Mobile.Request
{
    public class UserRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CurrentPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public ERegisterType RegisterType { get; set; }
        public string RegisterProfileId { get; set; }
        public ImageRequest ProfilePicture { get; set; }
        public string AppRegistration { get; set; }
        public EPlatform Platform { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }
        public int Points { get; set; }
        public List<UserTrapRequest> Traps { get; set; }
    }
}