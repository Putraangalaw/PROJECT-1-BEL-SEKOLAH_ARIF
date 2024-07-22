using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using Database;
using Component;
using System.Threading;

namespace PROJECT_1_BEL_SEKOLAH__ARIF_IK2
{
    class Program
    {
        public static AccessDatabaseHelper DB = new AccessDatabaseHelper("./Arif.accdb");

        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Kotak Kepala = new Kotak();
            Kepala.X = 0;
            Kepala.Y = 0;
            Kepala.Width = 110;
            Kepala.Height = 5;
            Kepala.Tampil();

            Kotak Sikil = new Kotak();
            Sikil.X = 0;
            Sikil.Y = 25;
            Sikil.Width = 110;
            Sikil.Height = 3;
            Sikil.Tampil();

            Kotak Kiri = new Kotak();
            Kiri.SetXY(0, 6);
            Kiri.SetWidthAndHeight(30, 18);
            Kiri.Tampil();

            Kotak Kanan = new Kotak();
            Kanan.SetXY(31, 6);
            Kanan.SetWidthAndHeight(79, 18);
            Kanan.Tampil();

            Tulisan NamaAplikasi = new Tulisan();
            NamaAplikasi.Text = "APLIKASI BEL SEKOLAH";
            NamaAplikasi.X = 0;
            NamaAplikasi.Y = 1;
            NamaAplikasi.Length = 110;
            NamaAplikasi.TampilTengah();

            Tulisan Sekolah = new Tulisan();
            Sekolah.Text = "WEARNES EDUCATION CENTER";
            Sekolah.SetXY(0, 2).SetLength(110);
            Sekolah.SetForeColor(ConsoleColor.Cyan);
            Sekolah.BackColor = ConsoleColor.DarkMagenta;
            Sekolah.TampilTengah();

            Tulisan Alamat = new Tulisan();
            Alamat.Text = "JALAN THAMRIN 35A KOTA MADIUN";
            Alamat.ForeColor = ConsoleColor.Red;
            Alamat.SetXY(0, 3);
            Alamat.Length = 109;
            Alamat.TampilTengah();

            Tulisan TulisanNengNgisor = new Tulisan();
            TulisanNengNgisor.SetText("ARIF AFRIYEN SYAPUTRA").SetForeColor(ConsoleColor.Blue).SetXY(0, 26).SetLength(110).TampilTengah();
            new Tulisan().SetXY(0, 27).SetText("INFORMATIKA II-BESTFAL").SetLength(110).SetForeColor(ConsoleColor.Yellow).TampilTengah();


            Menu menu = new Menu("JALANKAN","Lihat Data", "TAMBAH DATA", "EDIT DATA", "HAPUS DATA", "KELUAR");
            menu.SetXY(5, 10);
            menu.ForeColor = ConsoleColor.Cyan;
            menu.SetSelectedBackColor( ConsoleColor.Yellow);
            menu.SetSelectedForeColor(ConsoleColor.Blue);
            menu.Tampil();
            

            bool IsProgramJalan = true;

            while (IsProgramJalan)
            {
                ConsoleKeyInfo Tombol = Console.ReadKey(true);

                if (Tombol.Key == ConsoleKey.DownArrow)
                {
                    //Tombol Panah Ke Bawah
                    menu.Next();
                    menu.Tampil();

                }
                else if (Tombol.Key == ConsoleKey.UpArrow)
                {
                    //Tombol Panah Ke Atas
                    menu.Prev();
                    menu.Tampil();
                }
                else if (Tombol.Key == ConsoleKey.Enter)
                {
                    //Enter
                    int MenuTerpilih = menu.SelectedIndex;

                    if (MenuTerpilih == 0)
                    {
                        // Menu Jalankan
                        Jalankan();

                    }

                    else if (MenuTerpilih == 1)
                    {
                        //Menu Lihat Data
                        LihatData();
                    }
                    else if(MenuTerpilih == 2)
                    {
                        //Menu Tambah Data
                        TambahData();
                    }
                    else if(MenuTerpilih == 3)
                    {
                        //Menu Edit Data
                        EditData();
                    }
                    else if(MenuTerpilih == 4)
                    {
                        //Menu Hapus Data
                        HapusData();
                    }
                    else if (MenuTerpilih == 5)
                    {
                        //Keluar Aplikasi
                        IsProgramJalan = false;
                    }


                }
            }
        }

