using System;

namespace InfiniteAsyncSourceSample {
    public class IssueData {
        public IssueData(int id, string subject, string user, DateTime created, int votes, Priority priority) {
            Id = id;
            Subject = subject;
            User = user;
            Created = created;
            Votes = votes;
            Priority = priority;
        }
        public int Id { get; private set; }
        public string Subject { get; set; }
        public string User { get; set; }
        public DateTime Created { get; set; }
        public int Votes { get;  set; }
        public Priority Priority { get; set; }

        public IssueData Clone() {
            return new IssueData(Id, Subject, User, Created, Votes, Priority);
        }
    }
    public enum Priority { Low, BelowNormal, Normal, AboveNormal, High }
}
