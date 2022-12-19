namespace SimplifiedSearch
{
    /// <summary>
    /// Exposes methods for getting a searcher.
    /// </summary>
    public interface ISimplifiedSearchFactory
    {
        /// <summary>
        /// Get the default searcher.
        /// </summary>
        /// <returns></returns>
        ISimplifiedSearch Create();

        /// <summary>
        /// Get a named searcher. Or the default if the name is not found.
        /// </summary>
        /// <param name="name">Name of the searcher. Case insensitive.</param>
        /// <returns></returns>
        ISimplifiedSearch Create(string name);
    }
}
