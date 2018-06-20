namespace SFA.DAS.Support.Deployment.Validator
{
    public enum StatusClientResults : int
    {
        /// <summary>
        /// All services were contacted successfully and returned 200
        /// </summary>
        Success = 0,
        /// <summary>
        /// The Primary site could not be contacted.
        /// </summary>
        Failed = 1,
        /// <summary>
        /// The primary service was contacted but returned non 200 code
        /// </summary>
        BadPrimary = 2,
        /// <summary>
        /// The primary service was contacted and returned 200 code, one or more of the secondary sites were contactad and at least one of them returned a non 200 code
        /// </summary>
        BadSecondary = 3,

        NothingToDo = 32
    }
}