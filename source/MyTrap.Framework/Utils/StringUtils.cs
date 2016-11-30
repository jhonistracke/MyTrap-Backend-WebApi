using MyTrap.Framework.Properties;
using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;

namespace MyTrap.Framework.Utils
{
    public class StringUtils
    {
        public static Boolean IsSameCharacter(string value, Char character)
        {
            try
            {
                int length = value.Count();
                int qtdeCharacter = 0;

                foreach (char charCheck in value.ToCharArray())
                {
                    if (charCheck.Equals(character))
                    {
                        qtdeCharacter++;
                    }
                }

                if (length == qtdeCharacter)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string GetStringForCulture(string nameOfResource, string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                try
                {
                    var resourceUser = Resources.ResourceManager.GetResourceSet(new System.Globalization.CultureInfo(language), false, false);

                    if (resourceUser != null)
                    {
                        return resourceUser.GetString(nameOfResource);
                    }
                }
                catch (Exception) { }
            }

            return Resources.ResourceManager.GetString(nameOfResource);
        }

        public static string Pluralize(string value)
        {
            return PluralizationService.CreateService(new CultureInfo("en-US")).Pluralize(value);
        }
    }
}