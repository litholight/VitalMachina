namespace Mario.Common.Services
{
    public static class IdGenerator
    {
        private static long lastId = 0;

        public static string Generate()
        {
            return Interlocked.Increment(ref lastId).ToString();
        }
    }
}
