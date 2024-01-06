using Bogus;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using SimplifiedSearch;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class Benchmarks
    {
        public class Person
        {
            public string FirstName { get; set; } = null!;
            public string LastName { get; set; } = null!;
            public int Age { get; set; }
            public string Email { get; set; } = null!;
            public string Address { get; set; } = null!;
        }

        private List<Person> _people = null!;
        private Person _personSearchValues = null!;

        [GlobalSetup]
        public void Setup()
        {
            var faker = new Faker<Person>()
                .StrictMode(true)
                .UseSeed(8964)
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.Age, f => f.Person.Random.Number(18, 65))
                .RuleFor(p => p.Email, f => f.Person.Email)
                .RuleFor(p => p.Address, f => f.Person.Address.Street + " " + f.Person.Address.Suite);

            _people = faker.Generate(10_000);
            _personSearchValues = faker.Generate();
        }

        [Benchmark]
        public async Task<IList<Person>> SimplestSearch()
        {
            var results = await _people.SimplifiedSearchAsync($"{_personSearchValues.FirstName} {_personSearchValues.LastName}");
            return results;
        }
    }
}
