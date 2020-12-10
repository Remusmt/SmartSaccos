using SmartSaccos.Domains.Enums;
using System.Collections.Generic;

namespace SmartSaccos.ApplicationCore.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string MemberNumber { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string OtherNames { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IndentificationNo { get; set; }
        public int PassportCopyId { get; set; }
        public int SignatureId { get; set; }
        public int? HomeAddressId { get; set; }
        public int? PermanentAddressId { get; set; }
        public string NearestTown { get; set; }
        public string NextOfKin { get; set; }
        public string NokRelation { get; set; }
        public string NokContacts { get; set; }
        public bool NokIsMinor { get; set; }
        public string Occupation { get; set; }
        public OccupationType OccupationType { get; set; }
        public LearntAboutUs LearntAboutUs { get; set; }
        public AddressModel HomeAddress { get; set; }
        public AddressModel PermanentAddress { get; set; }

    }

    public class AddressModel
    {
        public int Id { get; set; }
        public string Village { get; set; }
        public string Location { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
    }
}