        static void Jalankan()
        {
            new Clear(32, 7, 76, 16).SetBackColor(ConsoleColor.Gray).Tampil();
            Tulisan Judul = new Tulisan();
            Judul.SetXY(31, 7).SetText(".: Jalankan :.").SetLength(79);
            Judul.TampilTengah();

            Tulisan HariSekarang = new Tulisan().SetXY(33, 9);
            Tulisan JamSekarang = new Tulisan().SetXY(33, 10);
            
            string QSelect = "SELECT * FROM Tb_jadwal WHERE hari=@hari AND jam=@jam;";

            DB.Connect();

            bool play = true;

            while (play)
            {
                DateTime Sekarang = DateTime.Now;

                HariSekarang.SetText("HARI SEKARANG : " + Sekarang.ToString("dddd"));
                HariSekarang.Tampil();

                JamSekarang.SetText("JAM SEKARANG : " + Sekarang.ToString("HH:mm:ss"));
                JamSekarang.Tampil();

                DataTable DT = DB.RunQuery(QSelect,
                    new OleDbParameter("@hari", Sekarang.ToString("ddd")),
                    new OleDbParameter("@jam", Sekarang.ToString("HH:mm")));

            if(DT.Rows.Count > 0)
                {
                    Audio HAA = new Audio();
                    HAA.File = "./Suara" + DT.Rows[0]["Sound"];
                    HAA.Play();

                    new Tulisan().SetXY(31, 14).SetText("BEL TELAH BERBUNYI!!!").SetBackColor(ConsoleColor.Red).SetLength(79).TampilTengah();
                    new Tulisan().SetXY(31, 15).SetText(DT.Rows[0]["Ket"].ToString()).SetBackColor(ConsoleColor.Red).SetLength(79).TampilTengah();

                    play = false;
                }

                Thread.Sleep(1000);
            }
        }

        static void LihatData()
        {
            new Clear(32, 7, 76, 16).SetBackColor(ConsoleColor.Gray).Tampil();
            Tulisan Judul = new Tulisan();
            Judul.SetXY(32, 8).SetText(".: Lihat Data Jadwal.: ").SetLength(79);
            Judul.TampilTengah();
            
            DB.Connect();
            DataTable DT = DB.RunQuery("Select * From Tb_jadwal");

            new Tulisan("┌─────┬─────────────┬─────────────┬────────────────────────────────────────┐").SetXY(34, 10).Tampil();
            new Tulisan("│ NO  │  HARI       │      JAM    │   KETERANGAN                           │").SetXY(34, 11).SetBackColor(ConsoleColor.Red).Tampil();
            new Tulisan("├─────┼─────────────┼─────────────┼────────────────────────────────────────┤").SetXY(34, 12).Tampil();
           


            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string ID = DT.Rows[i]["id"].ToString();
                string Hari = DT.Rows[i]["Hari"].ToString();
                string Jam = DT.Rows[i]["Jam"].ToString();
                string Ket = DT.Rows[i]["Ket"].ToString();

                String isi = string.Format("│{0, -5}│{1, -13}│{2, -13}│{3, -40}│", ID, Hari, Jam, Ket);
                new Tulisan(isi).SetXY(34, 13 + i).Tampil();
            }

