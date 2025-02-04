using System.Net;
using System.Text.Json;

namespace GithubActivityApp;

public class GitHubClient
{
    public static string FetchData(string url)
    {
        Console.WriteLine($"[Fetching API data from {url}]");
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.UserAgent = "GitHubActivityCLI";

        using var response = (HttpWebResponse)request.GetResponse();
        using var stream = response.GetResponseStream();
        using var reader = new StreamReader(stream);
        string result = reader.ReadToEnd();
        Console.WriteLine("[API Response]: " + result.Substring(0, Math.Min(200, result.Length)) + "...");

        return result;
    }

    public static List<Activity> ParseActivity(string json)
    {
        Console.WriteLine("[Raw API JSON]:");
        Console.WriteLine(json.Substring(0, Math.Min(200, json.Length)) + "...");
        
        var events = JsonSerializer.Deserialize<List<JsonElement>>(json);
        List<Activity> activities = new();

        foreach (var ev in events)
        {
            try
            {
                string type = ev.GetProperty("type").GetString();
                string repo = ev.GetProperty("repo").GetProperty("name").GetString();

                if (type == "PushEvemt")
                {
                    int commits =
                        ev.GetProperty("payload")
                            .GetProperty("commits")
                            .GetArrayLength();
                }
                else if (type == "IssuesEvent")
                {
                    string action =
                        ev.GetProperty("payload")
                            .GetProperty("action")
                            .GetString();
                }

                else if (type == "WatchEvent")
                {
                    activities.Add(new Activity($"Starred {repo}"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Skipping an event due to error: {e.Message}");
                throw;
            }

        }
        return activities;
    }
}