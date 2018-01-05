namespace SFA.DAS.Support.Portal.Core.Services
{
    public interface IRoleSettings
    {
        string ConsoleUserRole { get; set; }
        string T2Role { get;  set;}
        bool ForceT2UserLocally { get;  set;}
        string GroupClaim { get;  set;}
        string Tier2Claim { get;  set;}
    }
}