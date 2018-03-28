using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExtensions
{
    internal class Person
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public override String ToString()
        {
            return FirstName + " " + LastName;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Person> people = new List<Person>
            {
                new Person{ FirstName = "Sona", LastName = "Hakobyan" },
                new Person{ FirstName = "Suren", LastName = "Hakobyan" },
                new Person{ FirstName = "Hakob", LastName = "Hakobyan" },
                new Person{ FirstName = "Naira", LastName = "Khalatyan" },
            };

            IEnumerable<Char> filtered = people.ExtensionWhere(x => x.LastName == "Hakobyan" && x.FirstName.StartsWith("S")).ExtensionSelect(x => x.FirstName).Where(x => x.Contains("u")).Select(x => x[0]);

            foreach (Char p in filtered)
            {
                Console.WriteLine(p.ToString());
            }

            Console.Read();
        }
    }
}