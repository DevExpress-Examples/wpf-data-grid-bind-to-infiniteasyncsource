Namespace InfiniteAsyncSourceSample

    Public Class IssueData

        Public Sub New()
            Id = -1
            Created = Date.Now
            Priority = Priority.Normal
        End Sub

        Public Sub New(ByVal id As Integer, ByVal subject As String, ByVal user As String, ByVal created As Date, ByVal votes As Integer, ByVal priority As Priority)
            Me.Id = id
            Me.Subject = subject
            Me.User = user
            Me.Created = created
            Me.Votes = votes
            Me.Priority = priority
        End Sub

        Public Property Id As Integer

        Public Property Subject As String

        Public Property User As String

        Public Property Created As Date

        Public Property Votes As Integer

        Public Property Priority As Priority

        Public Function Clone() As IssueData
            Return New IssueData(Id, Subject, User, Created, Votes, Priority)
        End Function
    End Class

    Public Enum Priority
        Low
        BelowNormal
        Normal
        AboveNormal
        High
    End Enum
End Namespace
