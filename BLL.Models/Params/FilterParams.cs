namespace BLL.Models.Params;
public class FilterParams
{
    public string Attribute { get; set; } = "";

    public string Order { get; set; } = "";

    public int PageSize { get; set; } = int.MaxValue;

    public int PageNumber { get; set; } = 1;

    public int Limit { get; set; } = int.MaxValue;
}
