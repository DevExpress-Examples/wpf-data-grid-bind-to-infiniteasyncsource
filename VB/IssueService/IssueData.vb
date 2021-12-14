Imports System

Namespace InfiniteAsyncSourceSample

    Public Class IssueData

        Private _Subject As String, _User As String, _Created As DateTime, _Votes As Integer, _Priority As Priority

        Public Sub New(ByVal subject As String, ByVal user As String, ByVal created As System.DateTime, ByVal votes As Integer, ByVal priority As InfiniteAsyncSourceSample.Priority)
            Me.Subject = subject
            Me.User = user
            Me.Created = created
            Me.Votes = votes
            Me.Priority = priority
        End Sub

        Public Property Subject As String
            Get
                Return _Subject
            End Get

            Private Set(ByVal value As String)
                _Subject = value
            End Set
        End Property

        Public Property User As String
            Get
                Return _User
            End Get

            Private Set(ByVal value As String)
                _User = value
            End Set
        End Property

        Public Property Created As DateTime
            Get
                Return _Created
            End Get

            Private Set(ByVal value As DateTime)
                _Created = value
            End Set
        End Property

        Public Property Votes As Integer
            Get
                Return _Votes
            End Get

            Private Set(ByVal value As Integer)
                _Votes = value
            End Set
        End Property

        Public Property Priority As Priority
            Get
                Return _Priority
            End Get

            Private Set(ByVal value As Priority)
                _Priority = value
            End Set
        End Property
    End Class

    Public Enum Priority
        Low
        BelowNormal
        Normal
        AboveNormal
        High
    End Enum
End Namespace
