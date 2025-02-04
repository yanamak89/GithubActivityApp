using System.Globalization;
using GithubActivityApp;
using StackExchange.Redis;

class Program
{
    private static readonly ConnectionMultiplexer redis = 
        ConnectionMultiplexer.Connect("localhost");

    private static readonly IDatabase cache = redis.GetDatabase();
    
    public static void Main(string[] args)
    {
        
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: github-activity <GitHubUsername>[eventType]");
            return;
        }
        
        string username = args[0];
        string eventTypeFilter = args.Length > 1 ? args[1] : null;
        string apiUrl = $"https://api.github.com/users/{username}/events";

        try
        {
            string jsonResponse = FetchCachedData(username) ?? GitHubClient.FetchData(apiUrl);
            List<Activity> activities = GitHubClient.ParseActivity(jsonResponse);
            CacheData(username, jsonResponse);

            if (activities.Count == 0)
            {
                Console.WriteLine($"No recent activity found for user: {username}");
                return;
            }

            if (!string.IsNullOrEmpty(eventTypeFilter))
            {
                activities = activities.Where(a => a.Type.Equals(eventTypeFilter,
                    StringComparison.OrdinalIgnoreCase)).ToList();
            }
            Console.WriteLine($"Recent activity for {username}");
            foreach (var activity in activities)
            {
                PrintColoredActivity(activity);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    private static string FetchCachedData(string username)
    {
        string cachedData = cache.StringGet(username);
        if (!string.IsNullOrEmpty(cachedData))
        {
            Console.WriteLine("[Using Cached Data]");
            return cachedData;
        }
        Console.WriteLine("[No cache found, fetching fresh data...]");
        return null;
    }

    private static void CacheData(string username, string jsonData)
    {
        Console.WriteLine("[Caching fresh data...]");
        Console.WriteLine($"Cached data: {jsonData.Substring(0, Math.Min(200, jsonData.Length))}...");
        cache.StringSet(username, jsonData, TimeSpan.FromMinutes(10));
    }

    private static void PrintColoredActivity(Activity activity)
    {
        ConsoleColor color = activity.Type switch
        {
            "PushEvent"  => ConsoleColor.Green,
            "IssuesEvent" => ConsoleColor.Yellow,
            "WatchEvent"  => ConsoleColor.Cyan,
            _             => ConsoleColor.DarkBlue
        };

        Console.ForegroundColor = color;
        Console.WriteLine($"- [{activity.Type}] {activity.Description}");
        Console.ResetColor();
    }
}
