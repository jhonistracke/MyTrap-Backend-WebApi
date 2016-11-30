using System;

namespace MyTrap.Model.Framework
{
    public class MyTrapBusinessException : Exception
    {
        public string Inconsistency { get; set; }

        public MyTrapBusinessException(string inconsistency)
        {
            Inconsistency = inconsistency;
        }
    }
}