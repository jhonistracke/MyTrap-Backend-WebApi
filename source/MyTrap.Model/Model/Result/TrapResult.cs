using System;

namespace MyTrap.Model.Mobile.Result
{
    public class TrapResult
    {
        public string Id { get; set; }
        public string NameKey { get; set; }
        public string Name { get; set; }
        public string NameResource { get; set; }
        public int Points { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}