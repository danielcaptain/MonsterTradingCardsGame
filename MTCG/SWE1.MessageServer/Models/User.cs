namespace MonsterTradingCardsGame.Models
{
    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Coins { get; private set; }
        public string Aliasname { get; private set; }
        public string Bio { get; private set; }
        public string Image { get; private set; }
        public string Token { get; private set; }

        public static string CreateToken(string username)
        {
            return $"{username}-mtcgToken";
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
