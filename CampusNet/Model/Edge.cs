namespace CampusNet.Model
{
    public class Edge
    {
        public string FromId { get; set; }
        public string ToId { get; set; }

        public Edge(string fromId, string toId)
        {
            FromId = fromId;
            ToId = toId;
        }

        public override string ToString()
        {
            return $"{FromId} --> {ToId}";
        }
    }
}
