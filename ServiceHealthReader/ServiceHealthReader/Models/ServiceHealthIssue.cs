namespace ServiceHealthReader.Models
{
    public class ServiceHealthIssueRootObject
    {
        public string odatacontext { get; set; }
        public string odatanextLink { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public DateTime startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string title { get; set; }
        public string id { get; set; }
        public string impactDescription { get; set; }
        public string classification { get; set; }
        public string origin { get; set; }
        public string status { get; set; }
        public string service { get; set; }
        public string feature { get; set; }
        public string featureGroup { get; set; }
        public bool isResolved { get; set; }
        public Detail[] details { get; set; }
        public Post[] posts { get; set; }
    }

    public class Detail
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Post
    {
        public DateTime createdDateTime { get; set; }
        public string postType { get; set; }
        public Description description { get; set; }
    }

    public class Description
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

}
