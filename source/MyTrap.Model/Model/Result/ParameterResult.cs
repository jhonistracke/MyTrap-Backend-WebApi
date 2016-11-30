using System;

namespace MyTrap.Model.Mobile.Result
{
    public class ParameterResult
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}