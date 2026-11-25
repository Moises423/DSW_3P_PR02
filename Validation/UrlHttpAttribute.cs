using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UrlHttpAttribute : ValidationAttribute
{
    public UrlHttpAttribute()
        : base("La URL de imagen debe ser absoluta y usar el esquema HTTP o HTTPS.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        return value is string url
            && Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && !string.IsNullOrWhiteSpace(uri.Host)
            && (uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase)
                || uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase));
    }
}
