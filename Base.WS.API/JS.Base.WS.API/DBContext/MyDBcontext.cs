﻿using JS.Base.WS.API.Models;
using JS.Base.WS.API.Models.Authorization;
using JS.Base.WS.API.Models.Configuration;
using JS.Base.WS.API.Models.Domain;
using JS.Base.WS.API.Models.Domain.PurchaseTransaction;
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



        //Domain
        public virtual DbSet<CompanyCategory> CompanyCategories { get; set; }
        public virtual DbSet<CompanyRegister> CompanyRegisters { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
        public virtual DbSet<ScheduleHour> ScheduleHours { get; set; }

        public virtual DbSet<Market> Markets { get; set; }
        public virtual DbSet<MarketType> MarketTypes { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<ArticleSubCategory> ArticleSubCategories { get; set; }
        public virtual DbSet<ArticleCondition> ArticleConditions { get; set; }
        public virtual DbSet<MarketImgDetail> MarketImgDetails { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }


        public virtual DbSet<PurchaseTransactionStatus> PurchaseTransactionStatus { get; set; }
        public virtual DbSet<PurchaseTransactionType> PurchaseTransactionTypes { get; set; }
        public virtual DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }
        public virtual DbSet<PurchaseTransactionDetail> PurchaseTransactionDetails { get; set; }

    }
}