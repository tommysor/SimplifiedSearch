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
        /// Get a named searcher.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISimplifiedSearch Create(string name);
    }
}
