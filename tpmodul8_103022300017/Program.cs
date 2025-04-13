using System;
using System.IO;
class Program
{
    static void Main(string[] args)
    {
        string configFilePath = Path.Combine(AppContext.BaseDirectory, "covid_config.json");
        CovidConfig config = CovidConfig.LoadFromFile(configFilePath);
        Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.SatuanSuhu}");
        double suhuBadan;
        while (!double.TryParse(Console.ReadLine(), out suhuBadan))
        {
            Console.WriteLine("Input suhu tidak valid. Masukkan angka (misalnya 36.5).");
            Console.WriteLine($"Berapa suhu badan anda saat ini? Dalam nilai {config.SatuanSuhu}");
        }
        Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
        int hariDemam;
        while (!int.TryParse(Console.ReadLine(), out hariDemam))
        {
            Console.WriteLine("Input hari tidak valid. Masukkan angka bulat (misalnya 0, 7, 14).");
            Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala demam?");
        }
        bool suhuValid = false;
        if (config.SatuanSuhu.ToLower() == "celcius")
        {
            suhuValid = suhuBadan >= 36.5 && suhuBadan <= 37.5;
        }
        else if (config.SatuanSuhu.ToLower() == "fahrenheit")
        {
            suhuValid = suhuBadan >= 97.7 && suhuBadan <= 99.5;
        }
        bool hariDemamValid = hariDemam < config.BatasHariDemam;
        Console.WriteLine("\n--- Hasil Pengecekan ---");
        if (suhuValid && hariDemamValid)
        {
            Console.WriteLine(config.PesanDiterima);
        }
        else
        {
            Console.WriteLine(config.PesanDitolak);
            if (!suhuValid) Console.WriteLine($"Alasan: Suhu badan ({suhuBadan} {config.SatuanSuhu}) tidak dalam rentang normal.");
            if (!hariDemamValid) Console.WriteLine($"Alasan: Riwayat demam ({hariDemam} hari lalu) belum melewati batas ({config.BatasHariDemam} hari).");
        }
        config.UbahSatuan();
        config.SaveToFile(configFilePath);
        Console.WriteLine($"\nSatuan suhu telah diubah menjadi: {config.SatuanSuhu}");
        Console.WriteLine("Konfigurasi baru telah disimpan ke " + configFilePath);
        Console.WriteLine("\nTekan Enter untuk keluar...");
        Console.ReadLine();
    }
}