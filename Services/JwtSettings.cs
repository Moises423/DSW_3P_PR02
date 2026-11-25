namespace DSW_3P_PR02.Services;

public sealed class JwtSettings(string key, string issuer, string audience, int expireMinutes)
{
    public string Key { get; } = key;

    public string Issuer { get; } = issuer;

    public string Audience { get; } = audience;

    public int ExpireMinutes { get; } = expireMinutes;
}
