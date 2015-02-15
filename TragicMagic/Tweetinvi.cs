using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Parameters;
using Tweetinvi.Core.Interfaces.oAuth;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Json;

namespace TragicMagic
{
	class TweetinviClass
	{
		public TweetinviClass()
		{
			string OAuthAccessToken = "3037861589-POUB49lcqUSE4Stlci2c6OvSQjJwbfILny9kGw2";
			string OAuthAccessTokenSecret = "";
			string ConsumerKey = "AIfr1m8jogiO9M62xX5FFkiwM";
			string ConsumerSecret = "";

			//IOAuthCredentials credentials = TwitterCredentials.CreateCredentials( OAuthAccessToken, OAuthAccessTokenSecret, ConsumerKey, ConsumerSecret );
			//TwitterCredentials.SetCredentials( credentials );

			//Tweet_PublishTweetWithImage( "Tragic Magic", "../../resources/leap/leapcableback.png" );
		}

		private static void Tweet_PublishTweetWithImage( string text, string filePath )
		{
			byte[] file = File.ReadAllBytes( filePath );

			// Create a tweet with a single image
			var tweet = Tweet.CreateTweetWithMedia( text, file );

			tweet.Publish();
		}
	}
}