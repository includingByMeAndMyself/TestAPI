using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IIssuesClient = Timesheet.Domain.Interfaces.IClient.IIssuesClient;

namespace Timesheet.Itegrations.GitHub
{
    //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
    public class IssuesClient : IIssuesClient
    {
        private readonly IGitHubClient _gitHubClient;

        public IssuesClient(string token)
        {
            var client = new GitHubClient(new ProductHeaderValue("my-cool"));
            var tokenAuth = new Credentials(token); // NOTE: not real token
            client.Credentials = tokenAuth;

            _gitHubClient = client;
        }

        public async Task<Domain.Models.Issue[]> Get(string login)
        {
            var projects = await _gitHubClient
                .Repository
                .Project
                .GetAllForRepository(login, "project name");

            var timesheetProject = projects.FirstOrDefault();

            var columns = await _gitHubClient.Repository.Project.Column
                .GetAll(timesheetProject.Id);

            var issues = new List<Domain.Models.Issue>();

            foreach (var column in columns)
            {
                var columnCards = await _gitHubClient.Repository.Project.Card
                    .GetAll(column.Id);

                var columnIssues = columnCards.Select(x => new Domain.Models.Issue
                {
                    Id = 0,
                    Name = x.Note ?? x.ContentUrl,
                    SourceId = x.Id
                });

                issues.AddRange(columnIssues);
            }

            return issues.ToArray();
        }
    }
}
