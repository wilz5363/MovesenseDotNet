using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovesenseDemo.Model
{
	public class HRData
	{
		[JsonProperty("Body")]
		public Body Data;
	}
	public class Body
	{
		[JsonProperty("average")]
		public float Average { get; set; }

		[JsonProperty("rrData")]
		public RrDataArray[] RrData { get; set; }

	}
	public class RrDataArray
	{
		public short Second { get; set; }
	}
	
}
