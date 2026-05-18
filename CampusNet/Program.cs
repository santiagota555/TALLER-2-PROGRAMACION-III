using CampusNet.Controller;

namespace CampusNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new GraphController();
            controller.Run();
        }
    }
}
