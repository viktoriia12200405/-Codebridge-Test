namespace DAL.Models.Params;
public class PagingModel
{
    public int PageSize { get; set; } = int.MaxValue;

    public int PageNumber { get; set; } = 1;

    public int Limit { get; set; } = int.MaxValue;
}
