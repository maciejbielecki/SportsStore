﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Proszę podać imię i nazwisko.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę podać pierwszy wiersz adresu.")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę miasta.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę województwa")]
        public string State { get; set; }
        public string Zip { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę kraju.")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Proszę podać adres Email.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail jest nieprawidłowy.")]
        public string Email { get; set; }

    }
}
