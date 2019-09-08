using System;
using System.Collections.Generic;
using System.Text;

namespace NautecTechTest
{
    /// <summary>
    /// Classe que representa cada pessoa, possuindo um person_id 
    /// e uma ação principal.
    /// </summary>
    class Person
    {
        public string person_id { get; set; }
        public string main_action { get; set; }

        
        public Person(string person_id, string main_action)
        {
            this.person_id = person_id;
            this.main_action = main_action;
        }
    }
}
