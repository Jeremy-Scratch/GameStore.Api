using System;

namespace GameStore.Api.Entities;

public class Sessions
{
    public required int Id { get; set; }
    public required string Token { get; set; }
    public required string UserId { get; set; }
    public required string CreatedAt { get; set; }
    public required string ExpiresAt { get; set; }
}
