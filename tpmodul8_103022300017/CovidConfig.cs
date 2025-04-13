using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
public class CovidConfig
{
    [JsonPropertyName("satuan_suhu")]
    public string SatuanSuhu { get; set; }
    [JsonPropertyName("batas_hari_demam")]
    public int BatasHariDemam { get; set; }
    [JsonPropertyName("pesan_ditolak")]
    public string PesanDitolak { get; set; }
    [JsonPropertyName("pesan_diterima")]
    public string PesanDiterima { get; set; }
    private const string DefaultSatuanSuhu = "celcius";
    private const int DefaultBatasHariDemam = 14;
    private const string DefaultPesanDitolak = "Anda tidak diperbolehkan masuk ke dalam gedung ini";
    private const string DefaultPesanDiterima = "Anda dipersilahkan untuk masuk ke dalam gedung ini";
    public CovidConfig()
    {
        SetDefaults();
    }
    private void SetDefaults()
    {
        SatuanSuhu = DefaultSatuanSuhu;
        BatasHariDemam = DefaultBatasHariDemam;
        PesanDitolak = DefaultPesanDitolak;
        PesanDiterima = DefaultPesanDiterima;
    }
    public static CovidConfig LoadFromFile(string filePath)
    {
        CovidConfig config = new CovidConfig();
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                CovidConfig loadedConfig = JsonSerializer.Deserialize<CovidConfig>(json);
                if (loadedConfig != null)
                {
                    config = loadedConfig;
                }
                else
                {
                    Console.WriteLine($"Peringatan: Gagal membaca file konfigurasi '{filePath}', menggunakan nilai default.");
                }
            }
            else
            {
                Console.WriteLine($"File konfigurasi '{filePath}' tidak ditemukan, menggunakan nilai default dan membuat file baru.");
                config.SaveToFile(filePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saat memuat konfigurasi: {ex.Message}. Menggunakan nilai default.");
            config.SetDefaults();
        }
        return config;
    }
    public void SaveToFile(string filePath)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saat menyimpan konfigurasi: {ex.Message}");
        }
    }
    public void UbahSatuan()
    {
        if (SatuanSuhu.ToLower() == "celcius")
        {
            SatuanSuhu = "fahrenheit";
        }
        else if (SatuanSuhu.ToLower() == "fahrenheit")
        {
            SatuanSuhu = "celcius";
        }
    }
}