Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks

Namespace InfiniteAsyncSourceSample
	Public Module IssuesService
		#Region "helpers"
		Private SyncObject As New Object()
		Private AllIssues As New Lazy(Of IssueData())(Function()
			Dim [date] = DateTime.Today
			Dim rnd = New Random(0)
			Return Enumerable.Range(0, 100000).Select(Function(i)
				[date] = [date].AddSeconds(-rnd.Next(20 * 60))
				Return New IssueData(subject:= OutlookDataGenerator.GetSubject(), user:= OutlookDataGenerator.GetFrom(), created:= [date], votes:= rnd.Next(100), priority:= OutlookDataGenerator.GetPriority())
			End Function).ToArray()
		End Function)
		Private Class DefaultSortComparer
			Implements IComparer(Of IssueData)

			Private Function IComparerGeneric_Compare(ByVal x As IssueData, ByVal y As IssueData) As Integer Implements IComparer(Of IssueData).Compare
				If x.Created.Date <> y.Created.Date Then
					Return Comparer(Of DateTime).Default.Compare(x.Created.Date, y.Created.Date)
				End If
				Return Comparer(Of Integer).Default.Compare(x.Votes, y.Votes)
			End Function
		End Class
		#End Region

		Public Async Function GetIssuesAsync(ByVal page As Integer, ByVal pageSize As Integer, ByVal sortOrder As IssueSortOrder, ByVal filter As IssueFilter) As Task(Of IssueData())
			Await Task.Delay(300)
			Dim issues = SortIssues(sortOrder, AllIssues.Value)
			If filter IsNot Nothing Then
				issues = FilterIssues(filter, issues)
			End If
			Return issues.Skip(page * pageSize).Take(pageSize).ToArray()
		End Function

		Public Async Function GetSummariesAsync(ByVal filter As IssueFilter) As Task(Of IssuesSummaries)
			Await Task.Delay(300)
			Dim issues = DirectCast(AllIssues.Value, IEnumerable(Of IssueData))
			If filter IsNot Nothing Then
				issues = FilterIssues(filter, issues)
			End If
			Dim lastCreated = If(issues.Any(), issues.Max(Function(x) x.Created), CType(Nothing, DateTime?))
			Return New IssuesSummaries(count:= issues.Count(), lastCreated:= lastCreated)
		End Function

		#Region "filter"
		Private Function FilterIssues(ByVal filter As IssueFilter, ByVal issues As IEnumerable(Of IssueData)) As IEnumerable(Of IssueData)
			If filter.CreatedFrom IsNot Nothing OrElse filter.CreatedTo IsNot Nothing Then
				If filter.CreatedFrom Is Nothing OrElse filter.CreatedTo Is Nothing Then
					Throw New InvalidOperationException()
				End If
'INSTANT VB WARNING: Comparisons involving nullable type instances require Option Strict Off:
				issues = issues.Where(Function(x) x.Created >= filter.CreatedFrom.Value AndAlso x.Created < filter.CreatedTo)
			End If
			If filter.MinVotes IsNot Nothing Then
				issues = issues.Where(Function(x) x.Votes >= filter.MinVotes.Value)
			End If
			If filter.Priority IsNot Nothing Then
				issues = issues.Where(Function(x) filter.Priority.Equals(x.Priority))
			End If
			Return issues
		End Function
		#End Region

		#Region "sort"
		Private Function SortIssues(ByVal sortOrder As IssueSortOrder, ByVal issues As IEnumerable(Of IssueData)) As IEnumerable(Of IssueData)
			Select Case sortOrder
			Case IssueSortOrder.Default
				Return issues.OrderByDescending(Function(x) x, New DefaultSortComparer()).ThenByDescending(Function(x) x.Created)
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
