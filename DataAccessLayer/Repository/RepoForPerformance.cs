using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repository
{
    public class RepoForPerformance : IRepoForPerformance
    {
        private readonly AppDBContext _context;

        public RepoForPerformance(AppDBContext context)
        {
            _context = context;
        }

        public void GetMenu()
        {
        }
    }
}