using MediatR;

namespace messages.Requests
{
    public class CreateNewCustomer : IRequest
    {
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}