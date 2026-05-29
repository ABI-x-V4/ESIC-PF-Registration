namespace DataModels
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int FilteredRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

    }

    public class EmployeeListRowDto
    {
        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? PanNo { get; set; }
        public string? AadhaarNo { get; set; }
    }

    public class EmployeeExportQuery
    {
        public string? Search { get; set; }
        public string? Gender { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortDir { get; set; } = "asc";
    }

}
