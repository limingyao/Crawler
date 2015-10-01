/*
Navicat MySQL Data Transfer

Source Server         : 222.204.232.60
Source Server Version : 50541
Source Host           : 222.204.232.60:3306
Source Database       : grain

Target Server Type    : MYSQL
Target Server Version : 50541
File Encoding         : 65001

Date: 2015-08-09 18:16:23
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for grain
-- ----------------------------
DROP TABLE IF EXISTS `grain`;
CREATE TABLE `grain` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `grain_name` varchar(255) DEFAULT NULL,
  `grain_price` varchar(255) DEFAULT NULL,
  `grain_organization` varchar(255) DEFAULT NULL,
  `grain_date` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of grain
-- ----------------------------
