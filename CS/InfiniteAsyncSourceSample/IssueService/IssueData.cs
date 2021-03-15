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
        public string Subject { get; private set; }
        public string User { get; private set; }
        public DateTime Created { get; private set; }
        public int Votes { get; private set; }
        public Priority Priority { get; private set; }
    }
    public enum Priority { Low, BelowNormal, Normal, AboveNormal, High }
}
