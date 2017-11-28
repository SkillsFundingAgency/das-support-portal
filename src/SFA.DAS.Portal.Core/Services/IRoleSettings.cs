namespace SFA.DAS.Portal.Core.Services
{
    public interface IRoleSettings
    {
        string ConsoleUserRole { get; }
        string T2Role { get; }
        bool ForceT2UserLocally { get; }
        string GroupClaim { get; }
        string Tier2Claim { get; }
    }
}