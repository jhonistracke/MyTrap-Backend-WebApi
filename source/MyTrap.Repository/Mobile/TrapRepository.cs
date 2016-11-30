using MyTrap.Model.Mobile.Request;
using MyTrap.Model.Mobile.Result;
using MyTrap.Repository.Mobile.Contracts;
using MyTrap.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyTrap.Repository.Mobile
{
    public class TrapRepository : ITrapRepository
    {
        public async Task<TrapResult> GetById(string id)
        {
            Trap trap = await AppRepository.EntitiesContext.Traps.FindAsync(new Guid(id));

            return Parse(trap);
        }

        public async Task<TrapResult> GetByNameKey(string nameKey)
        {
            Trap trap = await AppRepository.EntitiesContext.Traps.FirstOrDefaultAsync(obj => obj.NameKey == nameKey);

            return Parse(trap);
        }

        public async Task InsertArmedTrap(ArmedTrapRequest request)
        {
            ArmedTrap armedTrap = Parse(request);

            armedTrap.Date = DateTime.UtcNow;
            armedTrap.Disarmed = false;

            AppRepository.EntitiesContext.ArmedTraps.Add(armedTrap);

            await AppRepository.EntitiesContext.SaveChangesAsync();
        }

        public async Task<List<ArmedTrapResult>> ListArmedTraps(float latitude, float longitude, string userId)
        {
            List<ArmedTrapResult> response = new List<ArmedTrapResult>();

            DbGeography searchLocation = DbGeography.FromText(string.Format("POINT({0} {1})", longitude.ToString(CultureInfo.InvariantCulture), latitude.ToString(CultureInfo.InvariantCulture)));

            var armedTraps = await
                 (from armedTrap in AppRepository.EntitiesContext.ArmedTraps
                  where
                     !armedTrap.Disarmed
                     && armedTrap.User.Id != new Guid(userId)
                  select new
                  {
                      Id = armedTrap.Id,
                      Date = armedTrap.Date,
                      UserId = armedTrap.User.Id,
                      Latitude = armedTrap.Latitude,
                      Longitude = armedTrap.Longitude,
                      NameKey = armedTrap.NameKey,
                      Distance = searchLocation.Distance(
                          DbGeography.FromText("POINT(" + armedTrap.Longitude + " " + armedTrap.Latitude + ")"))
                  })
                 .OrderBy(a => a.Distance)
                 .Where(a => a.Distance < 500)
                 .ToListAsync();

            if (armedTraps != null)
            {
                foreach (var armedTrap in armedTraps)
                {
                    ArmedTrapResult armedTrapResponse = new ArmedTrapResult();

                    armedTrapResponse.Date = armedTrap.Date;
                    armedTrapResponse.Id = armedTrap.Id.ToString();
                    armedTrapResponse.Latitude = armedTrap.Latitude;
                    armedTrapResponse.Longitude = armedTrap.Longitude;
                    armedTrapResponse.NameKey = armedTrap.NameKey;
                    armedTrapResponse.UserId = armedTrap.UserId.ToString();

                    response.Add(armedTrapResponse);
                }
            }

            return response;
        }

        public async Task<List<ArmedTrapResult>> ListArmedTrapsByUser(string userId)
        {
            List<ArmedTrapResult> response = new List<ArmedTrapResult>();

            List<ArmedTrap> armedTraps = await AppRepository.EntitiesContext.ArmedTraps.Where(obj => !obj.Disarmed && obj.User.Id == new Guid(userId)).Include(a => a.User).ToListAsync();

            if (armedTraps != null)
            {
                foreach (ArmedTrap armedTrap in armedTraps)
                {
                    response.Add(Parse(armedTrap));
                }
            }

            return response;
        }

        public async Task<ArmedTrapResult> GetArmedTrapById(string armedTrapId)
        {
            ArmedTrap armedTrap = await AppRepository.EntitiesContext.ArmedTraps.Where(a => a.Id == new Guid(armedTrapId)).Include(a => a.User).FirstOrDefaultAsync();

            return Parse(armedTrap);
        }

        private TrapResult Parse(Trap trap)
        {
            if (trap != null)
            {
                TrapResult response = new TrapResult();

                response.Active = trap.Active;
                response.CreateDate = trap.CreateDate;
                response.Id = trap.Id.ToString();
                response.Name = trap.NameResource;
                response.NameKey = trap.NameKey;
                response.NameResource = trap.NameResource;
                response.Points = trap.Points;

                return response;
            }
            else
            {
                return null;
            }
        }

        private ArmedTrapResult Parse(ArmedTrap armedTrap)
        {
            if (armedTrap != null)
            {
                ArmedTrapResult response = new ArmedTrapResult();

                response.Date = armedTrap.Date;
                response.Disarmed = armedTrap.Disarmed;
                response.Id = armedTrap.Id.ToString();
                response.Latitude = armedTrap.Latitude;
                response.Longitude = armedTrap.Longitude;
                response.NameKey = armedTrap.NameKey;
                response.UserId = armedTrap.User.Id.ToString();
                return response;
            }
            else
            {
                return null;
            }
        }

        private ArmedTrap Parse(ArmedTrapRequest request)
        {
            if (request != null)
            {
                ArmedTrap response = new ArmedTrap();

                response.Latitude = request.Latitude;
                response.Longitude = request.Longitude;
                response.NameKey = request.NameKey;
                response.User = AppRepository.EntitiesContext.Users.Find(new Guid(request.UserId));

                return response;
            }
            else
            {
                return null;
            }
        }
    }
}