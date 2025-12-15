using LoanReviewApi.Core.Enums;
using LoanReviewApi.Core.Models.Loans;

namespace LoanReviewApi.Infrastructure.QueryFilters
{
    public static class LoanReviewDetailQueryFilters
    {
        public static IQueryable<LoanReviewDetail> SearchByProductCode(this IQueryable<LoanReviewDetail> query, int templateId, string productCode)
        {
            if (string.IsNullOrWhiteSpace(productCode))
            {
                return query;
            }

            if (templateId == (int)ReviewTemplates.Residential)
            {
                return query
                    .Where(ent => ent.LoanDetail != null)
                    .Where(ent => ent.LoanDetail!.ProductBusinessGroupCode != null &&
                    ent.LoanDetail!.ProductBusinessGroupCode.StartsWith(productCode, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return query
                    .Where(ent => ent.LoanDetailsComm != null)
                    .Where(ent => ent.LoanDetailsComm!.ProductBusinessGroupCode != null &&
                    ent.LoanDetailsComm!.ProductBusinessGroupCode.StartsWith(productCode, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public static IQueryable<LoanReviewDetail> SearchByTeam(this IQueryable<LoanReviewDetail> query, int templateId, string team)
        {
            if (string.IsNullOrWhiteSpace(team))
            {
                return query;
            }

            if (templateId == (int)ReviewTemplates.Residential)
            {
                return query
                    .Where(ent => ent.LoanDetail != null)
                    .Where(ent => ent.LoanDetail!.BranchOfficeId != null &&
                    ent.LoanDetail!.BranchOfficeId.StartsWith(team, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return query
                    .Where(ent => ent.LoanDetailsComm != null)
                    .Where(ent => ent.LoanDetailsComm!.BranchOfficeId != null &&
                    ent.LoanDetailsComm!.BranchOfficeId.StartsWith(team, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public static IQueryable<LoanReviewDetail> SearchByDirector(this IQueryable<LoanReviewDetail> query, int templateId, string director)
        {
            if (string.IsNullOrWhiteSpace(director))
            {
                return query;
            }

            if (templateId == (int)ReviewTemplates.Residential)
            {
                return query
                    .Where(ent => ent.LoanDetail != null)
                    .Where(ent => ent.LoanDetail!.Director != null &&
                    ent.LoanDetail!.Director.StartsWith(director, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return query
                    .Where(ent => ent.LoanDetailsComm != null)
                    .Where(ent => ent.LoanDetailsComm!.Director != null &&
                    ent.LoanDetailsComm!.Director.StartsWith(director, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public static IQueryable<LoanReviewDetail> SearchByReviewer(this IQueryable<LoanReviewDetail> query, int? reviewerId)
        {
            if (!reviewerId.HasValue)
            {
                return query;
            }

            return query.Where(ent => ent.CompletedBy != null && ent.CompletedBy == reviewerId);
        }

        public static IQueryable<LoanReviewDetail> SearchByReviewType(this IQueryable<LoanReviewDetail> query, int templateId, string reviewType)
        {
            if (string.IsNullOrWhiteSpace(reviewType))
            {
                return query;
            }

            if (templateId == (int)ReviewTemplates.Residential)
            {
                return query
                    .Where(ent => ent.LoanDetail != null)
                    .Where(ent => ent.LoanDetail!.ReviewType != null &&
                    ent.LoanDetail!.ReviewType.StartsWith(reviewType, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return query
                    .Where(ent => ent.LoanDetailsComm != null)
                    .Where(ent => ent.LoanDetailsComm!.ReviewType != null &&
                    ent.LoanDetailsComm!.ReviewType.StartsWith(reviewType, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public static IQueryable<LoanReviewDetail> SearchByStatus(this IQueryable<LoanReviewDetail> query, int? status)
        {
            if (!status.HasValue)
            {
                return query;
            }

            return query.Where(ent => ent.StatusId != null && ent.StatusId == status);
        }

        public static IQueryable<LoanReviewDetail> SearchByMonth(this IQueryable<LoanReviewDetail> query, int templateId, DateTime? month)
        {
            if (!month.HasValue)
            {
                return query;
            }

            if (templateId == (int)ReviewTemplates.Residential)
            {
                return query.Where(ent => ent.LoanDetail != null).Where(ent => ent.LoanDetail!.Date1 != null && ent.LoanDetail!.Date1.Value == month.Value);
            }
            else
            {
                return query.Where(ent => ent.LoanDetailsComm != null).Where(ent => ent.LoanDetailsComm!.Date1 != null && ent.LoanDetailsComm!.Date1.Value == month.Value);
            }
        }

        public static IQueryable<LoanReviewDetail> SearchByQuarter(this IQueryable<LoanReviewDetail> query, int templateId, DateTime? startTime, DateTime? endTime)
        {
            if (!startTime.HasValue || !endTime.HasValue)
            {
                return query;
            }

            if (templateId == (int)ReviewTemplates.Residential)
            {
                return query
                    .Where(ent => ent.LoanDetail != null)
                    .Where(ent => ent.LoanDetail!.Date1 != null && (ent.LoanDetail!.Date1.Value >= startTime.Value && ent.LoanDetail!.Date1.Value <= endTime.Value));
            }
            else
            {
                return query
                    .Where(ent => ent.LoanDetail != null)
                    .Where(ent => ent.LoanDetailsComm!.Date1 != null && (ent.LoanDetailsComm!.Date1.Value >= startTime.Value && ent.LoanDetailsComm!.Date1.Value <= endTime.Value));
            }
        }
    }
}
