﻿using JS.Base.WS.API.Models;
using JS.Base.WS.API.Models.Authorization;
using JS.Base.WS.API.Models.Configuration;
using JS.Base.WS.API.Models.Domain;
using JS.Base.WS.API.Models.Domain.Inventory;
using JS.Base.WS.API.Models.EnterpriseConf;
using JS.Base.WS.API.Models.FileDocument;
using JS.Base.WS.API.Models.Permission;
using JS.Base.WS.API.Models.PersonProfile;
using JS.Base.WS.API.Models.Publicity;
using System.Data.Entity;

namespace JS.Base.WS.API.DBContext
{
    public class MyDBcontext: DbContext
    {

        public MyDBcontext() : base("name=JS.Base")
        {

        }

        //metodo para eliminar la plurarizacion de las entidades
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //}

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<LocatorType> LocatorTypes { get; set; }
        public virtual DbSet<Locator> Locators { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<UserStatus> UserStatus { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Entity> Entities { get; set; }
        public virtual DbSet<EntityAction> EntityActions { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public virtual DbSet<ConfigurationParameter> ConfigurationParameters { get; set; }
        public virtual DbSet<PersonType> PersonTypes { get; set; }
        public virtual DbSet<DocumentType> DocumentTypes { get; set; }


        public virtual DbSet<Currency> Currencies { get; set; }

        //Enterprise
        public virtual DbSet<Enterprise> Enterprises { get; set; }


        //Publicity
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<Novelty> Novelties { get; set; }
        public virtual DbSet<NoveltyType> NoveltyTypes { get; set; }

        //File
        public virtual DbSet<FileDocument> FileDocuments  { get; set; }


        public virtual DbSet<ScheduleHour> ScheduleHours { get; set; }

        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }


        //Domain
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<InventoryDetail> InventoryDetails { get; set; }
        public virtual DbSet<InventoryStatus> InventoryStatuses { get; set; }
        public virtual DbSet<InventoryConfiguration> InventoryConfigurations { get; set; }
        public virtual DbSet<InventorySection> InventorySections { get; set; }
        public virtual DbSet<InventoryTariff> InventoryTariffs { get; set; }




    }
}