using System.ComponentModel.DataAnnotations;

namespace ApiNegosud.Models
{
        public class Address
        {
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public int Number { get; set; }
            [Required]
            public string Street { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string Country { get; set; }
            /* Propriété de navigation vers le client associé à cette adresse */
             public virtual Client? Client { get; set; }
            /* Propriété de navigation vers le client associé à cette adresse */
            public virtual Provider? Provider { get; set; }

        public override string ToString()
        {
            return $"{Number} {Street}, {Name}, {City}, {Country}";
        }
    }
}
