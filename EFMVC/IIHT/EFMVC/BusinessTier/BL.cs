using DataAccess;
using EFMVC.BusinessTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
    public class BL
    {
        public static void Signup(PersonModel personModel)
        {
            using (var ctx = new EFMVCContext())
            {
                personModel.Joined = DateTime.Now;
                personModel.Active = true;
                personModel.Password = CreateMD5(personModel.Password);
                ctx.People.Add(personModel.GetPerson());
                ctx.SaveChanges();
            }
        }

        public static PersonModel Profile(PersonModel personModel)
        {
            using (var ctx = new EFMVCContext())
            {
                var person = ctx.People.FirstOrDefault(a => a.user_id == personModel.UserId);
                person.password = CreateMD5(personModel.Password);
                person.fullName = personModel.FullName;
                person.email = personModel.Email;
                ctx.People.Add(person);
                ctx.Entry(person).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                return person.GetPersonModel();
            }
        }

        public static TweetModel AddTweet(TweetModel model)
        {
            using (var ctx = new EFMVCContext())
            {
                if (model.TweetId == 0)
                {
                    model.Created = DateTime.Now;
                    var tweet = model.GetTweet();
                    ctx.Tweets.Add(tweet);
                    ctx.SaveChanges();
                    return tweet.GetTweetModel();
                }
                else
                {
                    var tweet = ctx.Tweets.FirstOrDefault(a => a.tweet_id == model.TweetId);
                    tweet.message = model.Message;
                    ctx.Entry(tweet).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                    return tweet.GetTweetModel();
                }

            }
        }

        public static void DeleteTweet(int id)
        {
            using (var ctx = new EFMVCContext())
            {
                var tweet = ctx.Tweets.FirstOrDefault(a => a.tweet_id == id);
                ctx.Tweets.Remove(tweet);
                ctx.SaveChanges();
            }
        }

        public static void DeletePerson(string id)
        {
            using (var ctx = new EFMVCContext())
            {
                var person = ctx.People.FirstOrDefault(a => a.user_id == id);
                person.active = false;
                ctx.Entry(person).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static List<PersonModel> GetPerson(string id, string searchText)
        {
            using (var ctx = new EFMVCContext())
            {
                var persons = ctx.People.Where(a => a.user_id != id).ToList();
                if (!string.IsNullOrEmpty(searchText))
                {
                    persons = persons.Where(a => a.fullName.Contains(searchText)).ToList();
                }

                return persons.Select(a =>
                new PersonModel
                {
                    UserId = a.user_id,
                    Password = a.password,
                    FullName = a.fullName,
                    Email = a.email,
                    Active = a.active,
                    Joined = a.joined,
                    IsFollowing = a.Person1.Count() > 0 && a.Person1.Where(b => b.user_id == id).Count() > 0
                }).ToList();
            }
        }

        public static List<PersonModel> FollowPerson(string id, string currentuserid)
        {
            using (var ctx = new EFMVCContext())
            {
                var person = ctx.People.FirstOrDefault(a => a.user_id == id);
                var currentuser = ctx.People.FirstOrDefault(a => a.user_id == currentuserid);
                if (person.Person1.Where(a => a.user_id == currentuserid).Count() > 0)
                {
                    person.Person1.Remove(currentuser);
                }
                else
                {
                    currentuser.People.Add(person);
                }
                ctx.SaveChanges();
                return currentuser.Person1.Select(a => a.GetPersonModel()).ToList();
            }
        }

        public static void EditTweet(TweetModel model)
        {
            using (var ctx = new EFMVCContext())
            {
                var tweet = ctx.Tweets.FirstOrDefault(a => a.tweet_id == model.TweetId);
                tweet.message = model.Message;
                ctx.Tweets.Remove(model.GetTweet());
                ctx.SaveChanges();
            }
        }

        public static List<TweetModel> GetTweet(PersonModel model)
        {
            using (var ctx = new EFMVCContext())
            {
                var tweet = ctx.Tweets.ToList();
                var tweets = ctx.Tweets.Where(a => a.user_id == model.UserId ||
                (ctx.People.Select(b => b.user_id == model.UserId && b.Person1.Select(c => c.user_id == a.user_id).Count() > 0).Count() > 0)).ToList();
                return tweets.Select(a => a.GetTweetModel()).ToList();
            }
        }

        public static TweetModel GetTweetById(int id)
        {
            using (var ctx = new EFMVCContext())
            {
                var tweet = ctx.Tweets.FirstOrDefault(a => a.tweet_id == id);
                return tweet.GetTweetModel();
            }
        }

        public static PersonModel Login(PersonModel personModel)
        {
            using (var ctx = new EFMVCContext())
            {
                Person person = ctx.People.FirstOrDefault(a => a.user_id == personModel.UserId);

                if (person != null && person.password == CreateMD5(personModel.Password))
                {
                    return person.GetPersonModel();
                } 
                return null;
            }
        }

        public static string CreateMD5(string password)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public static class Map
    {
        public static Tweet GetTweet(this TweetModel model)
        {
            return new Tweet
            {
                user_id = model.UserId,
                created = model.Created,
                message = model.Message,
                tweet_id = model.TweetId
            };
        }

        public static TweetModel GetTweetModel(this Tweet model)
        {
            return new TweetModel
            {
                UserId = model.user_id,
                Created = model.created,
                Message = model.message,
                TweetId = model.tweet_id,
                FullName = model.Person != null ? model.Person.fullName : null
            };
        }

        public static Person GetPerson(this PersonModel model)
        {
            return new Person
            {
                user_id = model.UserId,
                password = model.Password,
                fullName = model.FullName,
                email = model.Email,
                active = model.Active,
                joined = model.Joined,
            };
        }

        public static PersonModel GetPersonModel(this Person model)
        {
            return new PersonModel
            {
                UserId = model.user_id,
                Password = model.password,
                FullName = model.fullName,
                Email = model.email,
                Active = model.active,
                Joined = model.joined,
            };
        }
    }
}