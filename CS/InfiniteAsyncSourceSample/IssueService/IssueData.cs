using System;

namespace InfiniteAsyncSourceSample {
    public class IssueData {
        public IssueData() {

        }

        public IssueData(int id, string subject, string user, DateTime created, int votes, Priority priority) {
            Id = id;
            Subject = subject;
            User = user;
            Created = created;
            Votes = votes;
            Priority = priority;
        }
        public int Id { get; set; }
        public string Subject { get; set; }
        public string User { get; set; }
        public DateTime Created { get; set; }
        public int Votes { get;  set; }
        public Priority Priority { get; set; }
    }
    public enum Priority { Low, BelowNormal, Normal, AboveNormal, High }
}
