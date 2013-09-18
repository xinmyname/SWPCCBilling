using System.Runtime.Serialization;

namespace SWPCCBilling.Models
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public string DatabasePath { get; set; }

		[DataMember]
		public string EmailServer { get; set; }
		[DataMember]
		public int EmailPort { get; set; }
		[DataMember]
		public bool EmailSSL { get; set; }
		[DataMember]
		public string EmailFrom { get; set; }
    }
}