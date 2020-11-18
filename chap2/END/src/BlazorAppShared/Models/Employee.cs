using System;

namespace BlazorAppShared.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public Country Country { get; set; }
        public Job Job { get; set; }
    }
}
