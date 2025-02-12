namespace BuildingBlocks.BaseResponse;

public record GetAllResponse<T> where T : class
{
    public IEnumerable<T>? Records { get; set; }
    public long PageNumber { get; set; }
    public long PageSize { get; set; }
    public long NumberOfPages { get; set; }
    public long NumberOfRecordsInPage { get; set; }
    public long NumberOfRecordsReturned { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
