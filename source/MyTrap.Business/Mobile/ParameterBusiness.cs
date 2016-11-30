using MyTrap.Business.Mobile.Contracts;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTrap.Business.Mobile
{
    public class ParameterBusiness : IParameterBusiness
    {
        public List<ParameterResult> Itens { get; set; }

        public async Task ReloadProperties()
        {
            if (Itens != null)
            {
                Itens = null;
            }

            Itens = await AppRepository.Parameter.List();
        }

        public async Task<string> GetValue(string key)
        {
            string value = string.Empty;

            if (Itens == null)
            {
                await ReloadProperties();
            }

            foreach (ParameterResult parameter in Itens)
            {
                if (parameter.Key == key)
                {
                    value = parameter.Value;
                    break;
                }
            }

            return value;
        }
    }
}