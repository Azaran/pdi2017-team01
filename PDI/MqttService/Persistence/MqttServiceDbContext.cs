using System.Data.Entity;
using MqttService.Persistence.Entity;
using System.Data.SQLite;

namespace MqttService.Persistence
{
    public class MqttServiceDbContext : DbContext
    {
        public MqttServiceDbContext() : base("name=Database")
        {
            Database.SetInitializer<MqttServiceDbContext>(
                new DropCreateDatabaseIfModelChanges<MqttServiceDbContext>()
            );
        }

        public MqttServiceDbContext(string connectionString) : base(connectionString)
        {
            Database.SetInitializer<MqttServiceDbContext>(
                new DropCreateDatabaseIfModelChanges<MqttServiceDbContext>()
            );
        }

        public DbSet<MicrocontrollerEntity> Microcontrollers { get; set; }
        public DbSet<PowerStripEntity> PowerStrips { get; set; }
    }
}
