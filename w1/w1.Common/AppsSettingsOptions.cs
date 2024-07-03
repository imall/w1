

using System.ComponentModel.DataAnnotations;

namespace w1.Common;

public class AppsSettingsOptions
{
    public const string Appsettings = "Appsettings";

    [Required(ErrorMessage = "Somekey is required.")]
    public string Somekey { get; set; }

    [Required(ErrorMessage = "Smtpip is required.")]
    [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "Smtpip must be a valid IPv4 address.")]
    public string SmtpIp { get; set; }

    [Range(1, 65535, ErrorMessage = "Smtpport must be between 1 and 65535.")]
    public int Smtpport { get; set; }
}