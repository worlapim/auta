using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace auta
{
    public class Car
    {
        public string Model { get; set; } = "";
        public DateOnly SellDate { get; set; }
        public double Price { get; set; }
        public double Vat {  get; set; }
    }
}
