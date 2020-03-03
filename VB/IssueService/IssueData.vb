Imports System

Namespace InfiniteAsyncSourceSample
	Public Class IssueData
'INSTANT VB NOTE: The variable subject was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable user was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable created was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable votes was renamed since Visual Basic does not handle local variables named the same as class members well:
'INSTANT VB NOTE: The variable priority was renamed since Visual Basic does not handle local variables named the same as class members well:
		Public Sub New(ByVal subject_Conflict As String, ByVal user_Conflict As String, ByVal created_Conflict As DateTime, ByVal votes_Conflict As Integer, ByVal priority_Conflict As Priority)
			Me.Subject = subject_Conflict
			Me.User = user_Conflict
			Me.Created = created_Conflict
			Me.Votes = votes_Conflict
			Me.Priority = priority_Conflict
		End Sub
		Private privateSubject As String
		Public Property Subject() As String
			Get
				Return privateSubject
			End Get
			Private Set(ByVal value As String)
				privateSubject = value
			End Set
		End Property
		Private privateUser As String
		Public Property User() As String
			Get
				Return privateUser
			End Get
			Private Set(ByVal value As String)
				privateUser = value
			End Set
		End Property
		Private privateCreated As DateTime
		Public Property Created() As DateTime
			Get
				Return privateCreated
			End Get
			Private Set(ByVal value As DateTime)
				privateCreated = value
			End Set
		End Property
		Private privateVotes As Integer
		Public Property Votes() As Integer
			Get
				Return privateVotes
			End Get
			Private Set(ByVal value As Integer)
				privateVotes = value
			End Set
		End Property
		Private privatePriority As Priority
		Public Property Priority() As Priority
			Get
				Return privatePriority
			End Get
			Private Set(ByVal value As Priority)
				privatePriority = value
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
