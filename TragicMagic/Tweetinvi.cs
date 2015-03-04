using Otter;
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

// Matthew Cormack @johnjoemcbob
// 14/02/15
// Tweetinvi API for uploading an image of the battle to Twitter
// Depends on: N/A

namespace TragicMagic
{
	class TweetinviClass
	{
		public TweetinviClass()
		{
			string OAuthAccessToken = "3037861589-POUB49lcqUSE4Stlci2c6OvSQjJwbfILny9kGw2";
			string OAuthAccessTokenSecret = "";
			string ConsumerKey = "HOFJ5kSINAG0u3frgqDGPYpok";
			string ConsumerSecret = "";

			IOAuthCredentials credentials = TwitterCredentials.CreateCredentials( OAuthAccessToken, OAuthAccessTokenSecret, ConsumerKey, ConsumerSecret );
			TwitterCredentials.SetCredentials( credentials );
		}

		public static void SaveScreenshot( string filepath = "roundcomplete" )
		{
			Game.Instance.Surface.SaveToFile( filepath + ".png" );
		}

		public static void TweetImage( string text, string filepath = "roundcomplete" )
		{
			byte[] file = File.ReadAllBytes( filepath + ".png" );

			// Create a tweet with a single image
			var tweet = Tweet.CreateTweetWithMedia( text, file );

			tweet.Publish();
		}
	}
}