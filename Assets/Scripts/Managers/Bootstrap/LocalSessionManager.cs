using System.Collections.Generic;

public class LocalSessionManager : SingletonPersistent<LocalSessionManager>
{
    public User User { get; set; }

    private void Start()
    {
        User = PlayerPrefsUtility.LoadFromPlayPrefs<User>(Constants.PlayerPrefs.USER);
    }

    public void Initialize(string username)
    {
        User = new User()
        {
            Username = username,
            PictureIndex = 0,
            CardSleeveIndex = 0,
            DeckCollection = new DeckCollection()
        };

        PlayerPrefsUtility.SaveToPlayPrefs(Constants.PlayerPrefs.USER, User);
    }

    public Deck GetSelectedDeck()
    {
        return User.DeckCollection.Decks[User.DeckCollection.SelectedDeckIndex];
    }
}

public class User
{
    public string Username { get; set; }
    public int PictureIndex { get; set; }
    public int CardSleeveIndex { get; set; }
    public DeckCollection DeckCollection { get; set; }
}

public class DeckCollection
{
    public int SelectedDeckIndex { get; set; }
    public List<Deck> Decks { get; set; }
}
public class Deck
{
    public string DeckName { get; set; }
    public List<string> PlayDeck { get; set; }
    public List<string> CharacterDeck { get; set; }
}