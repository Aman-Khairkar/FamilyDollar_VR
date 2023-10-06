using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class EntryScreen : MonoBehaviour
{
    public Text pwDisplay;
    //private int entryCode = 2634;
    private HashSalt entryCodeHa = new HashSalt();
    private int enteredCode;
    // Start is called before the first frame update

    public class HashSalt
    {
        public string Hash { get; set; }
        public string Salt { get; set; }
    }

    public static HashSalt GenerateSaltedHash(int size, string password)
    {
        var saltBytes = new byte[size];
        var provider = new RNGCryptoServiceProvider();
        provider.GetNonZeroBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
        var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

        HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
        return hashSalt;
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
        return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
    }

    void Start()
    {
        entryCodeHa.Hash = "dv08SChHACy9p95AmMJOcHyxvu8DNL8Ydn9Ciq7qow+MHYI20US7kHr5lHUFxo2ciM9NuoGJ//bHahAypU4eu5Ply3X0PC9d6nykmF1qZlhLIwSQijEzKwerJ9AU2qE41NdTLyf8q8KkIA8LhuLb2JTXfFCh+7SIg07YU+SX7m9+LJ5w3W+vNYV+X/nVEcnG7BhDrotOCCZS7VP5Oo/Vpd+yauS2YqGJKhmUDercW1y3Df4OIzUtRzezN/44uQMhGfuYsOmHL1Hnw1A1rWevNz6s5paLYIVGKXSyZ258ydXSln7ZbyA8y/VqzBDDktMS+WOTMJ9wbvjP/HJ1cPEfFA==";
        entryCodeHa.Salt = "6hq8/nl9nqzjmBRspi9J887bJjntMI7yr9iNqBBkBOwjMdwy1YOjUjPYsfrbDAiC2B3si2H27ixkgvpK3ZpHag==";
       // HashSalt hashSalt = GenerateSaltedHash(64, entryCode.ToString());
       // Debug.Log("Hash to add is: " + hashSalt.Hash + " and salt is " + hashSalt.Salt);
    }

    public GameObject fadeScreen;
    public GameObject rightController;
    public void CompareValues()
    {
        if(VerifyPassword(enteredCode.ToString(), entryCodeHa.Hash, entryCodeHa.Salt))
        {
            pwDisplay.text = "Confirmed! Please wait...";
            StartCoroutine(PasswordConfirmed());
        }
        else
        {
            enteredCode = 0;
            pwDisplay.text = "Wrong Password!";
        }
    }

    IEnumerator PasswordConfirmed()
    {
        fadeScreen.GetComponent<FadeUser>().Fade();
        rightController.GetComponent<LineRenderer>().enabled = false;
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void AddNumber(int i)
    {
        if(enteredCode == 0)
        {
            enteredCode = i;
        }
        else if(enteredCode.ToString().Length < 5)
        {
            enteredCode = (enteredCode * 10) + i;
        }

            pwDisplay.text = enteredCode.ToString();
    }

    public void RemoveNumber()
    {
        if (enteredCode != 0)
        {
            //thank ebony for ints where we can remove the last number by dividing lmao
            enteredCode /= 10;
        }
            pwDisplay.text = enteredCode.ToString();
    }
}
