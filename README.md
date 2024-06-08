<h2 align="center"> Tugas Besar 3 IF2211 Strategi Algoritma </h2>
<h1 align="center">  Pemanfaatan Pattern Matching dalam Membangun Sistem Deteksi Individu Berbasis Biometrik Melalui Citra Sidik Jari </h1>

## Contributors
|   NIM    |                  Nama                  |
| :------: | :------------------------------------: |
| 13522013 |        Denise Felicia Tiowanni         |
| 13522031 |        Zaki Yudhistira Candra          |
| 13522074 |        Muhammad Naufal Aulia          |


## Penjelasan Singkat
Algoritma Knuth-Morris-Pratt (KMP) adalah algoritma pencocokan string yang menghindari pencarian ulang yang tidak perlu dengan menggunakan informasi dari pola itu sendiri. Hal ini dilakukan dengan cara membuat border table yang membantu menentukan seberapa banyak pola yang dapat digeser ketika terjadi ketidaksesuaian karakter.

Algoritma Boyer-Moore (BM) adalah algoritma pencocokan string yang menggunakan dua heuristik utama untuk mempercepat pencocokan: Bad Character Heuristic atau Character-Jump dan Good Suffix Heuristic atau Looking-Glass. Bad Character Heuristic membantu menentukan berapa banyak pola bisa digeser ketika terjadi ketidakcocokan karakter, sementara Good Suffix Heuristic disebut sebagai Looking Glass Heuristic karena berfokus pada pencocokan bagian belakang pola (suffix) untuk mengoptimalkan pergeseran pola saat terjadi ketidakcocokan.

Regular Expression (Regex) dalam permasalahan string matching dapat didefinisikan sebagai urutan karakter yang menentukan pola pencarian. Regex biasanya digunakan untuk menemukan atau mengganti teks dalam string dengan mengidentifikasi pola tertentu yang spesifik. Pada tugas ini, sebuah pattern regex dibuat berdasarkan nama asli pada tabel sidik jari yang kemudian dicocokkan dengan nama alay pada tabel biodata.

Pada program ini, ketiga algoritma diatas digunakan untuk mencocokkan sebuah citra sidik jari terhadap biodata pemiliknya. Dan apabila tidak ditemukan, maka program akan menggunakan algoritma Hemming Distance untuk mencari yang terdekat.

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
Pastikan C# dan dotnet sudah terinstall pada sistem.

## How to Run and Use
1. Clone repository ini dengan 
    ```
    git clone https://github.com/NopalAul/Tubes3_Printuyul.git
    ```
2. Buka folder repository pada terminal.
3. Jalankan blabla.