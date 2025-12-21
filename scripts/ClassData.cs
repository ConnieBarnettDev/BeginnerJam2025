using Godot;

namespace Game;

public class ClassData
{
    public string name { get; private set; }
    public string description { get; private set; }
    public string imagePath { get; private set; }
    public Color color { get; private set; }

    public ClassData(
        string name,
        string description,
        string imagePath,
        Color color
    )
    {
        this.name = name;
        this.description = description;
        this.imagePath = imagePath;
        this.color = color;
    }
}