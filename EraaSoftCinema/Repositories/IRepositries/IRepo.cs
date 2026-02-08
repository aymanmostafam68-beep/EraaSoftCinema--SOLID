namespace EraaSoftCinema.Repositories.IRepositries
{
    public interface IRepo<T> where T : class
    {
        void fileUpload(IFormFile file, string location, out string newFileName, bool replcae = false);
        
        void filesUpload(IFormFile[] file, string location, out List<string> newFileNames, bool replcae = false);

        Task Create(T entity);

        Task update(T entity);
        Task Delete(T entity);
        Task DeleteRange(IEnumerable<T> entity);
        Task<int> Comment();

        Task<List<T>> GetAll(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null, bool tracked = true);
    
        Task<T> GetOne(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>>? includeFunc = null, bool tracked = true);
    }
}
