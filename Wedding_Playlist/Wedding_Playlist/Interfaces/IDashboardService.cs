using Wedding_Playlist.Models;
using System.Threading.Tasks;

namespace Wedding_Playlist.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardData();
    }
} 