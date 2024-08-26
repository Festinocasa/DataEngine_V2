namespace DataEngine.Utility
{
    public static class ArrayExtensions
    {
        public static T[] GetArrayRange<T>(this T[] source, int startIndex, int count)
        {
            T[] result = new T[count];
            int point = startIndex;
            for (int i = 0; i < count; i++)
            {
                result[i] = source[point++];
            }
            return result;
        }

        public static T[] AddArrayRange<T>(this T[] source, int startIndex, T[] range)
        {
            for (int i = 0; i < range.Length; i++)
            {
                source[startIndex + i] = range[i];
            }

            return source;
        }
    }
}
