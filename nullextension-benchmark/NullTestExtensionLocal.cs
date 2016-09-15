namespace nullextension_benchmark
{
    public static class NullTestExtensionLocal
    {
        public static bool IsNullLocal<T>(this T obj) where T : class
        {
            return obj == null;
        }
    }
}
