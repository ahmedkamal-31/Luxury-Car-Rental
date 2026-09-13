using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxuryCarRental.Migrations
{
    public partial class AddCarReviewsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
                BEGIN
                    CREATE TABLE [Reviews] (
                        [Id] int NOT NULL IDENTITY,
                        [CarId] int NOT NULL,
                        [UserName] nvarchar(max) NULL,
                        [Rating] int NOT NULL,
                        [Comment] nvarchar(max) NOT NULL,
                        [CreatedAt] datetime2 NOT NULL,
                        CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id])
                    );
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews') DROP TABLE [Reviews];");
        }
    }
}
