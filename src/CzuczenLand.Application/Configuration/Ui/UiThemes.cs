using System.Collections.Generic;

namespace CzuczenLand.Configuration.Ui;

public static class UiThemes
{
    public static List<UiThemeInfo> All { get; }

    static UiThemes()
    {
        All = new List<UiThemeInfo>
        {
            new UiThemeInfo("Czerwony", "red"),
            new UiThemeInfo("Różowy", "pink"),
            new UiThemeInfo("Purpurowy", "purple"),
            new UiThemeInfo("Głęboki fiolet", "deep-purple"),
            new UiThemeInfo("Indygo", "indigo"),
            new UiThemeInfo("Niebieski", "blue"),
            new UiThemeInfo("Jasny niebieski", "light-blue"),
            new UiThemeInfo("Cyjan", "cyan"),
            new UiThemeInfo("Cyraneczka", "teal"),
            new UiThemeInfo("Zielony", "green"),
            new UiThemeInfo("Jasnozielony", "light-green"),
            new UiThemeInfo("Limonka", "lime"),
            new UiThemeInfo("Żółty", "yellow"),
            new UiThemeInfo("Bursztyn", "amber"),
            new UiThemeInfo("Pomarańczowy", "orange"),
            new UiThemeInfo("Głęboki pomarańczowy", "deep-orange"),
            new UiThemeInfo("brązowy", "brown"),
            new UiThemeInfo("Szary", "grey"),
            new UiThemeInfo("Niebiesko szary", "blue-grey"),
            new UiThemeInfo("Czarny", "black")
        };
    }
}