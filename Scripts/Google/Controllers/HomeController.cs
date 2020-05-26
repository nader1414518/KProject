using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

/* This class Holds the search Results */
public class search_results
{
    public static List<Result> results = new List<Result>();
    public static string response = "";
}
/* Search functionality class */
public class HomeController : MonoBehaviour
{
    public Text google_search_output_txt;

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            //string[] pages = url.Split('/');
            //int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                //string total_result = "";
                //Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                search_results.response = webRequest.downloadHandler.text;
                dynamic jsonData = JsonConvert.DeserializeObject(search_results.response);
                Debug.Log("Response Received: " + search_results.response);
                //List<Result> results = new List<Result>();
                if (google_search_output_txt)
                {
                    google_search_output_txt.text = "";
                }
                List<Result> results = new List<Result>();
                if (jsonData != null)
                {
                    foreach (var item in jsonData.items)
                    {
                        results.Add(new Result
                        {
                            Title = item.title,
                            Link = item.link,
                            Snippet = item.snippet,
                        });
                        if (google_search_output_txt)
                        {
                            google_search_output_txt.text += item.title + "\n" + item.snippet + "\n" + item.link + "\n\n";
                            Debug.Log(item.title + "\n" + item.snippet + "\n" + item.link + "\n\n");
                        }
                        //total_result = total_result + result.Title + "\n" + result.Snippet + "\n" + result.Link + "\n\n";
                        //Debug.Log("Result Added");
                    }
                    search_results.results = results;
                    jsonData = null;
                    results = null;
                }
                //Debug.Log("Searched For " + ui_manager.google_search_if_primary.text);
                //ui_manager.log_screen_info("Searched for " + ui_manager.google_search_if_primary.text, 5.0f);
            }
        }
    }

    public void StoreResults(string searchQuery)
    {
        //string searchQuery = Request["search"];
        string cx = "005074222856120532374:tznaiep6tsf";
        string apiKey = "AIzaSyDnE7c4WxNzcXAEaCr2FPkIKtiJA2pCQxc";
        string url = "https://www.googleapis.com/customsearch/v1?key=" + apiKey + "&cx=" + cx + "&q=" + searchQuery;
        //UnityWebRequest request = UnityWebRequest.Get("https://www.googleapis.com/customsearch/v1?key=" + apiKey + "&cx=" + cx + "&q=" + searchQuery);
        //request.SendWebRequest();
        //string responseString = request.downloadHandler.text;
        //var request = WebRequest.Create("https://www.googleapis.com/customsearch/v1?key=" + apiKey + "&cx=" + cx + "&q=" + searchQuery);
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //Stream dataStream = response.GetResponseStream();
        //StreamReader reader = new StreamReader(dataStream);
        //string responseString = reader.ReadToEnd();
        //Debug.Log("Response String: " + responseString);
        //dynamic jsonData = JsonConvert.DeserializeObject(responseString);
        StartCoroutine(GetRequest(url));
    }
}