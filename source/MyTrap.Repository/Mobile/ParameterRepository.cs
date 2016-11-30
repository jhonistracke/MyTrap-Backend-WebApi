using MyTrap.Model.Mobile.Result;
using MyTrap.Repository.Mobile.Contracts;
using MyTrap.Repository.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile
{
    public class ParameterRepository : IParameterRepository
    {
        public async Task<List<ParameterResult>> List()
        {
            List<ParameterResult> response = new List<ParameterResult>();

            List<Parameter> parameters = await AppRepository.EntitiesContext.Parameters.Where(obj => 1 == 1).ToListAsync();

            foreach (Parameter parameter in parameters)
            {
                response.Add(Parse(parameter));
            }

            return response;
        }

        private ParameterResult Parse(Parameter parameter)
        {
            ParameterResult response = new ParameterResult();

            response.Description = parameter.Description;
            response.Id = parameter.Id.ToString();
            response.Key = parameter.Key;
            response.Value = parameter.Value;

            return response;
        }
    }
}