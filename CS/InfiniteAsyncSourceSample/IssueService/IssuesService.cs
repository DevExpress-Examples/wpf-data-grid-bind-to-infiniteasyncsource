using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteAsyncSourceSample {
    public static class IssuesService {
        #region helpers
        static object SyncObject = new object();
        static Lazy<List<IssueData>> AllIssues = new Lazy<List<IssueData>>(() => {
            var date = DateTime.Today;
            var rnd = new Random(0);
            return Enumerable.Range(0, 100000)
                .Select(i => {
                    date = date.AddSeconds(-rnd.Next(20 * 60));
                    return new IssueData(
                        id: i,
                        subject: OutlookDataGenerator.GetSubject(),
                        user: OutlookDataGenerator.GetFrom(),
                        created: date,
                        votes: rnd.Next(100),
                        priority: OutlookDataGenerator.GetPriority());
                }).ToList();
        });
        class DefaultSortComparer : IComparer<IssueData> {
            int IComparer<IssueData>.Compare(IssueData x, IssueData y) {
                if(x.Created.Date != y.Created.Date)
                    return Comparer<DateTime>.Default.Compare(x.Created.Date, y.Created.Date);
                return Comparer<int>.Default.Compare(x.Votes, y.Votes);
            }
        } 
        #endregion

        public async static Task<IssueData[]> GetIssuesAsync(int skip, int take, IssueSortOrder sortOrder, IssueFilter filter) {
            await Task.Delay(300).ConfigureAwait(false);
            var issues = SortIssues(sortOrder, AllIssues.Value);
            if(filter != null)
                issues = FilterIssues(filter, issues);
            return issues.Skip(skip).Take(take).Select(x => x.Clone()).ToArray();
        }

        public async static Task<IssuesSummaries> GetSummariesAsync(IssueFilter filter) {
            await Task.Delay(300).ConfigureAwait(false);
            var issues = (IEnumerable<IssueData>)AllIssues.Value;
            if(filter != null)
                issues = FilterIssues(filter, issues);
            var lastCreated = issues.Any() ? issues.Max(x => x.Created) : default(DateTime?);
            return new IssuesSummaries(issues.Count(), lastCreated);
        }

        public async static Task AddNewIssueAsync(IssueData issueData) {
            await Task.Delay(300).ConfigureAwait(false);
            AllIssues.Value.Insert(0, issueData);
        }

        public async static Task DeleteIssueAsync(IssueData issueData) {
            await Task.Delay(300).ConfigureAwait(false);
            AllIssues.Value.Remove(issueData);
        }

        public async static Task UpdateRowAsync(IssueData row) {
            if(row == null)
                return;
            IssueData data = AllIssues.Value.FirstOrDefault(x => x.Id == row.Id);
            if(data == null)
                return;
            data.Priority = row.Priority;
            data.Subject = row.Subject;
            data.Votes = row.Votes;
            data.Created = row.Created;
            data.User = row.User;
            data.Id = row.Id;
            await Task.Delay(500).ConfigureAwait(false);
        }

        #region filter
        static IEnumerable<IssueData> FilterIssues(IssueFilter filter, IEnumerable<IssueData> issues) {
            if(filter.CreatedFrom != null || filter.CreatedTo != null) {
                if(filter.CreatedFrom == null || filter.CreatedTo == null) {
                    throw new InvalidOperationException();
                }
                issues = issues.Where(x => x.Created >= filter.CreatedFrom.Value && x.Created < filter.CreatedTo);
            }
            if(filter.MinVotes != null) {
                issues = issues.Where(x => x.Votes >= filter.MinVotes.Value);
            }
            if(filter.Priority != null) {
                issues = issues.Where(x => x.Priority == filter.Priority);
            }
            return issues;
        }
        #endregion

        #region sort
        static IEnumerable<IssueData> SortIssues(IssueSortOrder sortOrder, IEnumerable<IssueData> issues) {
            switch(sortOrder) {
            case IssueSortOrder.Default:
                return issues;//.OrderByDescending(x => x, new DefaultSortComparer()).ThenByDescending(x => x.Created);
            case IssueSortOrder.CreatedDescending:
                return issues.OrderByDescending(x => x.Created);
            case IssueSortOrder.VotesAscending:
                return issues.OrderBy(x => x.Votes).ThenByDescending(x => x.Created);
            case IssueSortOrder.VotesDescending:
                return issues.OrderByDescending(x => x.Votes).ThenByDescending(x => x.Created);
            default:
                throw new InvalidOperationException();
            }
        } 
        #endregion
    }
}
