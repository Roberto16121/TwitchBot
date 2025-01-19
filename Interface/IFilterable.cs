namespace TwitchBot.Interface;

public interface IFilterable
{
    void SetFilter<T>(T filter) where T : struct;

    void SetTime(string time);

    void SetSearch(string word);
}