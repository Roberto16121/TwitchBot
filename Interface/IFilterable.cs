namespace TwitchBot.Interface;

public interface IFilterable
{
    void SetFilter<T>(T filter) where T : struct;

    void SetSearch(string word);
}

public struct ModuleFilter
{
    public bool HideObsModules;
    public bool HideSoundModules;
    public int MinNrOfUses;
}

public struct UserFilter
{
    public int MinNrMessages;
    public int MinNrOfModUsages;
    public int MinViewtime;
    public int SelectedIndex;

}