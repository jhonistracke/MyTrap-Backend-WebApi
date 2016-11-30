using Microsoft.Practices.Unity;
using System;
using System.Web;

namespace MyTrap.Repository
{
    public class PerHttpRequestLifetime : LifetimeManager
    {
        private readonly Guid _key = Guid.NewGuid();

        public override object GetValue()
        {
            return HttpContext.Current.Items[_key];
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[_key] = newValue;
        }

        public override void RemoveValue()
        {
            var obj = GetValue();
            HttpContext.Current.Items.Remove(obj);
        }
    }
}