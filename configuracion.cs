using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.Diagnostics;


namespace Perro
{
    public class configuracion
    {
        IsolatedStorageSettings configuraciones;

        const string PrimeraKeyName = "Primeravez";

        const bool PrimeraDefault = true;

        public configuracion()
        {
            // Get the settings for this application.
            configuraciones = IsolatedStorageSettings.ApplicationSettings;
        }
         public bool AddOrUpdateValue(string Key, Object value)
         {
             bool valueChanged = false;

             // If the key exists
             if (configuraciones.Contains(Key))
             {
                 // If the value has changed
                 if (configuraciones[Key] != value)
                 {
                     // Store the new value
                     configuraciones[Key] = value;
                     valueChanged = true;
                 }
             }
             // Otherwise create the key.
             else
             {
                 configuraciones.Add(Key, value);
                 valueChanged = true;
             }
             return valueChanged;
         }
         public T GetValueOrDefault<T>(string Key, T defaultValue)
         {
             T value;

             // If the key exists, retrieve the value.
             if (configuraciones.Contains(Key))
             {
                 value = (T)configuraciones[Key];
             }
             // Otherwise, use the default value.
             else
             {
                 value = defaultValue;
             }
             return value;
         }

         public void Save()
         {
             configuraciones.Save();

         }
         public bool PrimeraVez
         {
             get
             {
                 return GetValueOrDefault<bool>(PrimeraKeyName, PrimeraDefault);
             }
             set
             {
                 if (AddOrUpdateValue(PrimeraKeyName, value))
                 {
                     Save();
                 }
             }
         }
        


    }
}