            new Tulisan("└─────┴─────────────┴─────────────┴────────────────────────────────────────┘").SetXY(34, 13 + DT.Rows.Count).Tampil();
            


        }

        static void TambahData()
        {
            new Clear(32, 7, 76, 16).SetBackColor(ConsoleColor.Gray).Tampil();
            Tulisan Judul = new Tulisan();
            Judul.SetXY(31, 7).SetText(".: Tambah Data Jadwal .: ").SetLength(79);
            Judul.TampilTengah();
            
            Inputan HariInput = new Inputan();
            HariInput.Text = "Masukkan Hari :";
            HariInput.SetXY(33, 9);

            Inputan JamInput = new Inputan();
            JamInput.Text = "Masukkan Jam :";
            JamInput.SetXY(33, 10);

            Inputan KetInput = new Inputan();
            KetInput.Text = "Masukkan Keterangan :";
            KetInput.SetXY(33, 11);

            //Inputan SoundInput = new Inputan();
            //SoundInput.Text = "Masukkan Sound :";
            //SoundInput.SetXY(33, 12);

            Pilihan SoundInput = new Pilihan();
            SoundInput.SetPilihans(
                "5 Menit Akhir Istirahat I.wav",
                "5 Menit Akhir Istirahat II.wav",
                "5 Menit Akhir Kegiatan Keagamaan.wav",
                "Pelajaran Ke 1.wav",
                "Pelajaran Ke 2.wav",
                "Pelajaran Ke 3.wav",
                "Pelajaran Ke 4.wav",
                "Pelajaran Ke 5.wav",
                "Pelajaran Ke 6.wav",
                "Pelajaran Ke 7.wav",
                "Pelajaran Ke 8.wav",
                "Pelajaran Ke 9.wav",
                "Pembuka.wav");

            SoundInput.Text = "Masukkan Audio :";
            SoundInput.SetXY(33, 12);
                

            string Hari = HariInput.Read();
            string Jam = JamInput.Read();
            string Ket = KetInput.Read();
            string Sound = SoundInput.Read();


            DB.Connect();
            DB.RunNonQuery("INSERT INTO Tb_jadwal ( hari, jam, ket, sound) VALUES ( @hari, @jam, @ket, @sound);",
                new OleDbParameter("@hari", Hari),
                new OleDbParameter("@jam", Jam),
                new OleDbParameter("@ket", Ket),
                new OleDbParameter("@sound", Sound)
                );

            DB.Disconnect();

            new Tulisan().SetXY(31, 14).SetText("Data Berhasil Di Simpan!!!!").SetBackColor(ConsoleColor.Red).SetLength(79).TampilTengah();
        }

        static void EditData()
        {
            new Clear(32, 7, 76, 16).SetBackColor(ConsoleColor.Gray).Tampil();
            Tulisan Judul = new Tulisan();
            Judul.SetXY(31, 7).SetText(".: Edit Data Jadwal .: ").SetLength(79);
            Judul.TampilTengah();
            
            Inputan HariInput = new Inputan();
            HariInput.Text = "Masukkan Hari :";
            HariInput.SetXY(33, 9);

            Inputan JamInput = new Inputan();
            JamInput.Text = "Masukkan Jam :";
            JamInput.SetXY(33, 10);

            Inputan KetInput = new Inputan();
            KetInput.Text = "Masukkan Katerangan :";
            KetInput.SetXY(33, 11);

            Pilihan SoundInput = new Pilihan();
            SoundInput.SetPilihans(
                "5 Menit Akhir Istirahat I.wav",
                "5 Menit Akhir Istirahat II.wav",
                "5 Menit Akhir Kegiatan Keagamaan.wav",
                "Pelajaran Ke 1.wav",
                "Pelajaran Ke 2.wav",
                "Pelajaran Ke 3.wav",
                "Pelajaran Ke 4.wav",
                "Pelajaran Ke 5.wav",
                "Pelajaran Ke 6.wav",
                "Pelajaran Ke 7.wav",
                "Pelajaran Ke 8.wav",
                "Pelajaran Ke 9.wav",
                "Pembuka.wav");

            SoundInput.Text = "Masukkan Audio :";
            SoundInput.SetXY(33, 14);

            string Hari = HariInput.Read();
            string Jam = JamInput.Read();
            string Ket = KetInput.Read();
            string Sound = KetInput.Read();

            DB.Connect();
            DB.RunNonQuery("UPDATE Tb_jadwal SET hari=@hari, jam=@jam, ket=@ket, sound=@sound) WHERE id=@id;",
                new OleDbParameter("@hari", Hari),
                new OleDbParameter("@jam", Jam),
                new OleDbParameter("@ket", Ket),
                new OleDbParameter("@sound", Sound)
                );

            DB.Disconnect();

            new Tulisan().SetXY(31, 16).SetText("Data Berhasil Di Update!!!!").SetBackColor(ConsoleColor.Red).SetLength(79).TampilTengah();

        }

        static void HapusData()
        {
            new Clear(32, 7, 76, 16).SetBackColor(ConsoleColor.Gray).Tampil();
            Tulisan Judul = new Tulisan();
            Judul.SetXY(31, 7).SetText(".: Hapus Data Jadwal .: ").SetLength(79);
            
            Inputan IDInput = new Inputan();
            IDInput.Text = "Masukkan ID Yang Di Hapus :";
            IDInput.SetXY(33, 9);
            string ID = IDInput.Read();

            // Cara Pertama
            // DB.Connect();
            //DB.RunNonQuery("DELETE FROM  Tb-jadwal WHERE id" + ID + ";);


            DB.Connect();
            DB.RunNonQuery("DELETE FROM Tb_jadwal WHERE id=@id;",
                new OleDbParameter("@id", ID));
            DB.Disconnect();

            new Tulisan().SetXY(31, 12).SetText("Data Berhasil Di Hapus!!!!").SetBackColor(ConsoleColor.Red).SetLength(79).TampilTengah();


        }
        static void Logo()
        {
            new Tulisan("                          .                       ").SetXY(31, 8).SetLength(79).TampilTengah();
            new Tulisan("                        .+#:                      ").SetXY(31, 9).SetLength(79).TampilTengah();
            new Tulisan("                      .+ *@-                      ").SetXY(31, 10).SetLength(79).TampilTengah();
            new Tulisan("                      +  =@@=                     ").SetXY(31, 11).SetLength(79).TampilTengah();
            new Tulisan("                     +  +-*@@=                    ").SetXY(31, 12).SetLength(79).TampilTengah();
            new Tulisan("                    .+  :@- %@@+                  ").SetXY(31, 13).SetLength(79).TampilTengah();
            new Tulisan("                   .+   %@= :@@@#                 ").SetXY(31, 14).SetLength(79).TampilTengah();
            new Tulisan("                  .+   +@@=  +@@@%.               ").SetXY(31, 15).SetLength(79).TampilTengah();
            new Tulisan("                  .+   .@@@+   #@@@@:             ").SetXY(31, 16).SetLength(79).TampilTengah();
            new Tulisan(" .               :+    #@@@+   .@@@@@-            ").SetXY(31, 17).SetLength(79).TampilTengah();
            new Tulisan(" .              -+    =@@@@+    -@@@@@+  .        ").SetXY(31, 18).SetLength(79).TampilTengah();
            new Tulisan(" .             -=    .@@@@@*     *@@@@@+          ").SetXY(31, 19).SetLength(79).TampilTengah();
            new Tulisan(" ..          .-=...=--@@@@@*    .*%-+%@@*.        ").SetXY(31, 20).SetLength(79).TampilTengah();
            new Tulisan("............-=.-#@@@=:%@@@#....**....=@@+.......  ").SetXY(31, 21).SetLength(79).TampilTengah();
            new Tulisan(".............+:.-*@@@+:%@@#...#*...:*@@#........  ").SetXY(31, 22).SetLength(79).TampilTengah();
            new Tulisan("..............==...+%@#.*@%..#*..=#@@@+.........  ").SetXY(31, 23).SetLength(79).TampilTengah();
            new Tulisan("...............-*....-##.=%.%*.+@@@@@=..........  ").SetXY(31, 24).SetLength(79).TampilTengah();
            new Tulisan("................:+-....-+::%#+@@@@@@=...........  ").SetXY(31, 25).SetLength(79).TampilTengah();
            new Tulisan(":.................==......:@@@@@@@%:............. ").SetXY(31, 26).SetLength(79).TampilTengah();
            new Tulisan(":..................-+======%%%###+:.............. ").SetXY(31, 27).SetLength(79).TampilTengah();
            

        }

    }
}
