/*
MySQL Backup
Database: cai_admin
Backup Time: 2024-05-10 11:46:28
*/

SET FOREIGN_KEY_CHECKS=0;
DROP TABLE IF EXISTS `cai_admin`.`attach`;
DROP TABLE IF EXISTS `cai_admin`.`sys_api`;
DROP TABLE IF EXISTS `cai_admin`.`sys_config`;
DROP TABLE IF EXISTS `cai_admin`.`sys_menu`;
DROP TABLE IF EXISTS `cai_admin`.`sys_menu_api`;
DROP TABLE IF EXISTS `cai_admin`.`sys_menu_permission`;
DROP TABLE IF EXISTS `cai_admin`.`sys_menu_permission_api`;
DROP TABLE IF EXISTS `cai_admin`.`sys_role`;
DROP TABLE IF EXISTS `cai_admin`.`sys_role_menu`;
DROP TABLE IF EXISTS `cai_admin`.`sys_role_menu_permission`;
DROP TABLE IF EXISTS `cai_admin`.`sys_token`;
DROP TABLE IF EXISTS `cai_admin`.`sys_user`;
DROP TABLE IF EXISTS `cai_admin`.`sys_user_login_log`;
CREATE TABLE `attach` (
  `id` int(11) NOT NULL,
  `create_time` datetime DEFAULT NULL,
  `create_user_id` int(11) DEFAULT NULL,
  `name` varchar(500) DEFAULT NULL,
  `size` int(11) DEFAULT NULL,
  `ext` varchar(50) DEFAULT NULL,
  `path` varchar(500) DEFAULT NULL,
  `entity_name` varchar(500) DEFAULT NULL,
  `entity_guid` varchar(36) DEFAULT NULL,
  `type` varchar(100) DEFAULT NULL,
  `is_public` tinyint(1) DEFAULT NULL,
  `delete_user_id` int(11) DEFAULT NULL,
  `delete_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_api` (
  `id` int(11) DEFAULT NULL,
  `name` varchar(450) DEFAULT NULL,
  `path` varchar(450) DEFAULT NULL,
  `is_common` tinyint(1) DEFAULT NULL,
  `permission_type` int(11) DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_config` (
  `key` varchar(450) DEFAULT NULL,
  `value` longtext,
  `type` longtext,
  `name` longtext
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_menu` (
  `id` int(11) NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  `parent_id` int(11) DEFAULT NULL,
  `path` varchar(200) DEFAULT NULL,
  `icon` varchar(50) DEFAULT NULL,
  `chain_ids` varchar(500) DEFAULT NULL,
  `order` int(11) DEFAULT NULL,
  `is_page` tinyint(1) DEFAULT NULL,
  `can_multiple_open` tinyint(1) DEFAULT NULL,
  `has_read` tinyint(1) DEFAULT NULL,
  `has_write` tinyint(1) DEFAULT NULL,
  `has_review` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_menu_api` (
  `api_id` int(11) NOT NULL,
  `menu_id` int(11) NOT NULL,
  PRIMARY KEY (`api_id`,`menu_id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_menu_permission` (
  `code` varchar(100) NOT NULL,
  `menu_id` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `order` int(11) DEFAULT NULL,
  PRIMARY KEY (`code`,`menu_id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_menu_permission_api` (
  `menu_id` int(11) NOT NULL,
  `permission_code` varchar(100) NOT NULL,
  `api_id` int(11) NOT NULL,
  PRIMARY KEY (`menu_id`,`permission_code`,`api_id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_role` (
  `id` int(11) NOT NULL,
  `name` varchar(450) DEFAULT NULL,
  `remarks` varchar(0) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_role_menu` (
  `role_id` int(11) NOT NULL,
  `menu_id` int(11) NOT NULL,
  `can_read` tinyint(1) DEFAULT NULL,
  `can_write` tinyint(1) DEFAULT NULL,
  `can_review` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`role_id`,`menu_id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_role_menu_permission` (
  `role_id` int(11) NOT NULL,
  `menu_id` int(11) NOT NULL,
  `permission_code` varchar(100) NOT NULL,
  PRIMARY KEY (`role_id`,`menu_id`,`permission_code`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_token` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `access_token` longtext,
  `access_expire` datetime DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `ip` varchar(100) DEFAULT NULL,
  `refresh_token` longtext,
  `is_disabled` tinyint(1) DEFAULT NULL,
  `refresh_expire` datetime DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_user` (
  `id` int(11) NOT NULL,
  `login_name` varchar(450) DEFAULT NULL,
  `login_password` varchar(500) DEFAULT NULL,
  `role_id` int(11) DEFAULT NULL,
  `is_disabled` tinyint(1) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  `update_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4;
CREATE TABLE `sys_user_login_log` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `ip_address` varchar(100) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
BEGIN;
LOCK TABLES `cai_admin`.`attach` WRITE;
DELETE FROM `cai_admin`.`attach`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_api` WRITE;
DELETE FROM `cai_admin`.`sys_api`;
INSERT INTO `cai_admin`.`sys_api` (`id`,`name`,`path`,`is_common`,`permission_type`) VALUES (1, '获取接口分页列表', '/Sys/Api/GetPageList', 0, 0),(2, '删除接口', '/Sys/Api/DeleteById', 0, 1),(3, '保存接口', '/Sys/Api/Save', 0, 1),(4, '获取接口公共分页列表', '/Sys/Api/GetCommonPageList', 1, 0),(5, '获取角色公共选项列表', '/Sys/Role/GetCommonOptionList', 1, 0),(6, '获取角色分页列表', '/Sys/Role/GetPageList', 0, 0),(7, '获取发票数据分页列表', '/Fam/Invoice/GetPageList', 0, 0),(8, '更新发票状态', '/Fam/Invoice/UpdateInvoiceFromNuoNuo', 0, 1),(9, '获取客户分页列表数据', '/Crm/Customer/GetPageList', 0, 0),(10, '同步客户到ES', '/Crm/Customer/SyncCustomersToEs', 0, 1),(11, '获取客户详情', '/Crm/Customer/GetDetail', 0, 0);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_config` WRITE;
DELETE FROM `cai_admin`.`sys_config`;
INSERT INTO `cai_admin`.`sys_config` (`key`,`value`,`type`,`name`) VALUES ('IS_REPAIR', 'OFF', 'BOOL', NULL),('LAYOUT', 'moduleLeftRight', 'STRING', NULL),('LIST_DEFAULT_PAGE_SIZE', '10', 'NUMBER', NULL),('LOGIN_VCODE', 'OFF', 'BOOL', NULL),('MENU_BAR_TITLE', '数据运维', 'STRING', NULL),('MENU_DEFAULT_ICON', 'el-icon-menu', 'STRING', NULL),('PAGE_TABS', 'ON', 'BOOL', NULL),('SHOW_MENU_ICON', 'OFF', 'BOOL', NULL),('SYSTEM_TITLE', '数据运维平台', 'STRING', NULL),('VERSION', '20200414001', 'STRING', NULL);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_menu` WRITE;
DELETE FROM `cai_admin`.`sys_menu`;
INSERT INTO `cai_admin`.`sys_menu` (`id`,`name`,`parent_id`,`path`,`icon`,`chain_ids`,`order`,`is_page`,`can_multiple_open`,`has_read`,`has_write`,`has_review`) VALUES (1, '系统管理', NULL, 'sys', NULL, '1', 99, 0, 0, 0, 0, 0),(2, '用户管理', 1, 'user', NULL, '1,2', 1, 0, 0, 1, 1, 0),(3, '角色管理', 1, 'role', NULL, '1,3', 2, 0, 0, 1, 1, 0),(4, '菜单管理', 1, 'menu', NULL, '1,4', 3, 0, 0, 1, 1, 0),(5, '接口管理', 1, 'api', NULL, '1,5', 4, 0, 0, 1, 1, 0),(6, '配置管理', 1, 'config', NULL, '1,6', 5, 0, 0, 1, 1, 0),(7, 'Token管理', 1, 'token', NULL, '1,7', 6, 0, 0, 1, 1, 0),(8, '客户数据', 2021, 'customer', '', '2021,8', 1, 0, 0, 1, 1, 0),(9, '发票数据', 2019, 'invoice', '', '2019,9', 2, 0, 0, 1, 1, 0),(10, '密码查询', NULL, 'passwordQuery', '', '10', 88, 0, 0, 1, 0, 0),(11, '旧ICP客户抬头数据', 2021, 'customerTitle', '', '2021,11', 4, 0, 0, 1, 1, 0),(12, '重复客户列表', 2021, 'repeatCustomer', '', '2021,12', 99, 0, 0, 0, 0, 0),(1012, '客户抬头数据', 2021, 'customerTitleNew', '', '2021,1012', 3, 0, 0, 1, 1, 0),(1013, '账单数据', 2019, 'bill', '', '2019,1013', 99, 0, 0, 1, 1, 0),(1014, '核销单数据', 2019, 'check', '', '2019,1014', 99, 0, 0, 1, 1, 0),(1015, '用户管理', 2022, 'user', '', '2022,1015', 99, 0, 0, 1, 1, 0),(1016, 'FCM', NULL, 'fcm', '', '1016', 30, 0, 0, 0, 0, 0),(1017, '运单数据', 1016, 'shipment', '', '1016,1017', 99, 0, 0, 0, 0, 0),(1018, '业务数据', 1016, 'shipmentService', '', '1016,1018', 99, 0, 0, 0, 0, 0),(2016, 'WF', NULL, 'wf', '', '2016', 40, 0, 0, 0, 0, 0),(2017, '工作流数据', 2016, 'workflow', '', '2016,2017', 99, 0, 0, 0, 0, 0),(2019, 'FAM', NULL, 'fam', NULL, '2019', 20, 0, 0, 0, 0, 0),(2020, '放单数据', 2019, 'releaseOrder', NULL, '2019,2020', 99, 0, 0, 0, 0, 0),(2021, 'CRM', NULL, 'crm', '', '2021', 50, 0, 0, 0, 0, 0),(2022, 'SSO', NULL, 'sso', NULL, '2022', 60, 0, 0, 0, 0, 0),(3019, 'Platform', NULL, 'platform', '', '3019,', 70, 0, 0, 0, 0, 0),(3020, '组织机构', 3019, 'organizationUnit', NULL, '3019,3020,', 99, 0, 0, 0, 0, 0),(4019, 'PUB', NULL, 'pub', NULL, '4019,', 80, 0, 0, 0, 0, 0),(4020, '会计科目', 4019, 'glCode', NULL, '4019,4020,', 99, 0, 0, 0, 0, 0),(4021, '公司配置', 4019, 'configure', NULL, '4019,4021,', 99, 0, 0, 0, 0, 0),(4022, '公海池客户监控', 2021, 'seaPoolStatistics', NULL, '20214022,', 99, 0, 0, 0, 0, 0);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_menu_api` WRITE;
DELETE FROM `cai_admin`.`sys_menu_api`;
INSERT INTO `cai_admin`.`sys_menu_api` (`api_id`,`menu_id`) VALUES (7, 9),(8, 9),(9, 8),(10, 8),(11, 8);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_menu_permission` WRITE;
DELETE FROM `cai_admin`.`sys_menu_permission`;
INSERT INTO `cai_admin`.`sys_menu_permission` (`code`,`menu_id`,`name`,`order`) VALUES ('READ', 2, '读', 99);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_menu_permission_api` WRITE;
DELETE FROM `cai_admin`.`sys_menu_permission_api`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_role` WRITE;
DELETE FROM `cai_admin`.`sys_role`;
INSERT INTO `cai_admin`.`sys_role` (`id`,`name`,`remarks`,`create_time`,`update_time`) VALUES (-1, '超级角色', NULL, '1900-01-01 00:00:00', '1900-01-01 00:00:00'),(1, '运维人员', '', '1900-01-20 18:46:35', '2023-04-04 14:29:47');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_role_menu` WRITE;
DELETE FROM `cai_admin`.`sys_role_menu`;
INSERT INTO `cai_admin`.`sys_role_menu` (`role_id`,`menu_id`,`can_read`,`can_write`,`can_review`) VALUES (1, 8, 1, 1, 0),(1, 9, 1, 1, 0),(1, 10, 1, 0, 0);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_role_menu_permission` WRITE;
DELETE FROM `cai_admin`.`sys_role_menu_permission`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_token` WRITE;
DELETE FROM `cai_admin`.`sys_token`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_user` WRITE;
DELETE FROM `cai_admin`.`sys_user`;
INSERT INTO `cai_admin`.`sys_user` (`id`,`login_name`,`login_password`,`role_id`,`is_disabled`,`create_time`,`update_time`) VALUES (-1, 'superadmin', 'admin', -1, 0, '1900-01-01 00:00:00', '1900-01-01 00:00:00'),(1, 'xuyong', 'co@123', 1, 0, '1900-01-20 18:22:39', '1900-01-20 18:46:44'),(2, 'lizhonglin', 'co@123', 1, 0, '1900-01-20 10:26:15', '1900-01-20 10:26:15');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `cai_admin`.`sys_user_login_log` WRITE;
DELETE FROM `cai_admin`.`sys_user_login_log`;
UNLOCK TABLES;
COMMIT;
