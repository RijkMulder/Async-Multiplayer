using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
public class Request : MonoBehaviour
{
    [SerializeField] private KeyCode requestKey;
    private string url = "http://127.0.0.1/edsa-webdev/connect.php";

    public IEnumerator RequestExample(Fruit newfruit)
    {
        // get new highscore
        string newEntry = JsonUtility.ToJson(new Fruit 
            { fruit_name = newfruit.fruit_name, color = newfruit.color, quantity = newfruit.quantity }, true);

        // send new highscore
        List<IMultipartFormSection> form = new();
        form.Add(new MultipartFormDataSection("newEntry", newEntry));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.downloadHandler.text);
        }
    }

    /*private IEnumerator RequestExample()
    {
        // get new highscore
        string newHighscore = JsonUtility.ToJson(new HighScore { name = "John", score = 40 }, true);

        // send new highscore
        List<IMultipartFormSection> form = new();
        form.Add(new MultipartFormDataSection("newHighscore", newHighscore));
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();
            Debug.Log(webRequest.downloadHandler.text);
        }
    }*/
}

[System.Serializable]
public class Fruit
{
    public string fruit_name;
    public string color;
    public int quantity;
}
