Imports System

Namespace InfiniteAsyncSourceSample
    Public Class IssueFilter
        Public Sub New(Optional ByVal priority? As Priority = Nothing, Optional ByVal createdFrom? As Date = Nothing, Optional ByVal createdTo? As Date = Nothing, Optional ByVal minVotes? As Integer = Nothing)
            Me.Priority = priority
            Me.CreatedFrom = createdFrom
            Me.CreatedTo = createdTo
            Me.MinVotes = minVotes
        End Sub
        Private privatePriority? As Priority
        Public Property Priority() As Priority?
            Get
                Return privatePriority
            End Get
            Private Set(ByVal value? As Priority)
                privatePriority = value
            End Set
        End Property
        Private privateCreatedFrom? As Date
        Public Property CreatedFrom() As Date?
            Get
                Return privateCreatedFrom
            End Get
            Private Set(ByVal value? As Date)
                privateCreatedFrom = value
            End Set
        End Property
        Private privateCreatedTo? As Date
        Public Property CreatedTo() As Date?
            Get
                Return privateCreatedTo
            End Get
            Private Set(ByVal value? As Date)
                privateCreatedTo = value
            End Set
        End Property
        Private privateMinVotes? As Integer
        Public Property MinVotes() As Integer?
            Get
                Return privateMinVotes
            End Get
            Private Set(ByVal value? As Integer)
                privateMinVotes = value
            End Set
        End Property
    End Class
End Namespace
