﻿using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;

namespace SFA.DAS.Support.Portal.ApplicationServices.Queries
{
    public class AccountTeamMembersQuery : IAsyncRequest<AccountDetailTeamMembersResponse>
    {
        public string Id { get; private set; }

        public AccountTeamMembersQuery(string id)
        {
            Id = id;
        }
    }
}