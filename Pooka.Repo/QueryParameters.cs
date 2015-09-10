namespace Pooka.Repo
{
    using System;

    public class QueryParameters<T> where T : class
    {
        public Type EntityType => typeof(T);
    }
}