﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Migrations
{
    public partial class init_GarmentLocalDebiturBalances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GarmentLocalDebiturBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: false),
                    CreatedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 255, nullable: false),
                    LastModifiedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(maxLength: 255, nullable: false),
                    DeletedAgent = table.Column<string>(maxLength: 255, nullable: false),
                    BalanceDate = table.Column<DateTimeOffset>(nullable: false),
                    BuyerId = table.Column<int>(nullable: false),
                    BuyerCode = table.Column<string>(maxLength: 10, nullable: true),
                    BuyerName = table.Column<string>(maxLength: 255, nullable: true),
                    BalanceAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GarmentLocalDebiturBalances", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GarmentLocalDebiturBalances");

        }
    }
}
