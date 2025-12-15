using System.Runtime.Serialization;

namespace LoanReviewApi.Core.Enums
{
    [DataContract]
    public enum ReviewTemplates
    {
        [EnumMember(Value = "Residential")]
        Residential = 1,
        [EnumMember(Value = "Commercial")]
        Commercial = 2
    }
}
