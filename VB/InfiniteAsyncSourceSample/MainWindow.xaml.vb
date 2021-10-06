Imports DevExpress.Data.Filtering
Imports DevExpress.Xpf.Data
Imports System
Imports System.ComponentModel
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows

Namespace InfiniteAsyncSourceSample

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            Me.InitializeComponent()
            Dim source = New InfiniteAsyncSource() With {.ElementType = GetType(IssueData), .KeyProperty = "Id"}
            AddHandler Unloaded, Sub(o, e) source.Dispose()
            AddHandler source.FetchRows, Sub(o, e) e.Result = FetchRowsAsync(e)
            AddHandler source.GetUniqueValues, Sub(o, e)
                If Equals(e.PropertyName, "Priority") Then
                    Dim values = [Enum].GetValues(GetType(Priority)).Cast(Of Object)().ToArray()
                    e.Result = Task.FromResult(values)
                Else
                    Throw New InvalidOperationException()
                End If
            End Sub
            AddHandler source.GetTotalSummaries, Sub(o, e) e.Result = GetTotalSummariesAsync(e)
            Me.grid.ItemsSource = source
        End Sub

        Private Shared Async Function FetchRowsAsync(ByVal e As FetchRowsAsyncEventArgs) As Task(Of FetchRowsResult)
            Dim sortOrder As IssueSortOrder = GetIssueSortOrder(e)
            Dim filter As IssueFilter = MakeIssueFilter(e.Filter)
            Dim take = If(e.Take, 30)
            Dim issues = Await GetIssuesAsync(skip:=e.Skip, take:=take, sortOrder:=sortOrder, filter:=filter)
            Return New FetchRowsResult(issues, hasMoreRows:=issues.Length = take)
        End Function

        Private Shared Async Function GetTotalSummariesAsync(ByVal e As GetSummariesAsyncEventArgs) As Task(Of Object())
            Dim filter As IssueFilter = MakeIssueFilter(e.Filter)
            Dim summaryValues = Await GetSummariesAsync(filter)
            Return e.Summaries.[Select](Function(x)
                If x.SummaryType = SummaryType.Count Then Return CObj(summaryValues.Count)
                If x.SummaryType = SummaryType.Max AndAlso Equals(x.PropertyName, "Created") Then Return summaryValues.LastCreated
                Throw New InvalidOperationException()
            End Function).ToArray()
        End Function

        Private Shared Function GetIssueSortOrder(ByVal e As FetchRowsAsyncEventArgs) As IssueSortOrder
            If e.SortOrder.Length > 0 Then
                Dim sort = e.SortOrder.[Single]()
                If Equals(sort.PropertyName, "Created") Then
                    If sort.Direction <> ListSortDirection.Descending Then Throw New InvalidOperationException()
                    Return IssueSortOrder.CreatedDescending
                End If

                If Equals(sort.PropertyName, "Votes") Then
                    Return If(sort.Direction = ListSortDirection.Ascending, IssueSortOrder.VotesAscending, IssueSortOrder.VotesDescending)
                End If
            End If

            Return IssueSortOrder.Default
        End Function

        Private Shared Function MakeIssueFilter(ByVal filter As CriteriaOperator) As IssueFilter
            Return filter.Match(binary:=Function(propertyName, value, type)
                If Equals(propertyName, "Votes") AndAlso type = BinaryOperatorType.GreaterOrEqual Then Return New IssueFilter(minVotes:=CInt(value))
                If Equals(propertyName, "Priority") AndAlso type = BinaryOperatorType.Equal Then Return New IssueFilter(priority:=CType(value, Priority))
                If Equals(propertyName, "Created") Then
                    If type = BinaryOperatorType.GreaterOrEqual Then Return New IssueFilter(createdFrom:=CDate(value))
                    If type = BinaryOperatorType.Less Then Return New IssueFilter(createdTo:=CDate(value))
                End If

                Throw New InvalidOperationException()
            End Function, [and]:=Function(filters) New IssueFilter(createdFrom:=filters.[Select](Function(x) x.CreatedFrom).SingleOrDefault(Function(x) x IsNot Nothing), createdTo:=filters.[Select](Function(x) x.CreatedTo).SingleOrDefault(Function(x) x IsNot Nothing), minVotes:=filters.[Select](Function(x) x.MinVotes).SingleOrDefault(Function(x) x IsNot Nothing), priority:=filters.[Select](Function(x) x.Priority).SingleOrDefault(Function(x) x IsNot Nothing)), null:=Nothing)
        End Function

        Private Sub CreateUpdateRow(ByVal sender As Object, ByVal e As DevExpress.Xpf.Grid.GridRowValidationEventArgs)
            If e.IsNewItem Then
                e.UpdateRowResult = AddNewIssueAsync(CType(e.Value, IssueData))
            Else
                e.UpdateRowResult = UpdateRowAsync(CType(e.Value, IssueData))
            End If
        End Sub

        Private Sub DeleteRows(ByVal sender As Object, ByVal e As DevExpress.Xpf.Grid.GridDeleteRowsValidationEventArgs)
            For Each row In e.Rows
                DeleteIssueAsync(CType(row, IssueData)).Wait()
            Next
        End Sub

        Private Sub InitNewRow(ByVal sender As Object, ByVal e As AddingNewEventArgs)
            e.NewObject = InitNewIssue()
        End Sub
    End Class
End Namespace
