namespace Models.Dtos
{
    public class Query
    {
        public string SearchPhrase { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string SortBy { get; set; } = string.Empty;
        public bool SortDirection { get; set; } = false;
    }
}
