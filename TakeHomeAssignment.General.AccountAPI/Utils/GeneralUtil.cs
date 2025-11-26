namespace TakeHomeAssignment.General.AccountAPI.Utils
{
    internal static class GeneralUtil
    {
        internal static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }
    }
}
