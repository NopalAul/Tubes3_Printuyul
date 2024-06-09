<h2 align="center"> Tugas Besar 3 IF2211 Strategi Algoritma </h2>
<h1 align="center">  Pemanfaatan Pattern Matching dalam Membangun Sistem Deteksi Individu Berbasis Biometrik Melalui Citra Sidik Jari </h1>

## Contributors
|   NIM    |                  Nama                  |
| :------: | :------------------------------------: |
| 13522013 |        Denise Felicia Tiowanni         |
| 13522031 |        Zaki Yudhistira Candra          |
| 13522074 |        Muhammad Naufal Aulia          |


## About Printuyul
> Printuyul merupakan sebuah desktop app yang dapat melakukan pencocokan biometrik melalui pencarian sidik jari dengan memanfaatkan algoritma pencocokan string berupa algoritma Knuth-Morris-Pratt dan Boyer-Moore.

Algoritma Knuth-Morris-Pratt (KMP) adalah algoritma pencocokan string yang menghindari pencarian ulang yang tidak perlu dengan menggunakan informasi dari pola itu sendiri. Hal ini dilakukan dengan cara membuat border table yang membantu menentukan seberapa banyak pola yang dapat digeser ketika terjadi ketidaksesuaian karakter.

Algoritma Boyer-Moore (BM) adalah algoritma pencocokan string yang menggunakan dua heuristik utama untuk mempercepat pencocokan: Bad Character Heuristic atau Character-Jump dan Good Suffix Heuristic atau Looking-Glass. Bad Character Heuristic membantu menentukan berapa banyak pola bisa digeser ketika terjadi ketidakcocokan karakter, sementara Good Suffix Heuristic disebut sebagai Looking Glass Heuristic karena berfokus pada pencocokan bagian belakang pola (suffix) untuk mengoptimalkan pergeseran pola saat terjadi ketidakcocokan.

Regular Expression (Regex) dalam permasalahan string matching dapat didefinisikan sebagai urutan karakter yang menentukan pola pencarian. Regex biasanya digunakan untuk menemukan atau mengganti teks dalam string dengan mengidentifikasi pola tertentu yang spesifik. Pada tugas ini, sebuah pattern regex dibuat berdasarkan nama asli pada tabel sidik jari yang kemudian dicocokkan dengan nama alay pada tabel biodata.

Pada program ini, ketiga algoritma diatas digunakan untuk mencocokkan sebuah citra sidik jari terhadap biodata pemiliknya. Dan apabila tidak ditemukan, maka program akan menggunakan algoritma Hemming Distance untuk mencari yang terdekat.

## The Program
![Printuyul](printuyul.gif)

## Project Structure
```
│
├── doc
│   └── Printuyul.pdf
│
├── src
│   ├── FingerprintApi
│   │   ├── ///-1
│   │   └── ///-2
│   ├── newjeans_avalonia
│   └── xx.py
│
├── test
│
└── README.md

```

## Program Dependencies
Pastikan C# dan .NET terbaru sudah terinstall pada sistem.

## How to Run and Use
1. Clone repository ini dengan 
    ```
    git clone https://github.com/NopalAul/Tubes3_Printuyul.git
    ```
2. Buka directory repository,
4. Untuk menjalankan program:
    - Windows:
        - Temukan file `setup.bat`, double click pada file tersebut atau jalankan via terminal dengan 
            ```
            ./setup.bat
            ```
        - Pada window setup, masukkan file yang dibutuhkan yaitu (*default dari repo ini):
            - sidik_jari.csv
            - encrypted_biodata.csv
        - Tunggu hingga keluar "Press any key to continue . . ."
        - Temukan file `run.bat`,  double click pada file tersebut atau jalankan via terminal dengan 
            ```
            ./run.bat
            ```
        - Program akan langsung terbuka dan siap digunakan
    - macOS:
        - Buat sebuah terminal baru, pindah ke directory FingerprintApi dengan `cd src/FingerprintApi`
        - Karena pada system macOS tidak dapat menggunakan System.Data.SQLite, maka untuk memasukkan data dari CSV ke DB, kita perlu menjalankan file python dengan cara:
            - python InsertBiodataFromCSV
            - python InsertSidikJariFromCSV
        - Masukkan command `dotnet run` untuk menjalankan backend setelah selesai memasukkan data ke DB.
        - Buat sebuah terminal baru, pindah ke directory FingerprintApi dengan `cd src/newjeans_avalonia`
        - Masukkan command `dotnet run` untuk menjalankan frontend.
        
3. Setelah itu, pengguna dapat langsung menggunakan program dengan:
1. Klik tombol "Start Here" untuk melanjutkan ke halaman berikutnya
1. Klik tombol "Insert" untuk mengupload gambar sidik jari yang hendak dicari. File gambar yang diperbolehkan hanyalah yang berekstensi .bmp
2. Pilih algoritma yang ingin digunakan pada dropdown di bagian bawah halaman. Pada bagian ini terdapat dua algoritma yang dapat digunakan, yaitu algoritma Knut Morris Pratt dan algoritma Boyer Moore.
3. Klik tombol "Search" untuk memulai pencarian sidik jari yang telah diupload. Tampilan Loading akan muncul selama proses pencarian berlangsung.
4. Setelah proses pencarian selesai, hasil pencarian akan muncul pada bagian kanan halaman. Hasil ini mencakup gambar sidik jari yang paling mirip, persentase kemiripan kedua gambar tersebut, dan waktu eksekusinya (termasuk waktu overhead fetch data dari database).
5. Pengguna dapat menekan tombol "See Details" untuk melihat detail dari hasil pencarian. Detail ini mencakup biodata dari pemilik sidik jari yang ditemukan.
6. Pengguna dapat menekan tombol "Back" untuk kembali ke halaman sebelumnya.
7. Pengguna juga dapat menekan tombol "Retry" untuk melakukan pencarian sidik jari lainnya.
