namespace Pooka.Repo
{
    using Contracts;
    using HandlerUtility;

    public class PookaHandlers : IPookaHandlers
    {
        public PookaHandlers(HandlerCollectionBuilder commandHandlers, HandlerCollectionBuilder queryHandlers)
        {
            CommandHandlers = commandHandlers;
            QueryHandlers = queryHandlers;
        }

        public HandlerCollectionBuilder CommandHandlers { get; }

        public HandlerCollectionBuilder QueryHandlers { get; }
    }
}