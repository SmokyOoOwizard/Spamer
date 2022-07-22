using System.ComponentModel.DataAnnotations;

namespace Spamer
{
	class FileMustExistAttribute : ValidationAttribute
	{
		public FileMustExistAttribute()
			: base("File must exists")
		{
		}

		protected override ValidationResult IsValid(object value, ValidationContext context)
		{
			if ((value as string) == null)
			{
				return new ValidationResult(FormatErrorMessage(context.DisplayName));
			}
			var path = value as string;

			if (!File.Exists(path))
			{
				return new ValidationResult("File must exists");
			}

			return ValidationResult.Success;
		}
	}
}