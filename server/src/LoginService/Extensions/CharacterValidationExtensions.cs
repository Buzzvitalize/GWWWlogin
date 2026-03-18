namespace GWWWlogin.LoginService.Extensions;

public static class CharacterValidationExtensions
{
    private static readonly HashSet<string> AllowedClasses =
    [
        "Warrior",
        "Champion",
        "Priest",
        "Mage"
    ];

    private static readonly HashSet<string> AllowedGenders =
    [
        "Male",
        "Female"
    ];

    public static bool HasValidCharacterPayload(this string value, int minLength, int maxLength)
    {
        return !string.IsNullOrWhiteSpace(value) && value.Trim().Length >= minLength && value.Trim().Length <= maxLength;
    }

    public static bool IsAllowedCharacterClass(this string value)
    {
        return AllowedClasses.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsAllowedCharacterGender(this string value)
    {
        return AllowedGenders.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}
