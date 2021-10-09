Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks

Namespace InfiniteAsyncSourceMVVMSample

    Public Module IssuesService

#Region "helpers"
        Private SyncObject As Object = New Object()

        Private AllIssues As Lazy(Of List(Of IssueData)) = New Lazy(Of List(Of IssueData))(Function()
            Dim [date] = Date.Today
            Dim rnd = New Random(0)
            Return Enumerable.Range(0, 100000).[Select](Function(i)
                [date] = [date].AddSeconds(-rnd.Next(20 * 60))
                Return New IssueData(id:=i, subject:=GetSubject(), userId:=GetFromId(), created:=[date], votes:=rnd.Next(100), priority:=GetPriority())
            End Function).ToList()
        End Function)

        Private Class DefaultSortComparer
            Implements IComparer(Of IssueData)

            Private Function Compare(ByVal x As IssueData, ByVal y As IssueData) As Integer Implements IComparer(Of IssueData).Compare
                If x.Created.Date <> y.Created.Date Then Return Comparer(Of Date).Default.Compare(x.Created.Date, y.Created.Date)
                Return Comparer(Of Integer).Default.Compare(x.Votes, y.Votes)
            End Function
        End Class

#End Region
        Public Async Function GetIssuesAsync(ByVal skip As Integer, ByVal take As Integer, ByVal sortOrder As IssueSortOrder, ByVal filter As IssueFilter) As Task(Of IssueData())
            Await Task.Delay(300).ConfigureAwait(False)
            Dim issues = SortIssues(sortOrder, AllIssues.Value)
            If filter IsNot Nothing Then issues = FilterIssues(filter, issues)
            Return issues.Skip(skip).Take(take).[Select](Function(x) x.Clone()).ToArray()
        End Function

        Public Async Function GetUsersAsync() As Task(Of UserData())
            Await Task.Delay(300).ConfigureAwait(False)
            Return Users.[Select](Function(x, i)
                Dim split = x.Split(" "c)
                Return New UserData(i, split(0), split(1))
            End Function).ToArray()
        End Function

        Public Async Function GetSummariesAsync(ByVal filter As IssueFilter) As Task(Of IssuesSummaries)
            Await Task.Delay(300).ConfigureAwait(False)
            Dim issues = CType(AllIssues.Value, IEnumerable(Of IssueData))
            If filter IsNot Nothing Then issues = FilterIssues(filter, issues)
            Dim lastCreated = If(issues.Any(), issues.Max(Function(x) x.Created), DirectCast(Nothing, Date?))
            Return New IssuesSummaries(count:=issues.Count(), lastCreated:=lastCreated)
        End Function

        Public Async Function UpdateRowAsync(ByVal row As IssueData) As Task
            If row Is Nothing Then Return
            Dim data As IssueData = AllIssues.Value.FirstOrDefault(Function(x) x.Id = row.Id)
            If data Is Nothing Then Return
            data.Priority = row.Priority
            data.Subject = row.Subject
            data.UserId = row.UserId
            data.Votes = row.Votes
            data.Created = row.Created
            Await Task.Delay(500).ConfigureAwait(False)
        End Function

        Public Async Function AddNewRowAsync(ByVal row As IssueData) As Task
            Await Task.Delay(500).ConfigureAwait(False)
            AllIssues.Value.Insert(0, row)
            row.Id = AllIssues.Value.Max(Function(x) x.Id) + 1
        End Function

        Public Async Function DeleteRowAsync(ByVal row As IssueData) As Task
            Await Task.Delay(500).ConfigureAwait(False)
            AllIssues.Value.Remove(AllIssues.Value.Find(Function(x) x.Id = row.Id))
        End Function

#Region "filter"
        Private Function FilterIssues(ByVal filter As IssueFilter, ByVal issues As IEnumerable(Of IssueData)) As IEnumerable(Of IssueData)
            If filter.CreatedFrom IsNot Nothing OrElse filter.CreatedTo IsNot Nothing Then
                If filter.CreatedFrom Is Nothing OrElse filter.CreatedTo Is Nothing Then
                    Throw New InvalidOperationException()
                End If

                issues = issues.Where(Function(x) x.Created >= filter.CreatedFrom.Value AndAlso x.Created < filter.CreatedTo)
            End If

            If filter.MinVotes IsNot Nothing Then
                issues = issues.Where(Function(x) x.Votes >= filter.MinVotes.Value)
            End If

            If filter.Priority IsNot Nothing Then
                issues = issues.Where(Function(x) x.Priority = filter.Priority)
            End If

            Return issues
        End Function

#End Region
#Region "sort"
        Private Function SortIssues(ByVal sortOrder As IssueSortOrder, ByVal issues As IEnumerable(Of IssueData)) As IEnumerable(Of IssueData)
            Select Case sortOrder
                Case IssueSortOrder.Default
                    Return issues '.OrderByDescending(x => x, new DefaultSortComparer()).ThenByDescending(x => x.Created);
                Case IssueSortOrder.CreatedDescending
                    Return issues.OrderByDescending(Function(x) x.Created)
                Case IssueSortOrder.VotesAscending
                    Return issues.OrderBy(Function(x) x.Votes).ThenByDescending(Function(x) x.Created)
                Case IssueSortOrder.VotesDescending
                    Return issues.OrderByDescending(Function(x) x.Votes).ThenByDescending(Function(x) x.Created)
                Case Else
                    Throw New InvalidOperationException()
            End Select
        End Function
#End Region
    End Module
End Namespace
