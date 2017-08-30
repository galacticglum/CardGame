public interface IEntityEditor
{
    string EntityName { get; }

    void Draw();
    void ClearValues();
    void Save(string filePath);
    string Load(string filePath);
}
