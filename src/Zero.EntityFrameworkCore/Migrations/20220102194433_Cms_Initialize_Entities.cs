using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Zero.Migrations
{
    public partial class Cms_Initialize_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cms_Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCount = table.Column<int>(type: "int", nullable: false),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CategoryCode = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    TitleDefault = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionDefault = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeywordDefault = table.Column<bool>(type: "bit", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorDefault = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Category_Cms_Category_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Cms_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_ImageBlock_Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_ImageBlock_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Menu_Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Menu_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Page_Theme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Page_Theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Widget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsBundleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsScript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsPlain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CssBundleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CssScript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CssPlain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    ContentCount = table.Column<int>(type: "int", nullable: false),
                    AsyncLoad = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Widget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommentCount = table.Column<int>(type: "int", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    TitleDefault = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionDefault = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeywordDefault = table.Column<bool>(type: "bit", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorDefault = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Post_Cms_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Cms_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_ImageBlock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    ImageBlockGroupId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageMobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_ImageBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_ImageBlock_Cms_ImageBlock_Group_ImageBlockGroupId",
                        column: x => x.ImageBlockGroupId,
                        principalTable: "Cms_ImageBlock_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    MenuGroupId = table.Column<int>(type: "int", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Menu_Cms_Menu_Group_MenuGroupId",
                        column: x => x.MenuGroupId,
                        principalTable: "Cms_Menu_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Page_Layout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PageThemeId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Page_Layout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Layout_Cms_Page_Theme_PageThemeId",
                        column: x => x.PageThemeId,
                        principalTable: "Cms_Page_Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Widget_PageTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WidgetId = table.Column<int>(type: "int", nullable: false),
                    PageThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Widget_PageTheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Widget_PageTheme_Cms_Page_Theme_PageThemeId",
                        column: x => x.PageThemeId,
                        principalTable: "Cms_Page_Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cms_Widget_PageTheme_Cms_Widget_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "Cms_Widget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Post_Category_Detail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Post_Category_Detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Post_Category_Detail_Cms_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Cms_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Cms_Post_Category_Detail_Cms_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Cms_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Post_Tag_Detail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Post_Tag_Detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Post_Tag_Detail_Cms_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Cms_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Cms_Post_Tag_Detail_Cms_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Cms_Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Page",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    PageLayoutId = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsHomePage = table.Column<bool>(type: "bit", nullable: false),
                    Publish = table.Column<bool>(type: "bit", nullable: false),
                    TitleDefault = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionDefault = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeywordDefault = table.Column<bool>(type: "bit", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorDefault = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numbering = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Cms_Page_Layout_PageLayoutId",
                        column: x => x.PageLayoutId,
                        principalTable: "Cms_Page_Layout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Page_Layout_Block",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageLayoutId = table.Column<int>(type: "int", nullable: false),
                    ColumnCount = table.Column<int>(type: "int", nullable: false),
                    WrapInRow = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ParentLayoutBlockId = table.Column<int>(type: "int", nullable: true),
                    ParentColumnUniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col1Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col1UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col1Class = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col2Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col2UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col2Class = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col3Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col3UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col3Class = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col4Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col4UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Col4Class = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Page_Layout_Block", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Layout_Block_Cms_Page_Layout_Block_ParentLayoutBlockId",
                        column: x => x.ParentLayoutBlockId,
                        principalTable: "Cms_Page_Layout_Block",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Layout_Block_Cms_Page_Layout_PageLayoutId",
                        column: x => x.PageLayoutId,
                        principalTable: "Cms_Page_Layout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Page_Widget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageBlockColumnId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    WidgetId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Page_Widget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Widget_Cms_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Cms_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Widget_Cms_Widget_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "Cms_Widget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Page_Widget_Detail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageWidgetId = table.Column<int>(type: "int", nullable: false),
                    ImageBlockGroupId = table.Column<int>(type: "int", nullable: true),
                    MenuGroupId = table.Column<int>(type: "int", nullable: true),
                    CustomContent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Page_Widget_Detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Widget_Detail_Cms_ImageBlock_Group_ImageBlockGroupId",
                        column: x => x.ImageBlockGroupId,
                        principalTable: "Cms_ImageBlock_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Widget_Detail_Cms_Menu_Group_MenuGroupId",
                        column: x => x.MenuGroupId,
                        principalTable: "Cms_Menu_Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cms_Page_Widget_Detail_Cms_Page_Widget_PageWidgetId",
                        column: x => x.PageWidgetId,
                        principalTable: "Cms_Page_Widget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Category_ParentId",
                table: "Cms_Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_ImageBlock_ImageBlockGroupId",
                table: "Cms_ImageBlock",
                column: "ImageBlockGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Menu_MenuGroupId",
                table: "Cms_Menu",
                column: "MenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_PageLayoutId",
                table: "Cms_Page",
                column: "PageLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Layout_PageThemeId",
                table: "Cms_Page_Layout",
                column: "PageThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Layout_Block_PageLayoutId",
                table: "Cms_Page_Layout_Block",
                column: "PageLayoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Layout_Block_ParentLayoutBlockId",
                table: "Cms_Page_Layout_Block",
                column: "ParentLayoutBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Widget_PageId",
                table: "Cms_Page_Widget",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Widget_WidgetId",
                table: "Cms_Page_Widget",
                column: "WidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Widget_Detail_ImageBlockGroupId",
                table: "Cms_Page_Widget_Detail",
                column: "ImageBlockGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Widget_Detail_MenuGroupId",
                table: "Cms_Page_Widget_Detail",
                column: "MenuGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Page_Widget_Detail_PageWidgetId",
                table: "Cms_Page_Widget_Detail",
                column: "PageWidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Post_CategoryId",
                table: "Cms_Post",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Post_Category_Detail_CategoryId",
                table: "Cms_Post_Category_Detail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Post_Category_Detail_PostId",
                table: "Cms_Post_Category_Detail",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Post_Tag_Detail_PostId",
                table: "Cms_Post_Tag_Detail",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Post_Tag_Detail_TagId",
                table: "Cms_Post_Tag_Detail",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Widget_PageTheme_PageThemeId",
                table: "Cms_Widget_PageTheme",
                column: "PageThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Widget_PageTheme_WidgetId",
                table: "Cms_Widget_PageTheme",
                column: "WidgetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cms_ImageBlock");

            migrationBuilder.DropTable(
                name: "Cms_Menu");

            migrationBuilder.DropTable(
                name: "Cms_Page_Layout_Block");

            migrationBuilder.DropTable(
                name: "Cms_Page_Widget_Detail");

            migrationBuilder.DropTable(
                name: "Cms_Post_Category_Detail");

            migrationBuilder.DropTable(
                name: "Cms_Post_Tag_Detail");

            migrationBuilder.DropTable(
                name: "Cms_Widget_PageTheme");

            migrationBuilder.DropTable(
                name: "Cms_ImageBlock_Group");

            migrationBuilder.DropTable(
                name: "Cms_Menu_Group");

            migrationBuilder.DropTable(
                name: "Cms_Page_Widget");

            migrationBuilder.DropTable(
                name: "Cms_Post");

            migrationBuilder.DropTable(
                name: "Cms_Tags");

            migrationBuilder.DropTable(
                name: "Cms_Page");

            migrationBuilder.DropTable(
                name: "Cms_Widget");

            migrationBuilder.DropTable(
                name: "Cms_Category");

            migrationBuilder.DropTable(
                name: "Cms_Page_Layout");

            migrationBuilder.DropTable(
                name: "Cms_Page_Theme");
        }
    }
}
