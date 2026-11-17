//remed

using System;
using System.Collections.Generic;

// no 1 class dan object
public class Agen
{
    public string idAgen;
    public string namaAgen;
    public int stokAgen;

    public Agen(string idAgen, string namaAgen, int stokAgen)
    {
        this.idAgen = idAgen;
        this.namaAgen = namaAgen;
        this.stokAgen = stokAgen;
    }

    public void TampilInfo()
    {
        Console.WriteLine($"ID Agen: {idAgen}");
        Console.WriteLine($"Nama Agen: {namaAgen}");
        Console.WriteLine($"Stok Agen: {stokAgen}");
    }

    public void TambahStok(int jumlah)
    {
        stokAgen += jumlah;
    }
}

// no 2, 5 encapsulation dan abstract class
public abstract class ProdukGalon
{
    private string idProduk;
    private string namaProduk;
    private int stokGudang;

    public string IdProduk
    {
        get { return idProduk; }
        set { idProduk = value; }
    }

    public string NamaProduk
    {
        get { return namaProduk; }
        set { namaProduk = value; }
    }

    public int StokGudang
    {
        get { return stokGudang; }
    }

    public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
    {
        IdProduk = idProduk;
        NamaProduk = namaProduk;

        if (stokGudang >= 0)
            this.stokGudang = stokGudang;
        else
            this.stokGudang = 0;
    }

    public void KurangiStok(int jumlah)
    {
        if (jumlah > 0 && jumlah <= stokGudang)
        {
            stokGudang -= jumlah;
        }
    }

    public abstract void HitungBiaya(int jumlah);
}

// no 3, 4 inheritance dan polymorphism
public class GalonAkua : ProdukGalon
{
    public int harga = 20000;

    public GalonAkua(string idProduk, string namaProduk, int stokGudang)
        : base(idProduk, namaProduk, stokGudang)
    {
    }

    public override void HitungBiaya(int jumlah)
    {
        int total = jumlah * harga;
        Console.WriteLine($"Biaya {jumlah} Galon Akua: Rp{total}");
    }
}

public class GalonBerkeran : ProdukGalon
{
    public int harga = 12000;
    public string jenisKeran;

    public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
        : base(idProduk, namaProduk, stokGudang)
    {
        this.jenisKeran = jenisKeran;
    }

    public override void HitungBiaya(int jumlah)
    {
        int total = jumlah * harga;
        Console.WriteLine($"Biaya {jumlah} Galon Berkeran ({jenisKeran}): Rp{total}");
    }
}

// no 6 interface
public interface DapatDidistribusikan
{
    void KirimPaket();
}

public class DistribusiGalon : DapatDidistribusikan
{
    public Agen agenTujuan;
    public ProdukGalon produk;
    public int jumlah;

    public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
    {
        this.agenTujuan = agenTujuan;
        this.produk = produk;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("Pengiriman gagal: jumlah tidak valid.");
        }
        else if (jumlah > produk.StokGudang)
        {
            Console.WriteLine("Pengiriman gagal: stok gudang tidak cukup.");
        }
        else
        {
            produk.KurangiStok(jumlah);
            agenTujuan.TambahStok(jumlah);
            Console.WriteLine($"Paket berhasil dikirimkan ke agen {agenTujuan.namaAgen} sebanyak {jumlah} galon.");
        }
    }
}

// no 7 sindikat galon 
public class SindikatGalon
{
    // hubungan SindikatGalon dengan Agen adalah aggregation.
    // agen dapat berdiri sendiri tanpa SindikatGalon.

    public List<Agen> daftarAgen = new List<Agen>();
    public List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

    public void TambahAgen(Agen agen)
    {
        daftarAgen.Add(agen);
    }

    public void TambahProduk(ProdukGalon produk)
    {
        daftarProduk.Add(produk);
    }

    public void TampilkanSindikat()
    {
        Console.WriteLine("DAFTAR AGEN ");
        foreach (Agen agen in daftarAgen)
        {
            agen.TampilInfo();
            Console.WriteLine();
        }

        Console.WriteLine("DAFTAR PRODUK ");
        foreach (ProdukGalon produk in daftarProduk)
        {
            Console.WriteLine($"{produk.NamaProduk} - Stok: {produk.StokGudang}");
        }
    }
}

class Program
{
    static void Main()
    {
        Agen agen1 = new Agen("AG01", "Aji", 100);
        Agen agen2 = new Agen("AG02", "Asep", 50);

        Console.WriteLine("DATA AGEN ");
        agen1.TampilInfo();
        agen2.TampilInfo();

        ProdukGalon galon1 = new GalonAkua("GL01", "Galon Akua", 100);
        ProdukGalon galon2 = new GalonBerkeran("GL02", "Galon Berkeran", 500, "Keran Tekan");

        Console.WriteLine("\nPERHITUNGAN BIAYA ");
        galon1.HitungBiaya(10);
        galon2.HitungBiaya(4);

        Console.WriteLine("\nDISTRIBUSI GALON ");
        DistribusiGalon kirim1 = new DistribusiGalon(agen1, galon1, 20);
        DistribusiGalon kirim2 = new DistribusiGalon(agen2, galon2, 999);
        DistribusiGalon kirim3 = new DistribusiGalon(agen2, galon2, 0);

        kirim1.KirimPaket();
        kirim2.KirimPaket();
        kirim3.KirimPaket();

        SindikatGalon sindikat = new SindikatGalon();
        sindikat.TambahAgen(agen1);
        sindikat.TambahAgen(agen2);
        sindikat.TambahProduk(galon1);
        sindikat.TambahProduk(galon2);

        Console.WriteLine("\nDATA SINDIKAT ");
        sindikat.TampilkanSindikat();
    }
}
