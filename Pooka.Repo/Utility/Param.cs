namespace Pooka.Repo.Utility
{
    using System;

    public class Param
    {
        public static void CheckNotNull<T>(T value, string paramName)
        {
            if (null == value)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void CheckValue<T>(T value, Func<T, bool> predicate, string errorMessage)
        {
            if (predicate(value))
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public static void CheckStringNotNullOrEmpty(string value, string paramName)
        {
            if (null == value)
            {
                throw new ArgumentNullException(paramName);
            }

            CheckValue(value, string.IsNullOrEmpty, $"{nameof(paramName)} cannote be empty");
        }
    }
}