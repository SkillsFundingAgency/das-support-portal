namespace SFA.DAS.Support.Portal.ApplicationServices.Services
{
    public class CreateInvitationRequest
    {
        public string NameOfPersonBeingInvited { get; set; }
        public string EmailOfPersonBeingInvited { get; set; }
        public string RoleOfPersonBeingInvited { get; set; }
    }
}