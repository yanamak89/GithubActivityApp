# GitHub Activity CLI

## 📌 Overview
The **GitHub Activity CLI** is a command-line tool that fetches and displays a user's recent activity from GitHub. This tool allows users to track their public activity, such as commits, issues, stars, and repository creations, directly from the terminal.
Link to the task **(https://roadmap.sh/projects/github-user-activity)**.

## 🚀 Features
- **Fetch recent activity** of a GitHub user.
- **Display user actions** such as commits, issue creations, and repository events.
- **Error handling** for invalid usernames or API failures.
- **Supports filtering** by event type (e.g., only show `PushEvent` or `CreateEvent`).
- **No external libraries** are used to fetch API data.

## 🛠️ Requirements
- **.NET 6.0** or later.
- **Redis** (for caching GitHub activity data)
- Internet connection (to fetch GitHub activity).

## 🔧 Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/yourusername/GitHubActivityCLI.git
   cd GitHubActivityCLI
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

## 🚀 Usage

## 🔍 Testing Redis
To ensure Redis is running correctly, you can perform the following tests:

1. **Check if Redis is running:**
   ```bash
   redis-cli ping
   ```
   If Redis is running, it should return:
   ```
   PONG
   ```

2. **Set and Get a test key:**
   ```bash
   redis-cli SET test_key "Hello, Redis!"
   redis-cli GET test_key
   ```
   The expected output should be:
   ```
   "Hello, Redis!"
   ```

3. **Check stored GitHub activity data:**
   ```bash
   redis-cli GET <GitHubUsername>
   ```
   Replace `<GitHubUsername>` with the actual username you are fetching activity for.

4. **Flush Redis Cache (if needed):**
   ```bash
   redis-cli FLUSHALL
   ```
   This will clear all stored data in Redis.

Run the application with a GitHub username:

```bash
   dotnet run <GitHubUsername>
```

### 📌 Example:
```bash
   dotnet run yanamak89
```

✅ **Expected Output (Example)**:
```
Recent activity for yanamak89:
- Pushed 1 commits to yanamak89/TaskTracker
- Pushed 1 commits to yanamak89/yanamak89
- Created a branch in yanamak89/TaskTracker
- Created a repository in yanamak89/TaskTracker
```

## 🔍 API Endpoint Used
The application fetches activity data from GitHub’s public API:
```
https://api.github.com/users/<GitHubUsername>/events
```
Example:
```
https://api.github.com/users/kamranahmedse/events
```

## ⚠️ Error Handling
- If the **username is invalid** or doesn't exist, an error message will be displayed.
- If the **GitHub API is down** or rate-limited, the CLI will handle the failure gracefully.

## 🌟 Future Enhancements
- Implement **caching** to reduce API calls and improve performance.
- Add **color formatting** for better readability.
- Support additional **GitHub API endpoints** for more insights.

## ❓ Questions or Issues?
If you encounter any issues, feel free to create an issue on the repository or contact the developer at **yana.makogon@gmail.com**.


