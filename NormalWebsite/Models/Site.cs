using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NormalWebsite.Models
{
	/// <summary>
	/// Simple data class: serialized/deserialized via json 
	/// </summary>
	public class Site
	{
		public int company_id;
		public string company_name;
		public string site_id;
		public string site_name;
		public string site_short_name;
	}
}