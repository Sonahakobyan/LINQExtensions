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
                new Person{ FirstName = "FirstNameA", LastName = "LastName" },
                new Person{ FirstName = "FirstNameB", LastName = "LastName" },
                new Person{ FirstName = "OtherName", LastName = "LastName" },
                new Person{ FirstName = "Other", LastName = "Other" },
            };

            IEnumerable<Char> filtered = people.ExtensionWhere(x => x.LastName == "LastName" && x.FirstName.StartsWith("F")).ExtensionSelect(x => x.FirstName).Where(x => x.Contains("B")).Select(x => x[0]);

            foreach (Char p in filtered)
            {
                Console.WriteLine(p.ToString());
            }

            Console.Read();
        }
    }
}