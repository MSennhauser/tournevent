using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournavent.Models
{
    public interface IPersonModel
    {
        List<Person> GetPeople();
        Person GetPerson(int id);
        Person AddPerson(Person person);
    }
}
