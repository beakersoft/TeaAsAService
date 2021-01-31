using System;
using System.ComponentModel.DataAnnotations;
using Tea.Core.Domain;

namespace Tea.Web.Models
{
	public class CreateUserModel
	{
		[Required]
		public string Password { get; set; }

		public string LocalizedString { get; set; }

	}
}

