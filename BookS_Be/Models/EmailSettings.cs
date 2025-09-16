namespace BookS_Be.Models;

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool UseStartTls { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromName { get; set; } = "BookS Support";
    public string FromAddress { get; set; } = string.Empty;
}