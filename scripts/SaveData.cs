
namespace Game;

public class SaveData
{
    public string text { get; private set; }
    public ClassData classData { get; private set; }
    public DifficultyData difficultyData { get; private set; }

    public SaveData(
        string text,
        ClassData classData,
        DifficultyData difficultyData
        )
    {
        this.text = text;
        this.classData = classData;
        this.difficultyData = difficultyData;
    }

}