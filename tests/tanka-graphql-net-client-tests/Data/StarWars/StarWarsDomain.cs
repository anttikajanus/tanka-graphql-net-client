using System.Collections.Generic;

namespace Tanka.GraphQL.Net.Client.Tests.Data.StarWars
{
    public abstract class Character
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double GeneralPerformanceRaiting { get; set; }
    }

    public class Human : Character
    {
        public List<Human> Friends { get; set; } = new List<Human>();

        public List<string> AppearsIn { get; set; } = new List<string>();      
    }
}
