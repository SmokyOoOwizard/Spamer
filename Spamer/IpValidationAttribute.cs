using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Spamer
{
	class IpValidationAttribute : ValidationAttribute
	{
		public IpValidationAttribute()
			: base("The value for {0} must be IP (like 127.0.0.1)")
		{
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			if ((value as string) == null)
			{
				return new ValidationResult(FormatErrorMessage(context.DisplayName));
			}

			var rawIp = value as string;
			if (IPAddress.TryParse(rawIp, out _))
			{
				return ValidationResult.Success;
			}

			var ips = Dns.GetHostAddresses(rawIp);
			if (ips != null && ips.Length > 0)
			{
				return ValidationResult.Success;
			}

			return new ValidationResult(FormatErrorMessage(context.DisplayName));
		}
	}
}