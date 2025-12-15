using System.Runtime.Serialization;

namespace LoanReviewApi.Core.Enums
{
    [DataContract]
    public enum LoanReviewStatuses
    {
        [EnumMember(Value = "In Review")]
        InReview = 1,
        [EnumMember(Value = "Review Complete")]
        ReviewComplete = 2,
        [EnumMember(Value = "New")]
        New = 3
    }
}
