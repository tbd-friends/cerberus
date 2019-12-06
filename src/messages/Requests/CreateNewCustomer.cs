using MediatR;

namespace command.Requests
{
    public class CreateNewCustomer : IRequest
    {
        public string Honorific { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}