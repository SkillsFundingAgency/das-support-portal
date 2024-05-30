namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public class CreateInvitationRequest
    {
        public string SupportUserEmail { get; set; }

        public string HashedAccountId { get; set; }

        public string NameOfPersonBeingInvited { get; set; }

        public string EmailOfPersonBeingInvited { get; set; }

        public string RoleOfPersonBeingInvited { get; set; }
    }
}