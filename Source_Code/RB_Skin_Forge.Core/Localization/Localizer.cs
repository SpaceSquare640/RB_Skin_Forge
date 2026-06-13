using RB_Skin_Forge.Core.Models;

namespace RB_Skin_Forge.Core.Localization;

/// <summary>One selectable UI language: its culture code and native display name.</summary>
public sealed record AppLanguage(string Code, string NativeName);

/// <summary>The languages RB Skin Forge's interface is translated into.</summary>
public static class Languages
{
    /// <summary>
    /// Order matters: every entry in <see cref="Strings"/> is an array indexed the
    /// same way as this list.
    /// </summary>
    public static readonly IReadOnlyList<AppLanguage> All = new[]
    {
        new AppLanguage("en",      "English"),
        new AppLanguage("zh-Hant", "繁體中文"),
        new AppLanguage("zh-Hans", "简体中文"),
        new AppLanguage("ja",      "日本語"),
        new AppLanguage("ko",      "한국어"),
        new AppLanguage("es",      "Español"),
        new AppLanguage("pt",      "Português"),
        new AppLanguage("fr",      "Français"),
        new AppLanguage("de",      "Deutsch"),
        new AppLanguage("ru",      "Русский"),
    };

    public const string DefaultCode = "en";

    public static int IndexOf(string code)
    {
        for (int i = 0; i < All.Count; i++)
            if (All[i].Code == code) return i;
        return 0; // fall back to English
    }
}

/// <summary>
/// Holds the active UI language and resolves localized strings. Shared by the
/// Blazor, Windows and Android front-ends so there is a single source of truth.
///
/// Console/log lines emitted by the processing engines stay in English on purpose
/// — they are technical diagnostics. Everything the user reads as interface chrome
/// (labels, buttons, headings, stat names, state words, body-part names) is
/// translated. The credit line is a required, verbatim attribution and is never
/// translated.
/// </summary>
public sealed class Localizer
{
    private int _index = Languages.IndexOf(Languages.DefaultCode);

    /// <summary>Raised after <see cref="SetLanguage"/> changes the active language.</summary>
    public event Action? Changed;

    public AppLanguage Current => Languages.All[_index];
    public string CurrentCode => Current.Code;

    public void SetLanguage(string code)
    {
        int idx = Languages.IndexOf(code);
        if (idx == _index) return;
        _index = idx;
        Changed?.Invoke();
    }

    /// <summary>Look up a key for the active language (falls back to English, then the key).</summary>
    public string T(string key)
    {
        if (Strings.Table.TryGetValue(key, out var values))
        {
            if (_index < values.Length && !string.IsNullOrEmpty(values[_index]))
                return values[_index];
            return values[0];
        }
        return key;
    }

    public string BodyPartName(BodyPart part) => T("bodypart." + part);
    public string StateName(TaskState state) => T("state." + state);
}
