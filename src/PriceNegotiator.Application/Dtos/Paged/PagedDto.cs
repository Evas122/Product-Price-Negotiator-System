namespace PriceNegotiator.Application.Dtos.Paged;

public class PagedDto<T>
{
    public List<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }

    public PagedDto(List<T> items, int totalItems, int pageSize, int pageNumber)
    {
        Items = items;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        TotalItemsCount = totalItems;

        if (totalItems != 0)
        {
            ItemsFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = Math.Min(ItemsFrom + pageSize - 1, totalItems);
        }
        ItemsFrom = 0;
        ItemsTo = 0;
    }
}