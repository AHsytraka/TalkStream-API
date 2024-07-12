namespace TalkStream_API.Helpers
{
    public static class GroupHelper
    {
        public static string GetGroupName(string userId1, string userId2)
        {
            return string.CompareOrdinal(userId1, userId2) < 0 ? $"{userId1}-{userId2}" : $"{userId2}-{userId1}";
        }
    }
}