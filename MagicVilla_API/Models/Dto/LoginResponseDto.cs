﻿namespace MagicVilla_API.Models.Dto
{
    public class LoginResponseDto
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}