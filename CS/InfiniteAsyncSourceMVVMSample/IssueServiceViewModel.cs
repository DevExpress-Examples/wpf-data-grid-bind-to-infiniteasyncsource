using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Data;
using DevExpress.Mvvm.Xpf;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace InfiniteAsyncSourceMVVMSample {
    public class IssueServiceViewModel : ViewModelBase {
        public UserData[] Users {
            get { return GetValue<UserData[]>(); }
            private set { SetValue(value); }
        }
        public IssueServiceViewModel() {
            AssignUsers();
        }
        async void AssignUsers() {
            Users = await IssuesService.GetUsersAsync();
        }

        [Command]
        public void FetchIssues(FetchRowsAsyncArgs args) {
            args.Result = GetIssuesAsync(args);
        }
        async Task<FetchRowsResult> GetIssuesAsync(FetchRowsAsyncArgs args) {
            var take = args.Take ?? 30;
            var issues = await IssuesService.GetIssuesAsync(
                skip: args.Skip,
                take: take,
                sortOrder: GetIssueSortOrder(args.SortOrder),
                filter: (IssueFilter)args.Filter);

            return new FetchRowsResult(issues, hasMoreRows: issues.Length == take);
        }
        static IssueSortOrder GetIssueSortOrder(SortDefinition[] sortOrder) {
            if(sortOrder.Length > 0) {
                var sort = sortOrder.Single();
                if(sort.PropertyName == "Created") {
                    if(sort.Direction != ListSortDirection.Descending)
                        throw new InvalidOperationException();
                    return IssueSortOrder.CreatedDescending;
                }
                if(sort.PropertyName == "Votes") {
                    return sort.Direction == ListSortDirection.Ascending
                        ? IssueSortOrder.VotesAscending
                        : IssueSortOrder.VotesDescending;
                }
            }
            return IssueSortOrder.Default;
        }

        [Command]
        public void GetTotalSummaries(GetSummariesAsyncArgs args) {
            args.Result = GetTotalSummariesAsync(args);
        }
        static async Task<object[]> GetTotalSummariesAsync(GetSummariesAsyncArgs e) {
            var summaryValues = await IssuesService.GetSummariesAsync((IssueFilter)e.Filter);
            return e.Summaries.Select(x => {
                if(x.SummaryType == SummaryType.Count)
                    return (object)summaryValues.Count;
                if(x.SummaryType == SummaryType.Max && x.PropertyName == "Created")
                    return summaryValues.LastCreated;
                throw new InvalidOperationException();
            }).ToArray();
        }

        [Command]
        public void GetUniqueValues(GetUniqueValuesAsyncArgs args) {
            if(args.PropertyName == "Priority") {
                var values = Enum.GetValues(typeof(Priority)).Cast<object>().ToArray();
                args.Result = Task.FromResult(values);
            } else {
                throw new InvalidOperationException();
            }
        }

        [Command]
        public void UpdateIssue(RowValidationArgs args) {
            args.ResultAsync = UpdateIssueAsync((IssueData)args.Item);
        }
        static async Task<ValidationErrorInfo> UpdateIssueAsync(IssueData issue) {
            await IssuesService.UpdateRowAsync(issue);
            return null;
        }
    }
}
