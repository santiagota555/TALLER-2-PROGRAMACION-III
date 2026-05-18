namespace CampusNet.Model
{
    public enum UserRole
    {
        Estudiante,
        Profesor,
        Egresado
    }

    public class Vertex
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserRole Role { get; set; }

        public Vertex(string id, string name, UserRole role)
        {
            Id = id;
            Name = name;
            Role = role;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name} ({Role})";
        }
    }
}
