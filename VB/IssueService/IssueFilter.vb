Imports System

Namespace InfiniteAsyncSourceSample
	Public Class IssueFilter
'INSTANT VB NOTE: The variable priority was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable createdFrom was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable createdTo was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable minVotes was renamed since Visual Basic does not handle local variables named the same as class members well:
		Public Sub New(Optional ByVal priority_Conflict? As Priority = Nothing, Optional ByVal createdFrom_Conflict? As DateTime = Nothing, Optional ByVal createdTo_Conflict? As DateTime = Nothing, Optional ByVal minVotes_Conflict? As Integer = Nothing)
			Me.Priority = priority_Conflict
			Me.CreatedFrom = createdFrom_Conflict
			Me.CreatedTo = createdTo_Conflict
			Me.MinVotes = minVotes_Conflict
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
		Private privateCreatedFrom? As DateTime
		Public Property CreatedFrom() As DateTime?
			Get
				Return privateCreatedFrom
			End Get
			Private Set(ByVal value? As DateTime)
				privateCreatedFrom = value
			End Set
		End Property
		Private privateCreatedTo? As DateTime
		Public Property CreatedTo() As DateTime?
			Get
				Return privateCreatedTo
			End Get
			Private Set(ByVal value? As DateTime)
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
