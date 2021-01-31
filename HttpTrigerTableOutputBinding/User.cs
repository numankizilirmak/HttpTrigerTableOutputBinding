using Microsoft.Azure.Cosmos.Table;

namespace HttpTrigerTableOutputBinding
{
   public class User:TableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
    }
}
