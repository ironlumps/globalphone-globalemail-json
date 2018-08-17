using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalCheck
{

    public class Email
    {
        public class Rootobject
        {
            public string Version { get; set; }
            public string TransmissionReference { get; set; }
            public string TransmissionResults { get; set; }
            public string TotalRecords { get; set; }
            public Record[] Records { get; set; }
        }

        public class Record
        {
            public string RecordID { get; set; }
            public string Results { get; set; }
            public string EmailAddress { get; set; }
            public string MailboxName { get; set; }
            public string DomainName { get; set; }
            public string TopLevelDomain { get; set; }
            public string TopLevelDomainName { get; set; }
            public string DateChecked { get; set; }
        }
    }
    
   public class Phone
    {
        public class Rootobject
        {
            public string Version { get; set; }
            public string TransmissionReference { get; set; }
            public string TransmissionResults { get; set; }
            public string TotalRecords { get; set; }
            public Record[] Records { get; set; }
        }

        public class Record
        {
            public string RecordID { get; set; }
            public string Results { get; set; }
            public string PhoneNumber { get; set; }
            public string AdministrativeArea { get; set; }
            public string CountryAbbreviation { get; set; }
            public string CountryName { get; set; }
            public string Carrier { get; set; }
            public string CallerID { get; set; }
            public string DST { get; set; }
            public string InternationalPhoneNumber { get; set; }
            public string Language { get; set; }
            public string Latitude { get; set; }
            public string Locality { get; set; }
            public string Longitude { get; set; }
            public string PhoneInternationalPrefix { get; set; }
            public string PhoneCountryDialingCode { get; set; }
            public string PhoneNationPrefix { get; set; }
            public string PhoneNationalDestinationCode { get; set; }
            public string PhoneSubscriberNumber { get; set; }
            public string UTC { get; set; }
            public string PostalCode { get; set; }
            public string TimeZoneCode { get; set; }
            public string TimeZoneName { get; set; }
            public Suggestion[] Suggestions { get; set; }
        }

        public class Suggestion
        {
        }

    }



}
