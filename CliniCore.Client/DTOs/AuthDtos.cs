namespace CliniCore.Client.DTOs
{
    // Lo que enviamos para entrar
    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Lo que la API nos responde (el token)
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}