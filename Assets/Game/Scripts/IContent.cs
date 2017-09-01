public interface IContent<out T> where T : new()
{
    /// <summary>
    /// Convert the object to JSON.
    /// </summary>
    /// <returns>The JSON contained the object data.</returns>
    string Save();

    /// <summary>
    /// Convert the JSON to object data.
    /// </summary>
    /// <param name="json"></param>
    /// <returns>The object with loaded values.</returns>
    T Load(string json);
}
