namespace Pooka.Repo.HandlerUtility
{
    using System;
    using System.Collections.Generic;

    public class HandlerTypeComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode(Type obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}