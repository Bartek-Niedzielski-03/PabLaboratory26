namespace AppCore.ValueObjects;

public class PhoneNumber
{
    public string Value { get; }
    public string CountryCode { get; }
    public string CountryName { get; }

    private static readonly Dictionary<string, string> CountryCodes = new()
    {
        { "48", "Polska" },
        { "1",  "USA/Kanada" },
        { "44", "Wielka Brytania" },
        { "49", "Niemcy" },
        { "33", "Francja" },
        { "39", "Włochy" },
        { "34", "Hiszpania" },
        { "380", "Ukraina" },
        { "7",  "Rosja" },
        { "420", "Czechy" },
    };

    private PhoneNumber(string value, string countryCode, string countryName)
    {
        Value = value;
        CountryCode = countryCode;
        CountryName = countryName;
    }

    public static PhoneNumber Create(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            throw new ArgumentException("Numer telefonu nie może być pusty.");

        //usuń spacje. myślniki,nawiasy
        var digits = new string(raw.Where(c => c == '+' || char.IsDigit(c)).ToArray());

        //zamian +48 na 48
        if (digits.StartsWith("+"))
            digits = digits[1..];

        //dopasowanie kodu do kraju
        foreach (var len in new[] { 3, 2, 1 })
        {
            if (digits.Length <= len) continue;
            var prefix = digits[..len];
            if (CountryCodes.TryGetValue(prefix, out var country))
                return new PhoneNumber(digits, "+" + prefix, country);
        }

        //nieznany kraj
        return new PhoneNumber(digits, "?", "Nieznany");
    }

    public override string ToString() => Value;

    public string ToDisplayString() => $"{CountryCode} {Value.Substring(CountryCode.Length - 1)}";
}