using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

using Spring.Social.OAuth2;
using Spring.Social.Facebook.Api;
using Spring.Social.Facebook.Connect;

namespace Spring.ConsoleQuickStart
{
    class Program
    {
        // Register your own Facebook app at https://developers.facebook.com/apps.
        // Set your application id & secret here
        private const string FacebookApiId = TODO;
        private const string FacebookApiSecret = TODO;

        static void Main(string[] args)
        {
            try
            {
                FacebookServiceProvider facebookServiceProvider = new FacebookServiceProvider(FacebookApiId, FacebookApiSecret);

                /* OAuth 'dance' */

                // Authentication using the client-side authorization flow 
                OAuth2Parameters parameters = new OAuth2Parameters()
                {
                    RedirectUrl = "https://www.facebook.com/connect/login_success.html",
                    Scope = "email, user_relationships,friends_relationships, user_about_me, friends_about_me, user_birthday, friends_birthday, user_hometown, friends_hometown, user_location, friends_location, user_website, friends_website, read_stream"
                };
                string authorizationUrl = facebookServiceProvider.OAuthOperations.BuildAuthorizeUrl(GrantType.ImplicitGrant, parameters);
                Console.WriteLine("Redirect user to Facebook for authorization: " + authorizationUrl);
                Process.Start(authorizationUrl);
                Console.WriteLine("Enter 'access_token' query string parameter from success url:");
                string accessToken = Console.ReadLine();

                /* API */

                IFacebook facebook = facebookServiceProvider.GetApi(accessToken);
                FacebookProfile me = facebook.UserOperations.GetUserProfile();
                DumpProfile("", me);

                string sTestUserID = "splendidcrm";
                TestUserOperations(facebook, sTestUserID);
                TestPlacesOperations(facebook, sTestUserID);
                TestLikeOperations(facebook, sTestUserID);
                TestFriendOperations(facebook, sTestUserID);
                TestFeedOperations(facebook, sTestUserID);
                TestGroupOperations(facebook, sTestUserID);
                TestCommentOperations(facebook, sTestUserID);
                TestEventOperations(facebook, sTestUserID);
                TestMediaOperations(facebook, sTestUserID);
                TestPageOperations(facebook, sTestUserID);
                TestQuestionOperations(facebook, sTestUserID);
                TestOpenGraphOperations(facebook, sTestUserID);
                TestFqlOperations(facebook, sTestUserID);
            }
            catch (Spring.Rest.Client.HttpResponseException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.GetResponseBodyAsString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("--- hit <return> to quit ---");
                Console.ReadLine();
            }
        }

        #region Dump
        static void DumpReference(string sHeader, Reference from)
        {
            if (from != null)
            {
                Console.WriteLine(sHeader + "ID    : " + from.ID);
                Console.WriteLine(sHeader + "Name  : " + from.Name);
            }
        }

