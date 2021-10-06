Imports DevExpress.Mvvm
Imports DevExpress.Mvvm.DataAnnotations
Imports DevExpress.Xpf.Data
Imports DevExpress.Mvvm.Xpf
Imports System
Imports System.ComponentModel
Imports System.Linq
Imports System.Threading.Tasks

Namespace InfiniteAsyncSourceMVVMSample

    Public Class IssueServiceViewModel
        Inherits ViewModelBase

        Public Property Users As UserData()
            Get
                Return GetValue(Of UserData())()
            End Get

            Private Set(ByVal value As UserData())
                SetValue(value)
            End Set
        End Property

        Public Sub New()
            AssignUsers()
        End Sub

        Private Async Sub AssignUsers()
            Users = Await GetUsersAsync()
        End Sub

        <Command>
        Public Sub FetchIssues(ByVal args As FetchRowsAsyncArgs)
            args.Result = GetIssuesAsync(args)
        End Sub

        Private Async Function GetIssuesAsync(ByVal args As FetchRowsAsyncArgs) As Task(Of FetchRowsResult)
            Dim take = If(args.Take, 30)
            Dim issues = Await IssuesService.GetIssuesAsync(skip:=args.Skip, take:=take, sortOrder:=GetIssueSortOrder(args.SortOrder), filter:=CType(args.Filter, IssueFilter))
            Return New FetchRowsResult(issues, hasMoreRows:=issues.Length = take)
        End Function

        Private Shared Function GetIssueSortOrder(ByVal sortOrder As SortDefinition()) As IssueSortOrder
            If sortOrder.Length > 0 Then
                Dim sort = sortOrder.[Single]()
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

        <Command>
        Public Sub GetTotalSummaries(ByVal args As GetSummariesAsyncArgs)
            args.Result = GetTotalSummariesAsync(args)
        End Sub

        Private Shared Async Function GetTotalSummariesAsync(ByVal e As GetSummariesAsyncArgs) As Task(Of Object())
            Dim summaryValues = Await GetSummariesAsync(CType(e.Filter, IssueFilter))
            Return e.Summaries.[Select](Function(x)
                If x.SummaryType = SummaryType.Count Then Return CObj(summaryValues.Count)
                If x.SummaryType = SummaryType.Max AndAlso Equals(x.PropertyName, "Created") Then Return summaryValues.LastCreated
                Throw New InvalidOperationException()
            End Function).ToArray()
        End Function

        <Command>
        Public Sub GetUniqueValues(ByVal args As GetUniqueValuesAsyncArgs)
            If Equals(args.PropertyName, "Priority") Then
                Dim values = [Enum].GetValues(GetType(Priority)).Cast(Of Object)().ToArray()
                args.Result = Task.FromResult(values)
            Else
                Throw New InvalidOperationException()
            End If
        End Sub

        <Command>
        Public Sub UpdateIssue(ByVal args As RowValidationArgs)
            If args.IsNewItem Then
                args.ResultAsync = AddNewIssueAsync(CType(args.Item, IssueData))
            Else
                args.ResultAsync = UpdateIssueAsync(CType(args.Item, IssueData))
            End If
        End Sub

        Private Shared Async Function UpdateIssueAsync(ByVal issue As IssueData) As Task(Of ValidationErrorInfo)
            Await UpdateRowAsync(issue)
            Return Nothing
        End Function

        Private Shared Async Function AddNewIssueAsync(ByVal issue As IssueData) As Task(Of ValidationErrorInfo)
            Await AddNewRowAsync(issue)
            Return Nothing
        End Function

        <Command>
        Public Sub InitNewIssue(ByVal args As NewRowArgs)
            args.Item = IssuesService.InitNewIssue()
        End Sub

        <Command>
        Public Sub DeleteIssues(ByVal args As DeleteRowsValidationArgs)
            For Each item As IssueData In args.Items
                DeleteRowAsync(item).Wait()
            Next
        End Sub
    End Class
End Namespace
