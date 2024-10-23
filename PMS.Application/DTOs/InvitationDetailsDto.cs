namespace PMS.Application.DTOs
{
    public class InvitationDetailsDto
    {
        public InvitationDto Invitation { get; set; }
        public bool UserExists { get; set; }
    }

    public class InvitationFilterDto
    {
        public InvitationFilterDto(int take, int skip, string? search, string orderBy, string orderDirection)
        {
            Take = take;
            Skip = skip;
            Search = search;
            SortDirection = orderDirection;
            SortBy = orderBy;
        }

        public int Take { get; set; }
        public int Skip { get; set; }
        public string? Search { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
    }
}
