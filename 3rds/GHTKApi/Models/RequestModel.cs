namespace GHTK.Models
{
    public class RequestModel
    {
    }

    public class CreateOrderRequestModel : RequestModel
    {
        public Order Order { get; set; }
    }
}