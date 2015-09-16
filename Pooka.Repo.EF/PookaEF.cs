namespace Pooka.Repo.EF
{
    using System.Data.Entity;
    using Contracts;

    public static class PookaEF
    {
        public static IRepository CreateRepositoryFromDbContext(DbContext dbContext, string connectionString)
        {
            return new PookaDbContext(dbContext, connectionString);
        }
    }
}