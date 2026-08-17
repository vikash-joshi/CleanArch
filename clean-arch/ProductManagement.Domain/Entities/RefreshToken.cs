public class RefreshToken
{
    public Guid Id {get;set;}
    public Guid UserId {get;set;}
     public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }

    public RefreshToken(Guid userId, string token, DateTime expiresAt)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
    }

    public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;

    public void Revoke() => IsRevoked = true;
}