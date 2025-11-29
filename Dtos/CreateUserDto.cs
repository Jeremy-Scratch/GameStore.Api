using System;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateUserDto
(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(50)] string Email,
    [Required][StringLength(50)] string Password
);
