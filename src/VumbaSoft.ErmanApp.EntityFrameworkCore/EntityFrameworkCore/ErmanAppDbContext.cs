using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using VumbaSoft.ErmanApp.Authors;
using VumbaSoft.ErmanApp.Books;
using VumbaSoft.ErmanApp.Demographics.Continents;
using VumbaSoft.ErmanApp.Demographics.Subcontinents;
using VumbaSoft.ErmanApp.Demographics.Regions;
using VumbaSoft.ErmanApp.Demographics.Countries;
using VumbaSoft.ErmanApp.Demographics.StateProvinces;
using VumbaSoft.ErmanApp.Demographics.DistrictCities;
using VumbaSoft.ErmanApp.Demographics.Localities;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace VumbaSoft.ErmanApp.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ErmanAppDbContext :
    AbpDbContext<ErmanAppDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<Author> Authors { get; set; }

    public DbSet<Book> Books { get; set; }

    public DbSet<Continent> Continents { get; set; }

    public DbSet<Subcontinent> Subcontinents { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<StateProvince> StateProvinces { get; set; }

    public DbSet<DistrictCity> DistrictCities { get; set; }

    public DbSet<Locality> Localities { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public ErmanAppDbContext(DbContextOptions<ErmanAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();

        builder.Entity<Author>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Authors",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(AuthorConsts.MaxNameLength);
            b.Property(x => x.ShortBio).HasMaxLength(AuthorConsts.MaxShortBioLength);
        });

        builder.Entity<Book>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Books",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.HasOne<Author>().WithMany().HasForeignKey(x => x.AuthorId).IsRequired();
        });

        /* Configure your own tables/entities inside here */

        builder.Entity<Continent>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Continents",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(ContinentConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(ContinentConsts.MaxRemarksLength);
            b.HasIndex(x => x.Name);
        });

        builder.Entity<Subcontinent>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Subcontinents",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(SubcontinentConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(SubcontinentConsts.MaxRemarksLength);
            b.HasIndex(x => x.Name);
            b.HasOne<Continent>().WithMany().HasForeignKey(x => x.ContinentId).IsRequired();
        });

        builder.Entity<Region>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Regions",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(RegionConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(RegionConsts.MaxRemarksLength);
            b.HasIndex(x => x.Name);
            b.HasOne<Subcontinent>().WithMany().HasForeignKey(x => x.SubcontinentId).IsRequired();
        });

        builder.Entity<Country>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Countries",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(CountryConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(CountryConsts.MaxRemarksLength);
            b.Property(x => x.FormalName).HasMaxLength(CountryConsts.MaxFormalNameLength);
            b.Property(x => x.NativeName).HasMaxLength(CountryConsts.MaxNativeNameLength);
            b.Property(x => x.ISO3).HasMaxLength(CountryConsts.MaxIso3Length);
            b.Property(x => x.ISO2).HasMaxLength(CountryConsts.MaxIso2Length);
            b.Property(x => x.CCN3).HasMaxLength(CountryConsts.MaxCcn3Length);
            b.Property(x => x.PhoneCode).HasMaxLength(CountryConsts.MaxPhoneCodeLength);
            b.Property(x => x.Capital).HasMaxLength(CountryConsts.MaxCapitalLength);
            b.Property(x => x.Currency).HasMaxLength(CountryConsts.MaxCurrencyLength);
            b.Property(x => x.Emoji).HasMaxLength(CountryConsts.MaxEmojiLength);
            b.Property(x => x.EmojiU).HasMaxLength(CountryConsts.MaxEmojiULength);
            b.HasIndex(x => x.Name);
            b.HasOne<Region>().WithMany().HasForeignKey(x => x.RegionId).IsRequired();
        });

        builder.Entity<StateProvince>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "StateProvinces",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(StateProvinceConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(StateProvinceConsts.MaxRemarksLength);
            b.Property(x => x.RegionCode).HasMaxLength(StateProvinceConsts.MaxRegionCodeLength);
            b.Property(x => x.StateProvinceCode).HasMaxLength(StateProvinceConsts.MaxStateProvinceCodeLength);
            b.HasIndex(x => x.Name);
            b.HasOne<Country>().WithMany().HasForeignKey(x => x.CountryId).IsRequired();
        });

        builder.Entity<DistrictCity>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "DistrictCities",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(DistrictCityConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(DistrictCityConsts.MaxRemarksLength);
            b.Property(x => x.CountryCode).HasMaxLength(DistrictCityConsts.MaxCountryCodeLength);
            b.Property(x => x.Latitude).HasPrecision(DistrictCityConsts.LatitudePrecision, DistrictCityConsts.LatLngScale);
            b.Property(x => x.Longitude).HasPrecision(DistrictCityConsts.LongitudePrecision, DistrictCityConsts.LatLngScale);
            b.HasIndex(x => x.Name);
            b.HasOne<StateProvince>().WithMany().HasForeignKey(x => x.StateProvinceId).IsRequired();
        });

        builder.Entity<Locality>(b =>
        {
            b.ToTable(ErmanAppConsts.DbTablePrefix + "Localities",
                ErmanAppConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(LocalityConsts.MaxNameLength);
            b.Property(x => x.Remarks).HasMaxLength(LocalityConsts.MaxRemarksLength);
            b.Property(x => x.DistrictCityCode).HasMaxLength(LocalityConsts.MaxDistrictCityCodeLength);
            b.Property(x => x.LocalityCode).HasMaxLength(LocalityConsts.MaxLocalityCodeLength);
            b.Property(x => x.Latitude).HasPrecision(LocalityConsts.LatitudePrecision, LocalityConsts.LatLngScale);
            b.Property(x => x.Longitude).HasPrecision(LocalityConsts.LongitudePrecision, LocalityConsts.LatLngScale);
            b.HasIndex(x => x.Name);
            b.HasOne<DistrictCity>().WithMany().HasForeignKey(x => x.DistrictCityId).IsRequired();
        });

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ErmanAppConsts.DbTablePrefix + "YourEntities", ErmanAppConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
