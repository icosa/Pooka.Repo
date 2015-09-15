namespace Pooka.Repo.Contracts
{
    using HandlerUtility;

    public interface IPookaHandlers
    {
        HandlerCollectionBuilder CommandHandlers { get; }

        HandlerCollectionBuilder QueryHandlers { get; }
    }
}