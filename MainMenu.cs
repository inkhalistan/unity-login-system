using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class MainMenu : MonoBehaviour {
    public bool PlayOffline = true;
	public static int userid;
	public static TCGData currentsettings;
    public static TCGData TCGMaker {
		get {
            if(currentsettings == null)     currentsettings = (TCGData)Resources.Load("TCGData", typeof(TCGData));
            return currentsettings;
		}
	}
	public static bool DownloadedPlayerDeck = false;
	public static bool DownloadedPlayerCollection = false;
	public static bool FirstLoadMenu = true;
	public static bool IsMulti = true;
	public static GUIStyle mystyle;
	public static bool CollectionNeedsUpdate = false;
	public static readonly string SceneNameMainMenu = "MainMenuScene";
    public static readonly string SceneNameMenu = "LobbyScene";
    public static readonly string SceneNameGame = "GameScene";
	public static readonly string SceneNameEditDeck = "EditDeckScene";
	public static string wwwtext="test";
	public static string username = "";
	private string pswd = ""; 
	private string repass = "";
	private string email = "";
	private string url = "http://losange-vision.com/registration.php";
	private string url_login = "http://losange-vision.com/login.php";
	private string url_latest_cards = "http://losange-vision.com/latestcards.php";
	private string url_player_deck = "http://losange-vision.com/playerdecks.php";
	private string url_player_collection = "http://losange-vision.com/playercollection.php";
	public static string url_update_deck = "http://losange-vision.com/updatedeck.php";
	private Hashtable[] promocards; 
	public static List <string> promo_prices = new List<string>();
	public static List <Vector3> promo_vector = new List<Vector3>();
	public static string deckstring, collectionstring;
	public static bool LoggedIn = false;
	private bool register = false;
	public static float ColliderWidth, ColliderHeight;
    public void Awake() {
		CardTemplate instance = CardTemplate.Instance;
    }
	public void Start() {
		if(FirstLoadMenu)
			playerDeck.pD.LoadPlayerDeckOffline();
        	else {
			if(LoggedIn) {
				Currency.GetCurrency();
				DoGetLatestCards();
				DoGetPlayerDeck();
				DoGetPlayerCollection();
			}
        	}
		FirstLoadMenu = false;
	}
	public void Update() {
        if(CollectionNeedsUpdate)   DoGetPlayerCollection();
        CollectionNeedsUpdate = false;
	}
	public static Texture2D SpriteToTexture(Sprite sprite) {
        Texture2D croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
		Color[] pixels = sprite.texture.GetPixels(0, 0, (int)sprite.rect.width, (int)sprite.rect.height);
		croppedTexture.SetPixels(pixels);
		croppedTexture.Apply();
        return croppedTexture;
	}
	static public Hashtable[] ParsePromocards(string cardsstring) {
        string[] lines = cardsstring.Split("\n"[0]); 
		string[] linearray;
		// finds the number of cards
		Hashtable[] output = new Hashtable[lines.Length-1];
		for(int i = 0; i < (lines.Length-1); i++) {
			output[i]=new Hashtable();
			linearray = lines[i].Split(","[0]);
			output[i]["id"] = linearray[0];
			output[i]["cost"] = linearray[1];
		}
        return output;
    }
	public void DoGetPlayerCollection() {
		WWWForm form = new WWWForm();
		form.AddField("userid", userid);
		WWW w = new WWW(url_player_collection, form);
		StartCoroutine(GetPlayerCollection(w));
	}
	IEnumerator GetPlayerCollection( WWW w) {
        yield return w;
		if (w.error ==null) {
			collectionstring = w.text;
			DownloadedPlayerCollection = true;
			playerDeck.pD.Collection = playerDeck.pD.LoadDeck(collectionstring);
		}
	}
	public void DoGetPlayerDeck() {
		WWWForm form = new WWWForm();
		form.AddField("userid", userid);
		WWW w = new WWW(url_player_deck, form);
		StartCoroutine(GetPlayerDeck(w));
	}
	IEnumerator GetPlayerDeck( WWW w) {
		yield return w;
		if (w.error ==null) {
			deckstring = w.text;
			DownloadedPlayerDeck = true;
			playerDeck.pD.Deck =  playerDeck.pD.LoadDeck(deckstring);
		}
	}
	public void DoGetLatestCards() {
		WWW w = new WWW(url_latest_cards);
		StartCoroutine(GetLatestCards(w));
	}
	IEnumerator GetLatestCards( WWW w) {
		yield return w;
		if (w.error ==null) {
			promo_prices.Clear();
			promo_vector.Clear();
			promocards = ParsePromocards(w.text);
			int Index;
			int i=0;
			foreach (Hashtable foundcard in promocards) {
			Index = System.Int32.Parse(foundcard["id"].ToString());
			GameObject promo_card_obj = new GameObject ();
			card promo_card = promo_card_obj.AddComponent<card>() as card; 
			promo_card.Index = Index;
			DbCard dbcard = MainMenu.TCGMaker.cards.Where(x => x.id == Index).SingleOrDefault();
			if (dbcard == null)     
				Debug.LogWarning("card not found in the new db!");
			promo_card.Type = dbcard.type;
			promo_card.Cost = dbcard.cost;
			promo_card.CardColor = dbcard.color;
				if (promo_card.IsACreature()) {
					promo_card.CreatureOffense = dbcard.offense; 
					promo_card.CreatureDefense = dbcard.defense; 
				}
			promo_card.CostInCurrency = System.Int32.Parse(foundcard["cost"].ToString());
			playerDeck.pD.AddArtAndText(promo_card);
			promo_card.transform.position = new Vector3 (3.79f + 2.5f*i, -0.9f, 0f);
			promo_prices.Add(foundcard["cost"].ToString());
			promo_vector.Add(Camera.main.WorldToScreenPoint(promo_card.transform.position));
			i++;
			}
		}
	}
	public void DoRegister() {
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", pswd);
		form.AddField("email", email);
		WWW w = new WWW(url, form);
		StartCoroutine(RegisterPlayer(w));
	}
	IEnumerator RegisterPlayer( WWW w) {
		yield return w;
		if (w.error ==null){}
	}
	public void DoLogin() {
		WWWForm form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", pswd);
		WWW w = new WWW(url_login, form);
		StartCoroutine(Login(w));
	}
    IEnumerator Login(WWW w) {
        yield return w;
		if(w.error ==null) {
			if (w.text.Contains("login-SUCCESS")) {
				userid = System.Int32.Parse(Regex.Match(w.text,"(?<=login-SUCCESS)[0-9]+").ToString());
				LoggedIn = true;
				Currency.GetCurrency();
				DoGetLatestCards();
				DoGetPlayerDeck();
				DoGetPlayerCollection();
			}
			else{}
		}
		else{}
	}
    public void OnGUI() {
		if (LoggedIn) {
			for (int i=0; i<promo_vector.Count; i++) {
				Vector3 p = promo_vector[i];
				GUI.Label(new Rect(p.x-30,Screen.height-p.y+70,200,30), "Price: " +promo_prices[i]);
			}
		}
        GUILayout.Label("deck count" + playerDeck.pD.Deck.Count.ToString());
        GUILayout.Box(Currency.messagecurrency);
        if (register){
            username = GUILayout.TextField(username);
            pswd = GUILayout.TextField(pswd);
            email = GUILayout.TextField(email);
            repass = GUILayout.TextField(repass);
            if (GUILayout.Button("Back")) register = false;
            if (GUILayout.Button("Register")){
                if (username == "" || pswd == "" || repass == "" || email == ""){}
                else if (pswd == repass) DoRegister();
            }
        }
        else if ((LoggedIn) || (PlayOffline == true)){
            if (GUILayout.Button("Single Game", GUILayout.Width(110))){
                IsMulti = false;
                Application.LoadLevel(SceneNameGame);
            }
            if (!PlayOffline){
                if (GUILayout.Button("Multiplayer Game", GUILayout.Width(110))) {
                    IsMulti = true;
                    Application.LoadLevel(SceneNameMenu);
                }
            }
            if (GUILayout.Button("Change deck", GUILayout.Width(110))) Application.LoadLevel(SceneNameEditDeck);
        }
        else{
            username = GUILayout.TextField(username, GUILayout.Width(100));
            pswd = GUILayout.PasswordField(pswd, "*"[0], GUILayout.Width(100));
            if (GUILayout.Button("Login")){
                if (username == "" || pswd == ""){}
                else DoLogin();
            }
            if (GUILayout.Button("Register")) register = true;
        }
    }
}
