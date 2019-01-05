using System;
using System.Collections.Generic;
using System.Text;

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
        public Human()
        {
        }

        public List<Human> Friends { get; } = new List<Human>();

        public List<string> AppearsIn { get; } = new List<string>();      
    }
}
