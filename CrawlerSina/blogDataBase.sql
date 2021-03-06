USE [Sina_Blog_UInfu]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 09/04/2013 15:33:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[uid] [nvarchar](50) NOT NULL,
	[idstr] [nvarchar](max) NULL,
	[screen_name] [nvarchar](max) NULL,
	[name] [nvarchar](max) NULL,
	[province] [int] NULL,
	[city] [int] NULL,
	[location] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[url] [nvarchar](max) NULL,
	[profile_image_url] [nvarchar](max) NULL,
	[profile_url] [nvarchar](max) NULL,
	[domain] [nvarchar](max) NULL,
	[weihao] [nvarchar](max) NULL,
	[gender] [nvarchar](max) NULL,
	[followers_count] [int] NULL,
	[friends_count] [int] NULL,
	[statuses_count] [int] NULL,
	[favourites_count] [int] NULL,
	[created_at] [nvarchar](max) NULL,
	[following] [int] NULL,
	[allow_all_act_msg] [int] NULL,
	[geo_enabled] [int] NULL,
	[verified] [int] NULL,
	[verified_type] [int] NULL,
	[remark] [nvarchar](max) NULL,
	[status] [nvarchar](max) NULL,
	[allow_all_comment] [int] NULL,
	[avatar_large] [nvarchar](max) NULL,
	[verified_reason] [nvarchar](max) NULL,
	[follow_me] [int] NULL,
	[online_status] [int] NULL,
	[bi_followers_count] [int] NULL,
	[lang] [nvarchar](max) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [uniqueuid] UNIQUE NONCLUSTERED 
(
	[uid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_tags]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_tags](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[uid] [nvarchar](50) NULL,
	[tagid] [nvarchar](50) NULL,
	[name] [nvarchar](100) NULL,
	[weight] [int] NULL,
 CONSTRAINT [PK_User_tags] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [tagsunique] UNIQUE NONCLUSTERED 
(
	[uid] ASC,
	[tagid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Blog_IDS]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Blog_IDS](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[uid] [nvarchar](50) NOT NULL,
	[blogID] [nvarchar](50) NOT NULL,
	[retweeted_status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_User_Blog_IDS] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [uniqueuesrblogid] UNIQUE NONCLUSTERED 
(
	[uid] ASC,
	[blogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'user''s blog id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Blog_IDS', @level2type=N'COLUMN',@level2name=N'blogID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否原创' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'User_Blog_IDS', @level2type=N'COLUMN',@level2name=N'retweeted_status'
GO
/****** Object:  Table [dbo].[RuningLog]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuningLog](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[userid] [nvarchar](50) NOT NULL,
	[blogid] [nvarchar](50) NOT NULL,
	[commentid] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_RuningLog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Geo]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Geo](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[blogid] [nvarchar](50) NOT NULL,
	[uid] [nvarchar](50) NULL,
	[longitude] [nvarchar](500) NULL,
	[latitude] [nvarchar](500) NULL,
	[city] [nvarchar](50) NULL,
	[province] [nvarchar](50) NULL,
	[city_name] [nvarchar](200) NULL,
	[province_name] [nvarchar](200) NULL,
	[address] [nvarchar](500) NULL,
	[pinyin] [nvarchar](500) NULL,
	[more] [nvarchar](500) NULL,
 CONSTRAINT [PK_Geo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendShip]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendShip](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[uid] [nvarchar](50) NOT NULL,
	[friendid] [nvarchar](50) NOT NULL,
	[type] [int] NOT NULL,
 CONSTRAINT [PK_FriendShip] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [FriendShipunique] UNIQUE NONCLUSTERED 
(
	[uid] ASC,
	[friendid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1: uid关注friendid' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FriendShip', @level2type=N'COLUMN',@level2name=N'type'
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[created_at] [nvarchar](50) NULL,
	[commentsid] [nvarchar](50) NULL,
	[text] [text] NULL,
	[source] [nvarchar](1000) NULL,
	[userid] [nvarchar](50) NULL,
	[commentsmid] [nvarchar](50) NULL,
	[blogid] [nvarchar](50) NULL,
	[replaycommentid] [nvarchar](50) NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [commentsunique] UNIQUE NONCLUSTERED 
(
	[commentsid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论作者的ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Comments', @level2type=N'COLUMN',@level2name=N'userid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论的MID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Comments', @level2type=N'COLUMN',@level2name=N'commentsmid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论的微博' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Comments', @level2type=N'COLUMN',@level2name=N'blogid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'评论来源评论，当本评论属于对另一评论的回复时返回此字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Comments', @level2type=N'COLUMN',@level2name=N'replaycommentid'
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 09/04/2013 15:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[created_at] [nvarchar](50) NULL,
	[blogid] [nvarchar](50) NOT NULL,
	[blogmid] [nvarchar](50) NOT NULL,
	[idstr] [nvarchar](50) NULL,
	[text] [text] NULL,
	[source] [nvarchar](500) NULL,
	[favorited] [int] NULL,
	[truncated] [int] NULL,
	[in_reply_to_status_id] [nvarchar](max) NULL,
	[in_reply_to_user_id] [nvarchar](max) NULL,
	[in_reply_to_screen_name] [nvarchar](max) NULL,
	[thumbnail_pic] [nvarchar](max) NULL,
	[bmiddle_pic] [nvarchar](max) NULL,
	[original_pic] [nvarchar](max) NULL,
	[reposts_count] [int] NULL,
	[comments_count] [int] NULL,
	[attitudes_count] [int] NULL,
	[mlevel] [int] NULL,
 CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [blogunique] UNIQUE NONCLUSTERED 
(
	[blogid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Blog', @level2type=N'COLUMN',@level2name=N'blogid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'微博MID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Blog', @level2type=N'COLUMN',@level2name=N'blogmid'
GO
