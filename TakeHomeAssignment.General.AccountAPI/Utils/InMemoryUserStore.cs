using TakeHomeAssignment.General.AccountAPI.Model;

namespace TakeHomeAssignment.General.AccountAPI.Utils
{
    public static class InMemoryUserStore
    {
        public static readonly Dictionary<string, User> Users = new();
    }
}
