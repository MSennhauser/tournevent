using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Tournavent.Models
{
    public class PersonModel : IPersonModel
    {
        private List<Person> personList;
        public PersonModel() 
        {
            personList = new List<Person>() {
            new Person()
            {
                Id = 0,
                Name = "Michael",
                Alter = 17,
                Beruf = "Informatiker"
            },
            new Person()
            {
                Id = 1,
                Name = "Michael",
                Alter = 17,
                Beruf = "Informatiker"
            }
        };
        }
        public List<Person> GetPeople()
        {
            return personList;
        }
        public Person GetPerson(int id)
        {
            Debug.WriteLine(personList.Count());
            return personList.ElementAt(id);
        }
        public Person AddPerson(Person person)
        {
            Debug.WriteLine(personList.Count());
            personList.Add(person);
            Debug.WriteLine(personList.Count());
            return person;
        }
    }
}