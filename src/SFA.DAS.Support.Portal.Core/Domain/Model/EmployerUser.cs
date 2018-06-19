using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Core.Domain.Model
{
    [ExcludeFromCodeCoverage]
    public class EmployerUser : IUser
    {
        public int FailedLoginAttempts { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }

        public UserStatus Status => DetermineStatus();

        private UserStatus DetermineStatus()
        {
            if (IsActive && IsLocked)
                return UserStatus.Locked;
            if (IsActive && IsLocked == false)
                return UserStatus.Active;

            return UserStatus.Unverified;
        }
    }
}