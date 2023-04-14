using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework.Contexts
{
    public class SpotifyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SpotifyDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<UserOperation> UserOperations { get; set; }
        public DbSet<GetAccessToken> AccessTokens { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<UserLibrary> UserLibraries { get; set; }
        public DbSet<UserPlaylist> UserPlaylists { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Follow> Follows { get; set; }



    }
}