        static void DumpComment(string sHeader, Comment comment)
        {
            Console.WriteLine(sHeader + "ID       : " + comment.ID);
            Console.WriteLine(sHeader + "Message  : " + comment.Message);
            DumpReference(sHeader + "From ", comment.From);
            if (comment.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime:" + comment.CreatedTime.Value.ToString());
            Console.WriteLine(sHeader + "LikesCount: " + comment.LikesCount.ToString());
            if (comment.Likes != null)
            {
                foreach (Reference like in comment.Likes)
                {
                    DumpReference(sHeader + "    Like: ", like);
                }
            }
        }

        static void DumpLocation(string sHeader, Location location)
        {
            if (location != null)
            {
                Console.WriteLine(sHeader + "Latitude : " + location.Latitude);
                Console.WriteLine(sHeader + "Longitude: " + location.Longitude);
                Console.WriteLine(sHeader + "Street   : " + location.Street);
                Console.WriteLine(sHeader + "City     : " + location.City);
                Console.WriteLine(sHeader + "State    : " + location.State);
                Console.WriteLine(sHeader + "Country  : " + location.Country);
                Console.WriteLine(sHeader + "Zip      : " + location.Zip);
            }
        }

        static void DumpPage(string sHeader, Page page)
        {
            Console.WriteLine(sHeader + "ID             : " + page.ID);
            Console.WriteLine(sHeader + "Name           : " + page.Name);
            Console.WriteLine(sHeader + "Link           : " + page.Link);
            Console.WriteLine(sHeader + "Category       : " + page.Category);
            Console.WriteLine(sHeader + "Description    : " + page.Description);
            Console.WriteLine(sHeader + "Website        : " + page.Website);
            Console.WriteLine(sHeader + "Picture        : " + page.Picture);
            Console.WriteLine(sHeader + "Phone          : " + page.Phone);
            Console.WriteLine(sHeader + "Affiliation    : " + page.Affiliation);
            Console.WriteLine(sHeader + "CompanyOverview: " + page.CompanyOverview);
            DumpLocation(sHeader + "      ", page.Location);
            Console.WriteLine(sHeader + "FanCount: " + page.FanCount.ToString());
            Console.WriteLine(sHeader + "Likes   : " + page.Likes.ToString());
            Console.WriteLine(sHeader + "Checkins: " + page.Checkins.ToString());
        }

        static void DumpProfile(string sHeader, FacebookProfile profile)
        {
            Console.WriteLine(sHeader + "ID       : " + profile.ID);
            Console.WriteLine(sHeader + "Username : " + profile.Username);
            Console.WriteLine(sHeader + "FirstName: " + profile.FirstName);
            Console.WriteLine(sHeader + "LastName : " + profile.LastName);
            Console.WriteLine(sHeader + "Email    : " + profile.Email);
            Console.WriteLine(sHeader + "Location : " + profile.Location);
            Console.WriteLine(sHeader + "Website  : " + profile.Website);
            Console.WriteLine(sHeader + "Birthday : " + profile.Birthday);
            Console.WriteLine(sHeader + "Gender   : " + profile.Gender);
            Console.WriteLine(sHeader + "About    : " + profile.About);
        }

        static void DumpGroup(string sHeader, Group group)
        {
            Console.WriteLine(sHeader + "ID         : " + group.ID);
            Console.WriteLine(sHeader + "Version    : " + group.Version);
            Console.WriteLine(sHeader + "Icon       : " + group.Icon);
            DumpReference(sHeader + "Owner      :", group.Owner);
            Console.WriteLine(sHeader + "Name       : " + group.Name);
            Console.WriteLine(sHeader + "Description: " + group.Description);
            Console.WriteLine(sHeader + "Link       : " + group.Link);
            Console.WriteLine(sHeader + "Privacy    : " + group.Privacy.ToString());
            if (group.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime:" + group.UpdatedTime.Value.ToString());
        }

        static void DumpStoryTag(string sHeader, StoryTag tag)
        {
            Console.WriteLine(sHeader + "ID         : " + tag.ID);
            Console.WriteLine(sHeader + "Name       : " + tag.Name);
            Console.WriteLine(sHeader + "Offset     : " + tag.Offset);
            Console.WriteLine(sHeader + "Length     : " + tag.Length);
        }

        static void DumpPost(string sHeader, Post post)
        {
            Console.WriteLine(sHeader + "ID          : " + post.ID);
            DumpReference(sHeader + "From        : ", post.From);
            Console.WriteLine(sHeader + "Caption     : " + post.Caption);
            Console.WriteLine(sHeader + "Message     : " + post.Message);
            Console.WriteLine(sHeader + "Picture     : " + post.Picture);
            Console.WriteLine(sHeader + "Link        : " + post.Link);
            Console.WriteLine(sHeader + "Name        : " + post.Name);
            Console.WriteLine(sHeader + "Description : " + post.Description);
            Console.WriteLine(sHeader + "Icon        : " + post.Icon);
            if (post.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime :" + post.CreatedTime.Value.ToString());
            if (post.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime :" + post.UpdatedTime.Value.ToString());
            Console.WriteLine(sHeader + "Application :", post.Application);
            Console.WriteLine(sHeader + "Type        :", post.Type.ToString());
            Console.WriteLine(sHeader + "LikeCount   :", post.LikeCount);
            Console.WriteLine(sHeader + "SharesCount :", post.SharesCount);
            Console.WriteLine(sHeader + "Story       :", post.Story);
            Console.WriteLine(sHeader + "CommentCount:", post.CommentCount);
            if (post.To != null)
            {
                foreach (Reference r in post.To)
                    DumpReference(sHeader + "To         : ", r);
            }
            if (post.Likes != null)
            {
                foreach (Reference r in post.Likes)
                    DumpReference(sHeader + "Like       : ", r);
            }
            if (post.StoryTags != null)
            {
                foreach (KeyValuePair<int, List<StoryTag>> tag in post.StoryTags)  // Dictionary<int, List<StoryTag>>
                {
                    foreach (StoryTag tag2 in tag.Value)
                        DumpStoryTag(sHeader + "StoryTag   : ", tag2);
                }
            }
            if (post.Comments != null)
            {
                foreach (Comment comment in post.Comments)
                    DumpComment(sHeader + "Comment     : ", comment);
            }
        }

        static void DumpEvent(string sHeader, Event evt)
        {
            Console.WriteLine(sHeader + "ID          : " + evt.ID);
            DumpReference(sHeader + "Owner       : ", evt.Owner);
            Console.WriteLine(sHeader + "Name        : " + evt.Name);
            Console.WriteLine(sHeader + "Description : " + evt.Description);
            if (evt.StartTime.HasValue)
                Console.WriteLine(sHeader + "StartTime   :" + evt.StartTime.Value.ToString());
            if (evt.EndTime.HasValue)
                Console.WriteLine(sHeader + "EndTime     :" + evt.EndTime.Value.ToString());
            Console.WriteLine(sHeader + "Location    : " + evt.Location);
            DumpLocation(sHeader + "Venue       : ", evt.Venue);
            Console.WriteLine(sHeader + "Privacy     : " + evt.Privacy.ToString());
            if (evt.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime :" + evt.UpdatedTime.Value.ToString());
        }

        static void DumpAlbum(string sHeader, Album album)
        {
            Console.WriteLine(sHeader + "ID          : " + album.ID);
            DumpReference(sHeader + "From ", album.From);
            Console.WriteLine(sHeader + "Name        : " + album.Name);
            Console.WriteLine(sHeader + "Description : " + album.Description);
            Console.WriteLine(sHeader + "Location    : " + album.Location);
            Console.WriteLine(sHeader + "Link        : " + album.Link);
            Console.WriteLine(sHeader + "CoverPhotoId: " + album.CoverPhotoId);
            Console.WriteLine(sHeader + "Privacy     : " + album.Privacy.ToString());
            Console.WriteLine(sHeader + "Count       : " + album.Count);
            Console.WriteLine(sHeader + "Type        : " + album.Type.ToString());
            if (album.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime :" + album.CreatedTime.Value.ToString());
            if (album.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime :" + album.UpdatedTime.Value.ToString());
            Console.WriteLine(sHeader + "CanUpload   : " + album.CanUpload.ToString());
        }

        static void DumpTag(string sHeader, Tag tag)
        {
            if (tag != null)
            {
                Console.WriteLine(sHeader + "ID          : " + tag.ID);
                Console.WriteLine(sHeader + "Name        : " + tag.Name);
                Console.WriteLine(sHeader + "X           : " + tag.X);
                Console.WriteLine(sHeader + "Y           : " + tag.Y);
                if (tag.CreatedTime.HasValue)
                    Console.WriteLine(sHeader + "CreatedTime :" + tag.CreatedTime.Value.ToString());
            }
        }

        static void DumpVideo(string sHeader, Video video)
        {
            Console.WriteLine(sHeader + "ID          : " + video.ID);
            DumpReference(sHeader + "From ", video.From);
            Console.WriteLine(sHeader + "Name        : " + video.Name);
            Console.WriteLine(sHeader + "Description : " + video.Description);
            Console.WriteLine(sHeader + "Picture     : " + video.Picture);
            Console.WriteLine(sHeader + "EmbedHtml   : " + video.EmbedHtml);
            Console.WriteLine(sHeader + "Icon        : " + video.Icon);
            Console.WriteLine(sHeader + "Source      : " + video.Source);
            if (video.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime :" + video.CreatedTime.Value.ToString());
            if (video.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime :" + video.UpdatedTime.Value.ToString());
            if (video.Tags != null)
            {
                foreach (Tag tag in video.Tags)
                    DumpTag(sHeader + "Tag    : ", tag);
            }
            if (video.Comments != null)
            {
                foreach (Comment comment in video.Comments)
                    DumpComment(sHeader + "Comment: ", comment);
            }
        }

        static void DumpQuestionOption(string sHeader, QuestionOption option)
        {
            Console.WriteLine(sHeader + "ID          : " + option.ID);
            DumpReference(sHeader + "From ", option.From);
            Console.WriteLine(sHeader + "Name        : " + option.Name);
            Console.WriteLine(sHeader + "Votes       : " + option.Votes.ToString());
            if (option.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime :" + option.CreatedTime.Value.ToString());
            DumpPage(sHeader + "Page ", option.Object);
        }

        static void DumpQuestion(string sHeader, Question question)
        {
            Console.WriteLine(sHeader + "ID          : " + question.ID);
            DumpReference(sHeader + "From ", question.From);
            Console.WriteLine(sHeader + "Text        : " + question.Text);
            if (question.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime :" + question.CreatedTime.Value.ToString());
            if (question.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime :" + question.UpdatedTime.Value.ToString());
            if (question.Options != null)
            {
                foreach (QuestionOption option in question.Options)
                {
                    DumpQuestionOption(sHeader + "  ", option);
                }
            }
        }

        static void DumpPhoto(string sHeader, Photo photo)
        {
            Console.WriteLine(sHeader + "ID          : " + photo.ID);
            DumpReference(sHeader + "From ", photo.From);
            Console.WriteLine(sHeader + "Name        : " + photo.Name);
            Console.WriteLine(sHeader + "Icon        : " + photo.Icon);
            Console.WriteLine(sHeader + "Picture     : " + photo.Picture);
            Console.WriteLine(sHeader + "Source      : " + photo.Source);
            Console.WriteLine(sHeader + "Link        : " + photo.Link);
            Console.WriteLine(sHeader + "Height      : " + photo.Height.ToString());
            Console.WriteLine(sHeader + "Width       : " + photo.Width.ToString());
            Console.WriteLine(sHeader + "Position    : " + photo.Position.ToString());
            if (photo.CreatedTime.HasValue)
                Console.WriteLine(sHeader + "CreatedTime :" + photo.CreatedTime.Value.ToString());
            if (photo.UpdatedTime.HasValue)
                Console.WriteLine(sHeader + "UpdatedTime :" + photo.UpdatedTime.Value.ToString());
            DumpPage(sHeader + "Place ", photo.Place);
            if (photo.OversizedImage != null) Console.WriteLine(sHeader + "OversizedImage: " + photo.OversizedImage.Source);
            if (photo.SourceImage != null) Console.WriteLine(sHeader + "SourceImage   : " + photo.SourceImage.Source);
            if (photo.SmallImage != null) Console.WriteLine(sHeader + "SmallImage    : " + photo.SmallImage.Source);
            if (photo.AlbumImage != null) Console.WriteLine(sHeader + "AlbumImage    : " + photo.AlbumImage.Source);
            if (photo.TinyImage != null) Console.WriteLine(sHeader + "TinyImage     : " + photo.TinyImage.Source);
            if (photo.Tags != null)
            {
                foreach (Tag tag in photo.Tags)
                    DumpTag(sHeader + "Tag    : ", tag);
            }
        }

        #endregion

        #region UserOperations
        static void TestUserOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("UserOperations");
            byte[] image1 = facebook.UserOperations.GetUserProfileImage();
            if (image1 != null)
                Console.WriteLine("Me Image size = " + image1.Length.ToString());
            byte[] image3 = facebook.UserOperations.GetUserProfileImage(ImageType.LARGE);
            if (image3 != null)
                Console.WriteLine("Me Image size = " + image3.Length.ToString());
            List<String> perm = facebook.UserOperations.GetUserPermissions();
            if (perm != null)
            {
                Console.WriteLine("Permissions");
                foreach (string s in perm)
                {
                    Console.WriteLine("    " + s);
                }
            }

            try
            {
                FacebookProfile profile2 = facebook.UserOperations.GetUserProfile(sTestUserID);
                if (profile2 != null)
                {
                    DumpProfile(sTestUserID + " ", profile2);
                }
                byte[] image2 = facebook.UserOperations.GetUserProfileImage(sTestUserID);
                if (image2 != null)
                    Console.WriteLine(sTestUserID + " Image size = " + image2.Length.ToString());
                byte[] image4 = facebook.UserOperations.GetUserProfileImage(sTestUserID, ImageType.NORMAL);
                if (image4 != null)
                    Console.WriteLine(sTestUserID + " Image size = " + image4.Length.ToString());

                List<Reference> search = facebook.UserOperations.Search(sTestUserID);
                if (search != null)
                {
                    foreach (Reference r in search)
                    {
                        Console.WriteLine("    " + r.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }
        }
        #endregion

        #region PlacesOperations
        static void TestPlacesOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("PlacesOperations");
            Console.WriteLine("Checkins");
            List<Checkin> checkins1 = facebook.PlacesOperations.GetCheckins();
            foreach (Checkin checkin in checkins1)
            {
                Console.WriteLine("Checkin ID         : " + checkin.ID);
                Console.WriteLine("Checkin Message    : " + checkin.Message);
                Console.WriteLine("Checkin Application: " + checkin.Application);
                if (checkin.CreatedTime.HasValue)
                    Console.WriteLine("Checkin CreatedTime: " + checkin.CreatedTime.Value.ToString());
                DumpReference("Checkin From ", checkin.From);
                if (checkin.Place != null)
                {
                    DumpPage("Checkin Place ", checkin.Place);
                }
                if (checkin.Tags != null)
                {
                    foreach (Reference r in checkin.Tags)
                    {
                        DumpReference("  Tag ", r);
                    }
                }
                if (checkin.Likes != null)
                {
                    foreach (Reference r in checkin.Likes)
                    {
                        DumpReference("  Like ", r);
                    }
                }
                if (checkin.Comments != null)
                {
                    foreach (Comment comment in checkin.Comments)
                    {
                        DumpComment("  Checkin Comment ", comment);
                    }
                }
            }
            List<Checkin> checkins2 = facebook.PlacesOperations.GetCheckins(0, 5);
            //List<Checkin> checkins3 = facebook.PlacesOperations.GetCheckins(string objectId);
            //List<Checkin> checkins4 = facebook.PlacesOperations.GetCheckins(string objectId, 0, 20);

            Console.WriteLine("GetCheckin");
            //Checkin checkin = facebook.PlacesOperations.GetCheckin(string checkinId);
            //string sCheckinID1 = facebook.PlacesOperations.Checkin(string placeId, double latitude, double longitude);
            //string sCheckinID2 = facebook.PlacesOperations.Checkin(string placeId, double latitude, double longitude, string message, string[] tags);

            List<Page> pages = facebook.PlacesOperations.Search("crm", 37.76, -122.427, 50000);
            foreach (Page page in pages)
            {
                DumpPage("  ", page);
            }
        }
        #endregion

        #region LikeOperations
        static void TestLikeOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("LikeOperations");
            Console.WriteLine("Like");
            Console.WriteLine("GetPagesLiked");
            List<Page> pages1 = facebook.LikeOperations.GetPagesLiked();
            foreach (Page page in pages1)
            {
                DumpPage("  ", page);
                List<Reference> likes1 = facebook.LikeOperations.GetLikes(page.ID);
                foreach (Reference like in likes1)
                {
                    DumpReference("  ", like);
                }
            }
            try
            {
                Console.WriteLine("GetPagesLiked " + sTestUserID);
                List<Page> pages2 = facebook.LikeOperations.GetPagesLiked(sTestUserID);
                foreach (Page page in pages2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            try
            {
                Console.WriteLine("Like");
                List<Page> pages3 = facebook.PlacesOperations.Search("crm", 37.76, -122.427, 50000);
                if (pages3 != null && pages3.Count > 0)
                {
                    string objectId = pages3[0].ID;
                    facebook.LikeOperations.Like(objectId);
                    facebook.LikeOperations.Unlike(objectId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetBooks");
            List<Page> books1 = facebook.LikeOperations.GetBooks();
            foreach (Page page in books1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetBooks " + sTestUserID);
                List<Page> books2 = facebook.LikeOperations.GetBooks(sTestUserID);
                foreach (Page page in books2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetMovies");
            List<Page> movies1 = facebook.LikeOperations.GetMovies();
            foreach (Page page in movies1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetMovies " + sTestUserID);
                List<Page> movies2 = facebook.LikeOperations.GetMovies(sTestUserID);
                foreach (Page page in movies2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetMusic");
            List<Page> music1 = facebook.LikeOperations.GetMusic();
            foreach (Page page in music1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetMusic " + sTestUserID);
                List<Page> music2 = facebook.LikeOperations.GetMusic(sTestUserID);
                foreach (Page page in music2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetTelevision");
            List<Page> tele1 = facebook.LikeOperations.GetTelevision();
            foreach (Page page in tele1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetTelevision " + sTestUserID);
                List<Page> tele2 = facebook.LikeOperations.GetTelevision(sTestUserID);
                foreach (Page page in tele2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetActivities");
            List<Page> activ1 = facebook.LikeOperations.GetActivities();
            foreach (Page page in activ1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetActivities " + sTestUserID);
                List<Page> activ2 = facebook.LikeOperations.GetActivities(sTestUserID);
                foreach (Page page in activ2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetInterests");
            List<Page> interest1 = facebook.LikeOperations.GetInterests();
            foreach (Page page in interest1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetInterests " + sTestUserID);
                List<Page> interest2 = facebook.LikeOperations.GetInterests(sTestUserID);
                foreach (Page page in interest2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetGames");
            List<Page> games1 = facebook.LikeOperations.GetGames();
            foreach (Page page in games1)
            {
                DumpPage("  ", page);
            }
            try
            {
                Console.WriteLine("GetGames " + sTestUserID);
                List<Page> games2 = facebook.LikeOperations.GetGames(sTestUserID);
                foreach (Page page in games2)
                {
                    DumpPage("  ", page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }
        }
        #endregion

        #region FriendOperations
        static void TestFriendOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("GetFriendLists");
            List<Reference> list1 = facebook.FriendOperations.GetFriendLists();
            try
            {
                Console.WriteLine("GetFriendLists " + sTestUserID);
                List<Reference> list2 = facebook.FriendOperations.GetFriendLists(sTestUserID);
                if (list2.Count > 0)
                {
                    string friendListId = list2[0].ID;
                    Reference list3 = facebook.FriendOperations.GetFriendList(friendListId);
                    List<Reference> members1 = facebook.FriendOperations.GetFriendListMembers(friendListId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("CreateFriendList");
            try
            {
                string friendListId = facebook.FriendOperations.CreateFriendList("Test " + DateTime.Now.ToString());
                //string sFriendListID2 = facebook.FriendOperations.CreateFriendList(sTestUserID, string name);
                facebook.FriendOperations.AddToFriendList(friendListId, sTestUserID);
                List<Reference> members2 = facebook.FriendOperations.GetFriendListMembers(friendListId);
                foreach (Reference member in members2)
                {
                    DumpReference("  ", member);
                }
                facebook.FriendOperations.RemoveFromFriendList(friendListId, sTestUserID);
                facebook.FriendOperations.DeleteFriendList(friendListId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetFriends");
            List<Reference> friends1 = facebook.FriendOperations.GetFriends();
            List<String> friends2 = facebook.FriendOperations.GetFriendIds();
            List<FacebookProfile> friends3 = facebook.FriendOperations.GetFriendProfiles();
            List<FacebookProfile> friends4 = facebook.FriendOperations.GetFriendProfiles(0, 20);

            try
            {
                Console.WriteLine("GetFriends " + sTestUserID);
                List<Reference> friends5 = facebook.FriendOperations.GetFriends(sTestUserID);
                List<String> friends6 = facebook.FriendOperations.GetFriendIds(sTestUserID);
                List<FacebookProfile> profiles1 = facebook.FriendOperations.GetFriendProfiles(sTestUserID);
                List<FacebookProfile> profiles2 = facebook.FriendOperations.GetFriendProfiles(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetFamily");
            List<FamilyMember> families1 = facebook.FriendOperations.GetFamily();
            try
            {
                Console.WriteLine("GetFamily " + sTestUserID);
                List<FamilyMember> families2 = facebook.FriendOperations.GetFamily(sTestUserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            try
            {
                Console.WriteLine("GetMutualFriends " + sTestUserID);
                List<Reference> friends7 = facebook.FriendOperations.GetMutualFriends(sTestUserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetSubscribedTo");
            List<Reference> subscribed1 = facebook.FriendOperations.GetSubscribedTo();
            try
            {
                Console.WriteLine("GetSubscribedTo " + sTestUserID);
                List<Reference> subscribed2 = facebook.FriendOperations.GetSubscribedTo(sTestUserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetSubscribers");
            List<Reference> subscriber1 = facebook.FriendOperations.GetSubscribers();
            try
            {
                Console.WriteLine("GetSubscribers " + sTestUserID);
                List<Reference> subscriber2 = facebook.FriendOperations.GetSubscribers(sTestUserID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }
        }
        #endregion

        #region FeedOperations
        static void TestFeedOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("FeedOperations");

            Console.WriteLine("GetFeed");
            List<Post> post1 = facebook.FeedOperations.GetFeed();
            foreach (Post feed in post1)
            {
                DumpPost("  Feed ", feed);
                Post post7 = facebook.FeedOperations.GetPost(feed.ID);
            }
            List<Post> post2 = facebook.FeedOperations.GetFeed(0, 20);
            try
            {
                Console.WriteLine("GetFeed " + sTestUserID);
                List<Post> post3 = facebook.FeedOperations.GetFeed(sTestUserID);
                List<Post> post4 = facebook.FeedOperations.GetFeed(sTestUserID, 0, 20);
                foreach (Post feed in post1)
                {
                    DumpPost("  Feed ", feed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            try
            {
                Console.WriteLine("GetHomeFeed");
                List<Post> post5 = facebook.FeedOperations.GetHomeFeed();
                List<Post> post6 = facebook.FeedOperations.GetHomeFeed(0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetStatuses");
            List<StatusPost> status1 = facebook.FeedOperations.GetStatuses();
            List<StatusPost> status2 = facebook.FeedOperations.GetStatuses(0, 20);
            try
            {
                Console.WriteLine("GetStatuses " + sTestUserID);
                List<StatusPost> status3 = facebook.FeedOperations.GetStatuses(sTestUserID);
                List<StatusPost> status4 = facebook.FeedOperations.GetStatuses(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetLinks");
            List<LinkPost> links1 = facebook.FeedOperations.GetLinks();
            List<LinkPost> links2 = facebook.FeedOperations.GetLinks(0, 20);
            try
            {
                Console.WriteLine("GetLinks " + sTestUserID);
                List<LinkPost> links3 = facebook.FeedOperations.GetLinks(sTestUserID);
                List<LinkPost> links4 = facebook.FeedOperations.GetLinks(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetNotes");
            List<NotePost> notes1 = facebook.FeedOperations.GetNotes();
            List<NotePost> notes2 = facebook.FeedOperations.GetNotes(0, 20);
            try
            {
                Console.WriteLine("GetNotes " + sTestUserID);
                List<NotePost> notes3 = facebook.FeedOperations.GetNotes(sTestUserID);
                List<NotePost> notes4 = facebook.FeedOperations.GetNotes(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetPosts");
            List<Post> posts1 = facebook.FeedOperations.GetPosts();
            List<Post> posts2 = facebook.FeedOperations.GetPosts(0, 20);
            try
            {
                Console.WriteLine("GetPosts " + sTestUserID);
                List<Post> posts3 = facebook.FeedOperations.GetPosts(sTestUserID);
                List<Post> posts4 = facebook.FeedOperations.GetPosts(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("UpdateStatus");
            try
            {
                // 04/15/2012 Paul.  These tests will generate email notifications, so test sparingly. 
                /*
                string statusID = facebook.FeedOperations.UpdateStatus("Test " + DateTime.Now.ToString());
                facebook.FeedOperations.DeletePost(statusID);
				
                FacebookLink link = new FacebookLink("https://amazon.com", "amazon", "amazon", "amazon");
                string linkID = facebook.FeedOperations.PostLink("Test " + DateTime.Now.ToString(), link);
                facebook.FeedOperations.DeletePost(linkID);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }
            try
            {
                // 04/15/2012 Paul.  These tests will generate email notifications, so test sparingly. 
                /*
                Console.WriteLine("Post " + sTestUserID);
                string postID = facebook.FeedOperations.Post(sTestUserID, "Test " + DateTime.Now.ToString());
                facebook.FeedOperations.DeletePost(postID);
				
                FacebookLink link = new FacebookLink("https://amazon.com", "amazon", "amazon", "amazon");
                string linkID = facebook.FeedOperations.PostLink(sTestUserID, "Test " + DateTime.Now.ToString(), link);
                facebook.FeedOperations.DeletePost(linkID);
                */
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("SearchPublicFeed");
            List<Post> search1 = facebook.FeedOperations.SearchPublicFeed("crm");
            List<Post> search2 = facebook.FeedOperations.SearchPublicFeed("crm", 0, 20);
            List<Post> search3 = facebook.FeedOperations.SearchHomeFeed("crm");
            List<Post> search4 = facebook.FeedOperations.SearchHomeFeed("crm", 0, 20);
            List<Post> search5 = facebook.FeedOperations.SearchUserFeed("crm");
            List<Post> search6 = facebook.FeedOperations.SearchUserFeed("crm", 0, 20);
            try
            {
                Console.WriteLine("SearchUserFeed " + sTestUserID);
                List<Post> search7 = facebook.FeedOperations.SearchUserFeed(sTestUserID, "crm");
                List<Post> search8 = facebook.FeedOperations.SearchUserFeed(sTestUserID, "crm", 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }
        }
        #endregion

        #region GroupOperations
        static void TestGroupOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("GroupOperations");
            Console.WriteLine("GetMemberships");
            List<GroupMembership> memberships1 = facebook.GroupOperations.GetMemberships();
            foreach (GroupMembership group in memberships1)
            {
                string groupId = group.ID;
                Group group1 = facebook.GroupOperations.GetGroup(groupId);
                DumpGroup("  Group ", group1);

                Console.WriteLine("  GetGroupImage");
                byte[] image1 = facebook.GroupOperations.GetGroupImage(groupId);
                byte[] image2 = facebook.GroupOperations.GetGroupImage(groupId, ImageType.SMALL);

                Console.WriteLine("  GetMembers");
                List<GroupMemberReference> members1 = facebook.GroupOperations.GetMembers(groupId);
                List<FacebookProfile> members2 = facebook.GroupOperations.GetMemberProfiles(groupId);
            }
            try
            {
                Console.WriteLine("GetMemberships " + sTestUserID);
                List<GroupMembership> memberships2 = facebook.GroupOperations.GetMemberships(sTestUserID);
                foreach (GroupMembership group in memberships2)
                {
                    string groupId = group.ID;
                    Group group1 = facebook.GroupOperations.GetGroup(groupId);
                    DumpGroup("Group ", group1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("Search");
            List<Group> groups1 = facebook.GroupOperations.Search("crm");
            foreach (Group group in groups1)
            {
                DumpGroup("  Group ", group);

                Console.WriteLine("  GetGroupImage");
                byte[] image1 = facebook.GroupOperations.GetGroupImage(group.ID);
                byte[] image2 = facebook.GroupOperations.GetGroupImage(group.ID, ImageType.SMALL);

                Console.WriteLine("  GetMembers");
                List<GroupMemberReference> members1 = facebook.GroupOperations.GetMembers(group.ID);
                foreach (GroupMemberReference member in members1)
                {
                    DumpReference("    Group Member", member);
                }
                List<FacebookProfile> members2 = facebook.GroupOperations.GetMemberProfiles(group.ID);
            }
            List<Group> groups2 = facebook.GroupOperations.Search("crm", 0, 20);
        }
        #endregion

        #region CommentOperations
        static void TestCommentOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("CommentOperations");
            Console.WriteLine("GetComments");
            List<Post> post1 = facebook.FeedOperations.GetFeed();
            foreach (Post post in post1)
            {
                string objectId = post.ID;
                List<Comment> comments1 = facebook.CommentOperations.GetComments(objectId);
                List<Comment> comments2 = facebook.CommentOperations.GetComments(objectId, 0, 20);
                foreach (Comment comment in comments1)
                {
                    Comment comment1 = facebook.CommentOperations.GetComment(comment.ID);
                    DumpComment("  ", comment1);
                }
                List<Reference> likes1 = facebook.CommentOperations.GetLikes(objectId);
                foreach (Reference like in likes1)
                {
                    DumpReference("  ", like);
                }
            }

            Console.WriteLine("AddComment");
            if (post1 != null && post1.Count > 0)
            {
                string objectId = post1[0].ID;
                DumpPost("", post1[0]);
                string commentId = facebook.CommentOperations.AddComment(objectId, "Test " + DateTime.Now.ToString());
                Comment comment1 = facebook.CommentOperations.GetComment(commentId);
                DumpComment("  ", comment1);
                facebook.CommentOperations.DeleteComment(commentId);
            }
        }
        #endregion

        #region EventOperations
        static void TestEventOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("EventOperations");
            Console.WriteLine("GetInvitations");
            List<Invitation> events1 = facebook.EventOperations.GetInvitations();
            List<Invitation> events2 = facebook.EventOperations.GetInvitations(0, 20);
            try
            {
                Console.WriteLine("GetInvitations " + sTestUserID);
                List<Invitation> events3 = facebook.EventOperations.GetInvitations(sTestUserID);
                List<Invitation> events4 = facebook.EventOperations.GetInvitations(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            try
            {
                Console.WriteLine("CreateEvent");
                string eventID = facebook.EventOperations.CreateEvent("Test " + DateTime.Now.ToString(), DateTime.Now, DateTime.Now.AddHours(1));
                try
                {
                    Console.WriteLine("AcceptInvitation");
                    facebook.EventOperations.MaybeInvitation(eventID);
                    facebook.EventOperations.AcceptInvitation(eventID);
                    facebook.EventOperations.DeclineInvitation(eventID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("  " + ex.Message);
                }
                facebook.EventOperations.DeleteEvent(eventID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("Search");
            List<Event> search1 = facebook.EventOperations.Search("crm");
            foreach (Event event1 in search1)
            {
                Console.WriteLine("GetEvent");

                string eventId = event1.ID;
                Event event2 = facebook.EventOperations.GetEvent(eventId);
                DumpEvent("  ", event2);
                byte[] image1 = facebook.EventOperations.GetEventImage(eventId);
                byte[] image2 = facebook.EventOperations.GetEventImage(eventId, ImageType.SMALL);

                //Console.WriteLine("GetInvited");
                List<EventInvitee> events5 = facebook.EventOperations.GetInvited(eventId);
                List<EventInvitee> events6 = facebook.EventOperations.GetAttending(eventId);
                List<EventInvitee> events7 = facebook.EventOperations.GetMaybeAttending(eventId);
                List<EventInvitee> events8 = facebook.EventOperations.GetNoReplies(eventId);
                List<EventInvitee> events9 = facebook.EventOperations.GetDeclined(eventId);
            }
            List<Event> search2 = facebook.EventOperations.Search("crm", 0, 20);
        }
        #endregion

        #region MediaOperations
        static void TestMediaOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("MediaOperations");
            Console.WriteLine("GetAlbums");
            List<Album> albums1 = facebook.MediaOperations.GetAlbums();
            List<Album> albums2 = facebook.MediaOperations.GetAlbums(0, 20);
            try
            {
                Console.WriteLine("GetAlbums " + sTestUserID);
                List<Album> albums3 = facebook.MediaOperations.GetAlbums(sTestUserID);
                foreach (Album album in albums3)
                {
                    string albumId = album.ID;
                    Album album1 = facebook.MediaOperations.GetAlbum(albumId);
                    DumpAlbum("  ", album1);
                    byte[] image1 = facebook.MediaOperations.GetAlbumImage(albumId);
                    byte[] image2 = facebook.MediaOperations.GetAlbumImage(albumId, ImageType.SMALL);

                }
                List<Album> albums4 = facebook.MediaOperations.GetAlbums(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("CreateAlbum");
            //string sAlbumID = facebook.MediaOperations.CreateAlbum(string name, string description);

            Console.WriteLine("GetPhotos");
            List<Photo> photos1 = facebook.MediaOperations.GetPhotos();
            foreach (Photo photo in photos1)
            {
                string photoId = photo.ID;
                Photo photo1 = facebook.MediaOperations.GetPhoto(photoId);
                DumpPhoto("  ", photo);
                byte[] image3 = facebook.MediaOperations.GetPhotoImage(photoId);
                byte[] image4 = facebook.MediaOperations.GetPhotoImage(photoId, ImageType.SMALL);
            }
            List<Photo> photos2 = facebook.MediaOperations.GetPhotos("me", 0, 20);
            try
            {
                List<Photo> photos3 = facebook.MediaOperations.GetPhotos(sTestUserID);
                foreach (Photo photo in photos3)
                {
                    string photoId = photo.ID;
                    Photo photo1 = facebook.MediaOperations.GetPhoto(photoId);
                    DumpPhoto("  ", photo);
                    byte[] image3 = facebook.MediaOperations.GetPhotoImage(photoId);
                    byte[] image4 = facebook.MediaOperations.GetPhotoImage(photoId, ImageType.SMALL);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("PostPhoto");
            //string sPostID1 = facebook.MediaOperations.PostPhoto(Resource photo);
            //string sPostID2 = facebook.MediaOperations.PostPhoto(Resource photo, string caption);
            //string sPostID3 = facebook.MediaOperations.PostPhoto(string albumId, Resource photo);
            //string sPostID4 = facebook.MediaOperations.PostPhoto(string albumId, Resource photo, string caption);

            Console.WriteLine("GetVideos");
            List<Video> videos1 = facebook.MediaOperations.GetVideos();
            List<Video> videos2 = facebook.MediaOperations.GetVideos(0, 20);
            try
            {
                Console.WriteLine("GetVideos " + sTestUserID);
                List<Video> videos3 = facebook.MediaOperations.GetVideos(sTestUserID);
                foreach (Video video in videos3)
                {
                    string videoId = video.ID;
                    Video video1 = facebook.MediaOperations.GetVideo(videoId);
                    DumpVideo("  ", video1);
                    byte[] image5 = facebook.MediaOperations.GetVideoImage(videoId);
                    byte[] image6 = facebook.MediaOperations.GetVideoImage(videoId, ImageType.SMALL);
                }
                List<Video> videos4 = facebook.MediaOperations.GetVideos(sTestUserID, 0, 20);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("PostVideo");
            //string sPostID5 = facebook.MediaOperations.PostVideo(Resource video);
            //string sPostID6 = facebook.MediaOperations.PostVideo(Resource video, string title, string description);
        }
        #endregion

        #region PageOperations
        static void TestPageOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("PageOperations");
            Console.WriteLine("GetAccounts");
            List<Account> accounts1 = facebook.PageOperations.GetAccounts();

            Console.WriteLine("GetPage");
            List<Page> pages1 = facebook.LikeOperations.GetPagesLiked();
            foreach (Page page in pages1)
            {
                string pageId = page.ID;
                Page page1 = facebook.PageOperations.GetPage(pageId);
                facebook.PageOperations.IsPageAdmin(pageId);
            }

            Console.WriteLine("Post");
            //string sPostID1 = facebook.PageOperations.Post(string pageId, string message);
            //string sPostID2 = facebook.PageOperations.Post(string pageId, string message, FacebookLink link);

            Console.WriteLine("PostPhoto");
            //string sPostID3 = facebook.PageOperations.PostPhoto(string pageId, string albumId, Resource photo);
            //string sPostID4 = facebook.PageOperations.PostPhoto(string pageId, string albumId, Resource photo, string caption);
        }
        #endregion

        #region QuestionOperations
        static void TestQuestionOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("QuestionOperations");
            try
            {
                Console.WriteLine("AskQuestion");
                string questionId = facebook.QuestionOperations.AskQuestion("Test " + DateTime.Now.ToString());
                string sOption1 = facebook.QuestionOperations.AddOption(questionId, "Option " + DateTime.Now.ToString());
                facebook.QuestionOperations.DeleteQuestion(questionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }

            Console.WriteLine("GetQuestions");
            List<Question> questions1 = facebook.QuestionOperations.GetQuestions();
            foreach (Question question in questions1)
            {
                string questionId = question.ID;
                Question question1 = facebook.QuestionOperations.GetQuestion(questionId);
                DumpQuestion("  ", question1);

                List<QuestionOption> options1 = facebook.QuestionOperations.GetOptions(questionId);
                foreach (QuestionOption option in options1)
                {
                    string optionId = option.ID;
                    QuestionOption option1 = facebook.QuestionOperations.GetOption(optionId);
                    DumpQuestionOption("    ", option1);
                }
            }
            try
            {
                Console.WriteLine("GetQuestions " + sTestUserID);
                List<Question> questions2 = facebook.QuestionOperations.GetQuestions(sTestUserID);
                foreach (Question question in questions2)
                {
                    string questionId = question.ID;
                    Question question1 = facebook.QuestionOperations.GetQuestion(questionId);
                    DumpQuestion("  ", question1);

                    List<QuestionOption> options1 = facebook.QuestionOperations.GetOptions(questionId);
                    foreach (QuestionOption option in options1)
                    {
                        string optionId = option.ID;
                        QuestionOption option1 = facebook.QuestionOperations.GetOption(optionId);
                        DumpQuestionOption("    ", option1);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  " + ex.Message);
            }
        }
        #endregion

        #region OpenGraphOperations
        static void TestOpenGraphOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("OpenGraphOperations");
            //string sID = IFacebook.OpenGraphOperations.PublishAction(string action, string objectType, string objectUrl);
        }
        #endregion

        #region FqlOperations
        static void TestFqlOperations(IFacebook facebook, string sTestUserID)
        {
            Console.WriteLine();
            Console.WriteLine("FqlOperations");
            // https://developers.facebook.com/docs/reference/fql/
            string fql = "SELECT uid, username, name, first_name, last_name, email, contact_email, website, birthday_date, hometown_location, about_me FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1 = me())";
            List<Spring.Social.Facebook.Api.FacebookProfile> friends = facebook.FqlOperations.QueryFQL<List<FacebookProfile>>(fql);
            foreach (Spring.Social.Facebook.Api.FacebookProfile friend in friends)
            {
                DumpProfile("", friend);
            }
        }
        #endregion
    }
}

