﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using RepoWebShop.Models;
using System;

namespace RepoWebShop.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RepoWebShop.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AddressLine1")
                        .HasMaxLength(100);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Country")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("DateOfBirth")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FacebookNameIdentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Gender")
                        .HasMaxLength(50);

                    b.Property<string>("GoogleNameIdentifier");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PhoneNumberDeclared");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("State")
                        .HasMaxLength(50);

                    b.Property<string>("StreetName")
                        .HasMaxLength(100);

                    b.Property<string>("StreetNumber")
                        .HasMaxLength(100);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<DateTime?>("ValidationMailToken");

                    b.Property<string>("ValidationPhoneToken");

                    b.Property<string>("ZipCode")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RepoWebShop.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryName");

                    b.Property<string>("Description");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("RepoWebShop.Models.DeliveryAddress", b =>
                {
                    b.Property<int>("DeliveryAddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Country")
                        .HasMaxLength(50);

                    b.Property<DateTime>("Created");

                    b.Property<decimal>("DeliveryCost");

                    b.Property<string>("DeliveryInstructions")
                        .HasMaxLength(256);

                    b.Property<int>("Distance");

                    b.Property<string>("ShoppingCartId");

                    b.Property<string>("State")
                        .HasMaxLength(50);

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserId");

                    b.Property<string>("ZipCode");

                    b.HasKey("DeliveryAddressId");

                    b.HasIndex("UserId");

                    b.ToTable("DeliveryAddresses");
                });

            modelBuilder.Entity("RepoWebShop.Models.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments")
                        .HasMaxLength(50);

                    b.Property<int>("DurationDays");

                    b.Property<int?>("InstancesLeft");

                    b.Property<bool>("IsActive");

                    b.Property<int?>("Loop");

                    b.Property<int>("Percentage");

                    b.Property<string>("Phrase")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<decimal>("Roof");

                    b.Property<DateTime>("ValidFrom");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("RepoWebShop.Models.Email", b =>
                {
                    b.Property<int>("EmailId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Bcc");

                    b.Property<string>("Body");

                    b.Property<string>("Cc");

                    b.Property<string>("Status");

                    b.Property<string>("Subject");

                    b.Property<string>("To");

                    b.HasKey("EmailId");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("RepoWebShop.Models.GalleryFlickrAlbum", b =>
                {
                    b.Property<int>("GalleryFlickrAlbumId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("FlickrSetId");

                    b.Property<bool>("InGallery");

                    b.HasKey("GalleryFlickrAlbumId");

                    b.ToTable("GalleryFlickrAlbums");
                });

            modelBuilder.Entity("RepoWebShop.Models.OpenHours", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DayId");

                    b.Property<TimeSpan>("Duration");

                    b.Property<TimeSpan>("StartingAt");

                    b.HasKey("Id");

                    b.ToTable("OpenHours");
                });

            modelBuilder.Entity("RepoWebShop.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BookingId");

                    b.Property<bool>("Cancelled");

                    b.Property<string>("CustomerComments");

                    b.Property<int?>("DeliveryAddressId");

                    b.Property<int?>("DiscountId");

                    b.Property<int?>("EmailId");

                    b.Property<bool>("Finished");

                    b.Property<string>("ManagementComments");

                    b.Property<string>("MercadoPagoMail");

                    b.Property<string>("MercadoPagoName");

                    b.Property<string>("MercadoPagoTransaction");

                    b.Property<string>("MercadoPagoUsername");

                    b.Property<string>("OrderHistory");

                    b.Property<DateTime>("OrderPlaced");

                    b.Property<decimal>("OrderTotal");

                    b.Property<bool>("PaymentReceived");

                    b.Property<DateTime?>("Payout");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(25);

                    b.Property<DateTime?>("PickUpTime");

                    b.Property<bool>("PickedUp");

                    b.Property<int>("PreparationTime");

                    b.Property<bool>("Refunded");

                    b.Property<string>("RegistrationId");

                    b.Property<bool>("Returned");

                    b.Property<string>("Status");

                    b.HasKey("OrderId");

                    b.HasIndex("DeliveryAddressId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("EmailId");

                    b.HasIndex("RegistrationId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("RepoWebShop.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("OrderId");

                    b.Property<int>("PieId");

                    b.Property<decimal>("Price");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PieId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("RepoWebShop.Models.PaymentNotice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Area_Code");

                    b.Property<string>("BookingId");

                    b.Property<decimal>("Concept_Amount");

                    b.Property<string>("Currency_Id");

                    b.Property<DateTime?>("Date_Approved");

                    b.Property<DateTime?>("Date_Created");

                    b.Property<string>("Extension");

                    b.Property<decimal?>("Installment_Amount");

                    b.Property<int>("Installments");

                    b.Property<string>("MercadoPagoMail");

                    b.Property<string>("MercadoPagoName");

                    b.Property<string>("MercadoPagoTransaction");

                    b.Property<string>("MercadoPagoUsername");

                    b.Property<string>("Merchant_Order_Id");

                    b.Property<DateTime?>("Money_Release_Date");

                    b.Property<decimal>("Net_Received_Amount");

                    b.Property<string>("Operation_Type");

                    b.Property<decimal>("OrderTotal");

                    b.Property<string>("Order_Id");

                    b.Property<bool>("PaymentReceived");

                    b.Property<string>("Payment_Id");

                    b.Property<string>("Payment_Type");

                    b.Property<DateTime?>("Payout");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("Reason");

                    b.Property<string>("Site_Id");

                    b.Property<string>("Status");

                    b.Property<string>("Status_Detail");

                    b.Property<decimal>("Total_Paid_Amount");

                    b.Property<decimal>("Transaction_Amount");

                    b.Property<int>("Transaction_Order_Id");

                    b.Property<string>("User_Id");

                    b.HasKey("Id");

                    b.ToTable("PaymentNotices");
                });

            modelBuilder.Entity("RepoWebShop.Models.Pie", b =>
                {
                    b.Property<int>("PieId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CategoryId");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("PieDetailId");

                    b.Property<decimal>("Price");

                    b.Property<string>("SizeDescription")
                        .IsRequired();

                    b.Property<decimal?>("StorePrice");

                    b.HasKey("PieId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PieDetailId");

                    b.ToTable("Pies");
                });

            modelBuilder.Entity("RepoWebShop.Models.PieDetail", b =>
                {
                    b.Property<int>("PieDetailId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AllergyInformation");

                    b.Property<int>("CategoryId");

                    b.Property<long>("FlickrAlbumId");

                    b.Property<bool>("InStock");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsPieOfTheWeek");

                    b.Property<string>("LongDescription")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("PreparationTime");

                    b.Property<string>("ShortDescription")
                        .IsRequired();

                    b.HasKey("PieDetailId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PieDetails");
                });

            modelBuilder.Entity("RepoWebShop.Models.ProcessingHours", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DayId");

                    b.Property<TimeSpan>("Duration");

                    b.Property<TimeSpan>("StartingAt");

                    b.HasKey("Id");

                    b.ToTable("ProcessingHours");
                });

            modelBuilder.Entity("RepoWebShop.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<int>("MinOrderAmount");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal?>("OldPrice");

                    b.Property<decimal>("Price");

                    b.Property<string>("Temperature");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RepoWebShop.Models.PublicHoliday", b =>
                {
                    b.Property<int>("PublicHolidayId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int?>("OpenHoursId");

                    b.Property<int?>("ProcessingHoursId");

                    b.HasKey("PublicHolidayId");

                    b.HasIndex("OpenHoursId");

                    b.HasIndex("ProcessingHoursId");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("RepoWebShop.Models.ShoppingCartComment", b =>
                {
                    b.Property<int>("ShoppingCartCommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comments");

                    b.Property<DateTime>("Created");

                    b.Property<string>("ShoppingCartId");

                    b.HasKey("ShoppingCartCommentId");

                    b.ToTable("ShoppingCartComments");
                });

            modelBuilder.Entity("RepoWebShop.Models.ShoppingCartData", b =>
                {
                    b.Property<int>("ShoppingCartDataId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BookingId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<string>("MercadoPagoPreferenceId");

                    b.HasKey("ShoppingCartDataId");

                    b.ToTable("ShoppingCartData");
                });

            modelBuilder.Entity("RepoWebShop.Models.ShoppingCartDiscount", b =>
                {
                    b.Property<int>("ShoppingCartDiscountId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DiscountId");

                    b.Property<string>("ShoppingCartId");

                    b.HasKey("ShoppingCartDiscountId");

                    b.HasIndex("DiscountId");

                    b.ToTable("ShoppingCartDiscount");
                });

            modelBuilder.Entity("RepoWebShop.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ShoppingCartItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int?>("PieId");

                    b.Property<string>("ShoppingCartId");

                    b.HasKey("ShoppingCartItemId");

                    b.HasIndex("PieId");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("RepoWebShop.Models.Vacation", b =>
                {
                    b.Property<int>("VacationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("VacationId");

                    b.ToTable("Vacations");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RepoWebShop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RepoWebShop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RepoWebShop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RepoWebShop.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RepoWebShop.Models.DeliveryAddress", b =>
                {
                    b.HasOne("RepoWebShop.Models.ApplicationUser", "User")
                        .WithMany("DeliveryAddresses")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RepoWebShop.Models.Order", b =>
                {
                    b.HasOne("RepoWebShop.Models.DeliveryAddress", "DeliveryAddress")
                        .WithMany()
                        .HasForeignKey("DeliveryAddressId");

                    b.HasOne("RepoWebShop.Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId");

                    b.HasOne("RepoWebShop.Models.Email", "Email")
                        .WithMany()
                        .HasForeignKey("EmailId");

                    b.HasOne("RepoWebShop.Models.ApplicationUser", "Registration")
                        .WithMany()
                        .HasForeignKey("RegistrationId");
                });

            modelBuilder.Entity("RepoWebShop.Models.OrderDetail", b =>
                {
                    b.HasOne("RepoWebShop.Models.Order", "Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RepoWebShop.Models.Pie", "Pie")
                        .WithMany()
                        .HasForeignKey("PieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RepoWebShop.Models.Pie", b =>
                {
                    b.HasOne("RepoWebShop.Models.Category")
                        .WithMany("Pies")
                        .HasForeignKey("CategoryId");

                    b.HasOne("RepoWebShop.Models.PieDetail", "PieDetail")
                        .WithMany()
                        .HasForeignKey("PieDetailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RepoWebShop.Models.PieDetail", b =>
                {
                    b.HasOne("RepoWebShop.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RepoWebShop.Models.PublicHoliday", b =>
                {
                    b.HasOne("RepoWebShop.Models.OpenHours", "OpenHours")
                        .WithMany()
                        .HasForeignKey("OpenHoursId");

                    b.HasOne("RepoWebShop.Models.ProcessingHours", "ProcessingHours")
                        .WithMany()
                        .HasForeignKey("ProcessingHoursId");
                });

            modelBuilder.Entity("RepoWebShop.Models.ShoppingCartDiscount", b =>
                {
                    b.HasOne("RepoWebShop.Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId");
                });

            modelBuilder.Entity("RepoWebShop.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("RepoWebShop.Models.Pie", "Pie")
                        .WithMany()
                        .HasForeignKey("PieId");
                });
#pragma warning restore 612, 618
        }
    }
}
