using System;

namespace SFA.DAS.Support.Shared.Tests
{
    public class TestType
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Value { get; set; }
    }
}