using Microsoft.EntityFrameworkCore;


namespace WebApiWithRoleAuthentication.Helper
{
    public class PaginationList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PaginationList(int currentPage, int pageSize, List<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            AddRange(items);//將一個集合中的所有元素添加到清單末尾。
        }

        public static async Task<PaginationList<T>> CreateAsync(
            int currentPage, int pageSize, IQueryable<T> result)
        {
            //pageNumber = 2 pageSize = 10
            //skip = (2 - 1) * 10 = 10;
            var skip = (currentPage - 1) * pageSize;
            result = result.Skip(skip);
            result = result.Take(pageSize);
            //result = result.Skip(10).Take(10); // 取第11~20筆資料
            var items = await result.ToListAsync();

            return new PaginationList<T>(currentPage, pageSize, items);
        }
    }
}
