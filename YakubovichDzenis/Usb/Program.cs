using System;
using System.Management;

namespace Usb
{
    internal class Program
    {
        static void Main()
        {
            // Tworzymy zapytanie WMI do monitorowania podłączenia wszystkich urządzeń
            ManagementEventWatcher watcher = new ManagementEventWatcher(
                new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 1 " +
                                  "WHERE TargetInstance ISA 'Win32_PnPEntity'"));

            // Subskrybujemy zdarzenie z odpowiednią sygnaturą
            watcher.EventArrived += DeviceInsertedEvent;

            // Rozpoczynamy monitorowanie
            watcher.Start();

            Console.WriteLine("Oczekiwanie na podłączenie urządzeń USB...");
            Console.WriteLine("Naciśnij dowolny klawisz, aby zakończyć.");

            // Czekamy na zakończenie
            Console.ReadKey();

            // Zatrzymujemy monitorowanie zdarzeń
            watcher.Stop();
        }

        // Obsługuje zdarzenie, gdy urządzenie jest podłączane
        private static void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            // Pobieramy obiekt urządzenia
            ManagementBaseObject targetInstance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            // Wyciągamy różne parametry urządzenia
            string deviceID = targetInstance["DeviceID"]?.ToString();
            string deviceDescription = targetInstance["Description"]?.ToString();
            string devicePNP = targetInstance["PNPDeviceID"]?.ToString();

            // Wyświetlamy informacje o podłączonym urządzeniu na konsoli
            Console.WriteLine("Podłączono nowe urządzenie:");
            Console.WriteLine($"DeviceID: {deviceID}");
            Console.WriteLine($"Opis urządzenia: {deviceDescription}");
            Console.WriteLine($"PNPDeviceID: {devicePNP}");
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
