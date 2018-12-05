using System.Xml.Serialization;

namespace Milestone4.Model
{
    public class JobApplied
    {
        public string ApplicationDate { get; set; }

        public int JobId { get; set; }

        [XmlIgnore]
        public Job Job { get; set; }
    }
}
