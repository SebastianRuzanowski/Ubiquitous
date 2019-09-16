using U.SmartStoreAdapter.Domain.Entities.Catalog;
using U.SmartStoreAdapter.Domain.Entities.Media;
using Microsoft.EntityFrameworkCore;
using SmartStore.Persistance.EntityBuilders.Catalog;
using SmartStore.Persistance.EntityBuilders.Media;

namespace SmartStore.Persistance.Context
{
    public class SmartStoreContext : DbContext
    {
        public SmartStoreContext(DbContextOptions<SmartStoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<MediaStorage> MediaStorages { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new ProductMap());
            modelBuilder.ApplyConfiguration(new ManufacturerMap());
            modelBuilder.ApplyConfiguration(new PictureMap());
            modelBuilder.ApplyConfiguration(new ProductPictureMap());
            modelBuilder.ApplyConfiguration(new ProductManufacturerMap());
            modelBuilder.ApplyConfiguration(new MediaStorageMap());
            modelBuilder.ApplyConfiguration(new ProductCategoryMap());
        } 
    }
}