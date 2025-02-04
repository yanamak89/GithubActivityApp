using System.Net;
using System.Text.Json;

namespace GithubActivityApp;

public class GitHubClient
{
    public static string FetchData(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.UserAgent = "GitHubActivityCLI";

        using var response = (HttpWebResponse)request.GetResponse();
        using var stream = response.GetResponseStream();
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static List<Activity> ParseActivity(string json)
    {
        var events = JsonSerializer.Deserialize<List<JsonElement>>(json);
        List<Activity> activities = new();

        foreach (var ev in events)
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
        return activities;
    }
}