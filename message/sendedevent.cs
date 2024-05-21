namespace message
{
        public class sendedevent
        {
            public string name { get; set; }
            public string major { get; set; }
            public sendedevent(string name, string major)
            {
                this.name = name;
                this.major = major;
            }
        }
}
