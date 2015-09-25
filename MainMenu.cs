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

	public static bool IsMulti = true;
	public static string wwwtext="test";
	public static string username = "";
	private string pswd = ""; 
	private string repass = "";
	private string email = "";
	private string url = "http://losange-vision.com/registration.php";
	private string url_login = "http://losange-vision.com/login.php";

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
			if (w.text.Contains("userid=")) {
				userid = System.Int32.Parse(Regex.Match(w.text,"(?<=userid=)[0-9]+").ToString());
				LoggedIn = true;
			}
		}
	}
    public void OnGUI() {
        if (register){
            username = GUILayout.TextField(username);
            pswd = GUILayout.TextField(pswd);
            email = GUILayout.TextField(email);
            repass = GUILayout.TextField(repass);

            if (GUILayout.Button("Register")){
                if (username == "" || pswd == "" || repass == "" || email == ""){}
                else if (pswd == repass) DoRegister();
            }
        }
        else{
            username = GUILayout.TextField(username, GUILayout.Width(100));
            pswd = GUILayout.PasswordField(pswd, "*"[0], GUILayout.Width(100));
            if (GUILayout.Button("Login")){
                if (username != "" && pswd != "")
                DoLogin();
            }
            if (GUILayout.Button("Register"))
            register = true;
        }
    }
}
