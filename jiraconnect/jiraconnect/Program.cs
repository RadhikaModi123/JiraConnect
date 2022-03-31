using Atlassian.Jira;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class JiraRepository : IJiraRepository
{
   
    private Jira  _jira ;

    public JiraRepository(string server, string userName, string password)
    {
        _jira = new Jira("SEN-30195395", "RADHIKA MODI", "");
        _jira.MaxIssuesPerRequest = 1000;
    }

    public async Task<Project> GetProjectAsync(string key)
    {
        return await Task.Factory.StartNew(() => _jira.GetProjects().Where((p => p.Key == key)).FirstOrDefault());
    }

    public async Task<Issue> GetIssueAsync(string key)
    {
        return await Task.Factory.StartNew(() => _jira.GetIssue(key));
    }

    public Task<List<Issue>> GetEpicBySummaryAsync(string project, string summary)
    {
        return Task.Factory.StartNew(() => (from i in _jira.Issues
                                            where i.Project == project && i.Type == "Epic" && i.Summary == summary
                                            select i).ToList());
    }

    public Task<List<Issue>> GetStoryIssuesAsync(string project)
    {
        return Task.Factory.StartNew(() => (from i in _jira.Issues
                                            where i.Project == project && i.Type == "Story"
                                            select i).ToList());
    }

    public Task<List<Issue>> GetEpicssAsync(string project)
    {
        return Task.Factory.StartNew(() => (from i in _jira.Issues
                                            where i.Project == project && i.Type == "Epic"
                                            select i).ToList());
    }

    public Task<Issue> CreateIssueAsync(string project, string summary, string description, string epicKey = null, string guid = null, string assignee = null, string reporter = null)
    {
        return Task.Factory.StartNew(() =>
        {
            Issue newIssue = _jira.CreateIssue(project);
            newIssue.Summary = summary;
            newIssue.Description = description;
            newIssue.Assignee = assignee;
            newIssue.Reporter = reporter;
            newIssue.Priority = "Low";
            newIssue.Type = "Story";

            if (epicKey != null)
                newIssue["Epic Link"] = epicKey;

            newIssue.SaveChanges();
            newIssue.Refresh();
            return newIssue;
        });
    }

    public Task<Issue> UpdateIssueAsync(Issue issue)
    {
        return Task.Factory.StartNew(() =>
        {
            issue.SaveChanges();
            issue.Refresh();
            return issue;
        });
    }
}

