using EFMVC.BusinessTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.BusinessTier
{
    public class TwitterStreamModel
    {
        public TwitterStreamModel()
        {
            Person = new PersonModel();
            TweetList = new List<TweetModel>();
            PersonList = new List<PersonModel>();
        }
        public PersonModel Person { get; set; }

        public TweetModel CurrentTweet { get; set; }

        public List<TweetModel> TweetList { get; set; }

        public List<PersonModel> PersonList { get; set; }

        public string SearchText { get; set; }
    }
}
