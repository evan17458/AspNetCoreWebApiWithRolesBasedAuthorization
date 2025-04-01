using Microsoft.EntityFrameworkCore;


namespace WebApiWithRoleAuthentication.Helper
{
    public class PaginationList<T> : List<T>
    {
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginationList(int totalCount, int currentPage, int pageSize, List<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);//將一個集合中的所有元素添加到清單末尾。
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public static async Task<PaginationList<T>> CreateAsync(
            int currentPage, int pageSize, IQueryable<T> result)
        {
            var totalCount = await result.CountAsync();
            //pageNumber = 2 pageSize = 10
            //skip = (2 - 1) * 10 = 10;
            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip);
            result = result.Take(pageSize);
            //result = result.Skip(10).Take(10); // 取第11~20筆資料
            var items = await result.ToListAsync();

            return new PaginationList<T>(totalCount, currentPage, pageSize, items);
        }
    }
}
