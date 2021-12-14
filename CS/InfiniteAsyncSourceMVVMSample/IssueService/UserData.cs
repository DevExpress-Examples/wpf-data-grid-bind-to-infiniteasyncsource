using System;

namespace InfiniteAsyncSourceMVVMSample {
    public class UserData {
        public UserData(int id, string firstName, string lastName) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName => FirstName + " " + LastName;
    }
}
