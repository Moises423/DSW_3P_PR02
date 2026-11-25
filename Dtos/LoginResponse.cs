namespace DSW_3P_PR02.Dtos;

/// <summary>Token JWT emitido por la API.</summary>
public sealed record LoginResponse(string Token, string TokenType, DateTimeOffset ExpiresAt);
