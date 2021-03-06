﻿using Microsoft.EntityFrameworkCore;
using ParkingLotModelLayer;


namespace ParkingLotRepositoryLayer
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options)
        {

        }

        public DbSet<UserModel> UserTable { get; set; }

        public DbSet<ParkingModel> ParkingTable { get; set; }
        
        public DbSet<DriverTypeModel> DriverTypeTable { get; set; }
        
        public DbSet<VehicalTypeModel> VehicalTypeTable { get; set; }
        
        public DbSet<ParkingTypeModel> ParkingTypeTable { get; set; }

    }
}
