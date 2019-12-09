using System;

namespace consumer.models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsDeleted => string.IsNullOrEmpty(FirstName) &&
                                 string.IsNullOrEmpty(LastName) &&
                                 string.IsNullOrEmpty(Honorific);
    }
}