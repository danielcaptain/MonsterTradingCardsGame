﻿using System.Drawing;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace MonsterTradingCardsGame.Models
{
    public class User
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; }

        [JsonPropertyName("Password")]
        public string Password { get; set; }
        public int Coins { get; set; }
        public string Aliasname { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }

        public static string CreateToken(string username)
        {
            return $"{username}-mtcgToken";
        }

        public User()
        {
            Username = "";
            Password = "";
            Coins = 0;
            Aliasname = "";
            Bio = "";
            Image = "";
            Token = "";
        }


        public User(string username, string password, int coins, string aliasname, string bio, string image)
        {
            Username = username;
            Password = password;
            Coins = coins;
            Aliasname = aliasname;
            Bio = bio;
            Image = image;
            Token = CreateToken(username);
        }

        public User(string username, string password, int coins, string aliasname, string bio, string image, string token)
        {
            Username = username;
            Password = password;
            Coins = coins;
            Aliasname = aliasname;
            Bio = bio;
            Image = image;
            Token = token;
        }
    }
}
