using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaiAdmin.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attach",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    CreateUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    Ext = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    EntityName = table.Column<string>(nullable: true),
                    EntityGuid = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    DeleteUserId = table.Column<int>(nullable: true),
                    DeleteTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attach", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysApi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    IsCommon = table.Column<bool>(nullable: false),
                    PermissionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysApi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysConfig",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysConfig", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "SysMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    ParentIds = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    IsPage = table.Column<bool>(nullable: false),
                    CanMultipleOpen = table.Column<bool>(nullable: false),
                    HasRead = table.Column<bool>(nullable: false),
                    HasWrite = table.Column<bool>(nullable: false),
                    HasReview = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysMenuApi",
                columns: table => new
                {
                    ApiId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuApi", x => new { x.ApiId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_SysMenuApi_SysApi_ApiId",
                        column: x => x.ApiId,
                        principalTable: "SysApi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysMenuApi_SysMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "SysMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysRoleMenu",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false),
                    CanRead = table.Column<bool>(nullable: false),
                    CanWrite = table.Column<bool>(nullable: false),
                    CanReview = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysRoleMenu", x => new { x.RoleId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_SysRoleMenu_SysMenu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "SysMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysRoleMenu_SysRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SysRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysUser",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoginName = table.Column<string>(nullable: true),
                    LoginPassword = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysUser_SysRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SysRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToken = table.Column<string>(nullable: true),
                    AccessExpire = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Ip = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    RefreshExpire = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysToken_SysUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SysUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysUserLoginLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAddress = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysUserLoginLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysUserLoginLog_SysUser_UserId",
                        column: x => x.UserId,
                        principalTable: "SysUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "SysApi",
                columns: new[] { "Id", "IsCommon", "Name", "Path", "PermissionType" },
                values: new object[,]
                {
                    { 1, false, "获取接口分页列表", "/Sys/Api/GetPageList", 0 },
                    { 2, false, "删除接口", "/Sys/Api/DeleteById", 1 },
                    { 3, false, "保存接口", "/Sys/Api/Save", 1 },
                    { 4, true, "获取接口公共分页列表", "/Sys/Api/GetCommonPageList", 0 },
                    { 5, true, "获取角色公共选项列表", "/Sys/Role/GetCommonOptionList", 0 },
                    { 6, false, "获取角色分页列表", "/Sys/Role/GetPageList", 0 }
                });

            migrationBuilder.InsertData(
                table: "SysConfig",
                columns: new[] { "Key", "Name", "Type", "Value" },
                values: new object[,]
                {
                    { "PAGE_TABS", "使用多页面标签", "BOOL", "ON" },
                    { "LOGIN_VCODE", "登录需要验证码", "BOOL", "OFF" },
                    { "SHOW_MENU_ICON", "是否显示菜单图标", "BOOL", "OFF" },
                    { "MENU_DEFAULT_ICON", "菜单默认图标", "STRING", "el-icon-menu" },
                    { "LIST_DEFAULT_PAGE_SIZE", "列表默认页容量", "NUMBER", "10" },
                    { "IS_REPAIR", "网站维护", "BOOL", "OFF" },
                    { "VERSION", "版本号", "STRING", "20200414001" },
                    { "MENU_BAR_TITLE", "菜单栏标题", "STRING", "LessAdmin" },
                    { "SYSTEM_TITLE", "系统标题", "STRING", "LessAdmin快速开发框架" },
                    { "LAYOUT", "后台布局", "STRING", "leftRight" }
                });

            migrationBuilder.InsertData(
                table: "SysMenu",
                columns: new[] { "Id", "CanMultipleOpen", "HasRead", "HasReview", "HasWrite", "Icon", "IsPage", "Name", "Order", "ParentId", "ParentIds", "Path" },
                values: new object[,]
                {
                    { 7, false, true, false, true, null, false, "Token管理", 6, 1, "1", "token" },
                    { 1, false, false, false, false, null, false, "系统管理", 99, null, null, "sys" },
                    { 2, false, true, false, true, null, false, "用户管理", 1, 1, "1", "user" },
                    { 3, false, true, false, true, null, false, "角色管理", 2, 1, "1", "role" },
                    { 4, false, true, false, true, null, false, "菜单管理", 3, 1, "1", "menu" },
                    { 5, false, true, false, true, null, false, "接口管理", 4, 1, "1", "api" },
                    { 6, false, true, false, true, null, false, "配置管理", 5, 1, "1", "config" }
                });

            migrationBuilder.InsertData(
                table: "SysRole",
                columns: new[] { "Id", "CreateTime", "Name", "Remarks", "UpdateTime" },
                values: new object[] { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "超级角色", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "SysUser",
                columns: new[] { "Id", "CreateTime", "IsDisabled", "LoginName", "LoginPassword", "RoleId", "UpdateTime" },
                values: new object[] { -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "superadmin", "admin", -1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_SysApi_Name",
                table: "SysApi",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SysApi_Path",
                table: "SysApi",
                column: "Path",
                unique: true,
                filter: "[Path] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuApi_MenuId",
                table: "SysMenuApi",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRole_Name",
                table: "SysRole",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SysRoleMenu_MenuId",
                table: "SysRoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SysToken_UserId",
                table: "SysToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SysUser_LoginName",
                table: "SysUser",
                column: "LoginName",
                unique: true,
                filter: "[LoginName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SysUser_RoleId",
                table: "SysUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysUserLoginLog_UserId",
                table: "SysUserLoginLog",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attach");

            migrationBuilder.DropTable(
                name: "SysConfig");

            migrationBuilder.DropTable(
                name: "SysMenuApi");

            migrationBuilder.DropTable(
                name: "SysRoleMenu");

            migrationBuilder.DropTable(
                name: "SysToken");

            migrationBuilder.DropTable(
                name: "SysUserLoginLog");

            migrationBuilder.DropTable(
                name: "SysApi");

            migrationBuilder.DropTable(
                name: "SysMenu");

            migrationBuilder.DropTable(
                name: "SysUser");

            migrationBuilder.DropTable(
                name: "SysRole");
        }
    }
}
