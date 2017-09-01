public interface IContentEditor
{
    string ContentName { get; }

    void Draw();
    void ClearValues();
    void Save(string filePath);
    string Load(string filePath);
}
