namespace Game;

public class OptionsData
{
    //Audio
    public double masterVolume { get; private set; }
    public double musicVolume { get; private set; }
    public double sfxVolume { get; private set; }

    //Display
    public string windowMode { get; private set; }
    public string resolution { get; private set; }

    public OptionsData(
        double masterVolume,
        double musicVolume,
        double sfxVolume,
        string windowMode,
        string resolution
        )
    {
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.windowMode = windowMode;
        this.resolution = resolution;
    }

}