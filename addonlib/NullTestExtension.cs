namespace addonlib
{
    public static class NullTestExtension
    {
        public static bool IsNullFromOtherDLL<T>(this T obj) where T: class
        {
            return obj == null;
        }
    }
}
