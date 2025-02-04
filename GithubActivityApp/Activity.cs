namespace GithubActivityApp;

public class Activity
{
    public string Description { get; set; }
    public string Type { get; set; }

    public Activity(string description, string type)
    {
        Type = type;
        Description = description;
    }

    public Activity(string description)
    {
        Description = description;
    }

    public override string ToString()
    {
        return Description;
    }
}
