using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SinaDal.service
{
    class UserService
    {

        private UserDAO userDAO = new UserDAO();

        public Boolean insertUser(NetDimension.Weibo.Entities.user.Entity user)
        {
            userDAO.insertUserParameter(user.ID, user.IDStr, user.ScreenName, user.Name,
                           int.Parse(user.Province), int.Parse(user.City), user.Location, user.Description, user.Url,
                           user.ProfileImageUrl, user.ProfileUrl, user.Domain, user.Weihao, user.Gender,
                           user.FollowersCount, user.FriendsCount, user.StatusesCount, (int)user.FavouritesCount,
                           CommonLib.DataTranslate.SinaDateToString(user.CreatedAt), Convert.ToInt32(user.Following), Convert.ToInt32(user.AllowAllActMsg), Convert.ToInt32(user.GEOEnabled), Convert.ToInt32(user.Verified),
                           int.Parse(user.VerifiedType), user.Remark, Convert.ToInt32(user.AllowAllComment), user.AvatarLarge, user.VerifiedReason,
                           Convert.ToInt32(user.FollowMe), user.OnlineStatus, user.BIFollowersCount, user.Lang);
            return true;
        }

        public Boolean insertUserTags(string userID,string tagID,string tagName,int weight)
        {
            userDAO.insertUserTagsParameter(userID, tagID, tagName, weight);
            return true;
        }

        public Boolean insertUserRetweetedBlog(NetDimension.Weibo.Entities.status.Entity blog_data, string sText)
        {
            userDAO.insertUserBlogParameter(CommonLib.DataTranslate.SinaDateToString(blog_data.RetweetedStatus.CreatedAt), blog_data.RetweetedStatus.ID, blog_data.RetweetedStatus.MID, blog_data.RetweetedStatus.ID, sText, blog_data.RetweetedStatus.Source, Convert.ToInt32(blog_data.RetweetedStatus.Favorited),
                                    Convert.ToInt32(blog_data.RetweetedStatus.Truncated), blog_data.RetweetedStatus.InReplyToStuatusID, blog_data.RetweetedStatus.InReplyToUserID, blog_data.RetweetedStatus.InReplyToScreenName, blog_data.RetweetedStatus.ThumbnailPictureUrl,
                                    blog_data.RetweetedStatus.RepostsCount, blog_data.RetweetedStatus.CommentsCount);
            return true;
        }

        public Boolean insertUserBlog(NetDimension.Weibo.Entities.status.Entity blog_data, string sText)
        {
            userDAO.insertUserBlogParameter(CommonLib.DataTranslate.SinaDateToString(blog_data.CreatedAt), blog_data.ID, blog_data.MID, blog_data.ID, sText, blog_data.Source, Convert.ToInt32(blog_data.Favorited),
                                         Convert.ToInt32(blog_data.Truncated), blog_data.InReplyToStuatusID, blog_data.InReplyToUserID, blog_data.InReplyToScreenName, blog_data.ThumbnailPictureUrl,
                                         blog_data.RepostsCount, blog_data.CommentsCount);
            return true;
        }

        public Boolean insertUserBlogIDS(NetDimension.Weibo.Entities.user.Entity user, NetDimension.Weibo.Entities.status.Entity blog_data, string retweeted_status)
        {
            userDAO.insertUserBlogIDSParameter(user.ID, blog_data.ID, retweeted_status);
            return true;
        }
    }
}
