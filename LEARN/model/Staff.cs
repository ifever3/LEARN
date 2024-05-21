using LEARN.data;

namespace LEARN.model
{
    public class Staff :IEntity
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string major { get; set; }
    }
}