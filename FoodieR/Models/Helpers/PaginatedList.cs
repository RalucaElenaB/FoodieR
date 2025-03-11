namespace FoodieR.Models.Helpers;

//modelul PagedList<T>, care extinde List<T> și gestionează informațiile despre paginare
public class PagedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalNumberOfPages { get; private set; }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalNumberOfPages;

    public PagedList(List<T> items, int count, int pageIndex, int pageSize)//constructor
    {
        PageIndex = pageIndex;//initializare

        TotalNumberOfPages = (int)Math.Ceiling(count / (double)pageSize);//setarea paginii curente

        AddRange(items);
    }
}
