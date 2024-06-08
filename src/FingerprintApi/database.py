import random
import sqlite3
from datetime import datetime, timedelta

def random_case(char):
    return char.upper() if random.choice([True, False]) else char.lower()

def replace_with_numbers(char):
    replacements = {'a': '4', 'e': '3', 'i': '1', 'o': '0', 's': '5'}
    if char.lower() in replacements and random.choice([True, False]):
        return random_case(replacements[char.lower()])
    return char

def remove_vowels(name):
    vowels = 'aeiouAEIOU'
    return ''.join(char for char in name if char.lower() not in vowels)

def weird_name_generator(name):
    weird_name = ''.join(replace_with_numbers(random_case(char)) for char in name)
    if random.choice([True, False]):
        weird_name = remove_vowels(weird_name)
    return weird_name

def generate_unique_niks(count):
    unique_niks = set()
    while len(unique_niks) < count:
        nik = ''.join(random.choices('0123456789', k=16))
        unique_niks.add(nik)
    return list(unique_niks)

# harus >= 17 thn
def generate_random_birthdate():
    end_date = datetime.now() - timedelta(days=365 * 17)
    start_date = end_date - timedelta(days=365 * 60)
    random_date = start_date + (end_date - start_date) * random.random()
    return random_date.strftime('%Y-%m-%d')

def generate_random_city():
    cities = ["Jakarta", "Surabaya", "Bandung", "Medan", "Bekasi", "Tangerang", "Depok",
    "Semarang", "Palembang", "South Tangerang", "Makassar", "Batam", "Pekanbaru",
    "Bogor", "Bandar Lampung", "Padang", "Malang", "Denpasar", "Samarinda", 
    "Tasikmalaya", "Pontianak", "Cimahi", "Jambi", "Balikpapan", "Surakarta", 
    "Manado", "Banjarmasin", "Yogyakarta", "Cilegon", "Mataram", "Kupang", 
    "Palu", "Ambon", "Jayapura", "Kediri", "Binjai", "Gorontalo", "Magelang", 
    "Tarakan", "Pangkal Pinang", "Tegal", "Tanjungpinang", "Sabang", "Langsa",
    "Lhokseumawe", "Subulussalam", "Banda Aceh", "Sukabumi", "Cirebon", 
    "Purwokerto", "Pasuruan", "Probolinggo", "Madiun", "Blitar", "Kediri", 
    "Mojokerto", "Jombang", "Kudus", "Pati", "Salatiga", "Kebumen", 
    "Sragen", "Purbalingga", "Boyolali", "Klaten", "Sukoharjo", "Wonogiri", 
    "Banjarnegara", "Cilacap", "Brebes", "Tuban", "Lamongan", "Gresik", 
    "Sidoarjo", "Mojokerto", "Jember", "Banyuwangi", "Situbondo", 
    "Bondowoso", "Lumajang", "Probolinggo", "Pasuruan", "Malang", "Batu", 
    "Blitar", "Tulungagung", "Kediri", "Nganjuk", "Madiun", "Magetan", 
    "Ngawi", "Ponorogo", "Pacitan", "Trenggalek", "Sumenep", "Pamekasan", 
    "Sampang", "Bangkalan", "Bojonegoro", "Tuban", "Lamongan", "Gresik", 
    "Sidoarjo", "Mojokerto", "Jember", "Banyuwangi", "Situbondo", "Bondowoso", 
    "Lumajang", "Probolinggo", "Pasuruan", "Malang", "Batu", "Blitar", 
    "Tulungagung", "Kediri", "Nganjuk", "Madiun", "Magetan", "Ngawi", 
    "Ponorogo", "Pacitan", "Trenggalek", "Sumenep", "Pamekasan", "Sampang", 
    "Bangkalan"]
    return random.choice(cities)

def generate_random_attributes():
    genders = ["Perempuan", "Laki-Laki"]
    blood_types = ["A", "B", "AB", "O"]
    religions = ["Islam", "Protestan", "Katolik", "Buddha", "Hindu", "Khonghucu"]
    marital_statuses = ["Belum Menikah", "Menikah", "Cerai"]
    jobs = [
        "Belum / Tidak Bekerja", "Mengurus Rumah Tangga", "Pelajar / Mahasiswa", "Pensiunan", "Pegawai Negeri Sipil",
        "Tentara Nasional Indonesia", "Kepolisian RI", "Perdagangan", "Petani / Pekebun", "Peternak", "Nelayan / Perikanan",
        "Industri", "Konstruksi", "Transportasi", "Karyawan Swasta", "Karyawan BUMN", "Karyawan BUMD", "Karyawan Honorer",
        "Buruh Harian Lepas", "Buruh Tani / Perkebunan", "Buruh Nelayan / Perikanan", "Buruh Peternakan", "Pembantu Rumah Tangga",
        "Tukang Cukur", "Tukang Listrik", "Tukang Batu", "Tukang Kayu", "Tukang Sol Sepatu", "Tukang Las / Pandai Besi",
        "Tukang Jahit", "Penata Rambut", "Penata Rias", "Penata Busana", "Mekanik", "Tukang Gigi", "Seniman", "Tabib",
        "Paraji", "Perancang Busana", "Penerjemah", "Imam Masjid", "Pendeta", "Pastur", "Wartawan", "Ustadz / Mubaligh",
        "Juru Masak", "Promotor Acara", "Anggota DPR-RI", "Anggota DPD", "Anggota BPK", "Presiden", "Wakil Presiden",
        "Anggota Mahkamah Konstitusi", "Anggota Kabinet / Kementerian", "Duta Besar", "Gubernur", "Wakil Gubernur",
        "Bupati", "Wakil Bupati", "Walikota", "Wakil Walikota", "Anggota DPRD Provinsi", "Anggota DPRD Kabupaten", "Dosen",
        "Guru", "Pilot", "Pengacara", "Notaris", "Arsitek", "Akuntan", "Konsultan", "Dokter", "Bidan", "Perawat", "Apoteker",
        "Psikiater / Psikolog", "Penyiar Televisi", "Penyiar Radio", "Pelaut", "Peneliti", "Sopir", "Pialang", "Paranormal",
        "Pedagang", "Perangkat Desa", "Kepala Desa", "Biarawati", "Wiraswasta", "Anggota Lembaga Tinggi", "Artis", "Atlit",
        "Chef", "Manajer", "Tenaga Tata Usaha", "Operator", "Pekerja Pengolahan, Kerajinan", "Teknisi", "Asisten Ahli", "Lainnya"
    ]
    nationalities = ["Indonesia", "Malaysia", "Canada", "Jepang", "Korea", "Amerika", "Inggris", "Perancis", "Italia", "Singapura", "Thailand", "Filipina", "Vietnam", "Brunei", "Kamboja", "Laos", "Myanmar"]

    gender = random.choice(genders)
    blood_type = random.choice(blood_types)
    religion = random.choice(religions)
    marital_status = random.choice(marital_statuses)
    job = random.choice(jobs)
    nationality = random.choice(nationalities)
    
    return gender, blood_type, religion, marital_status, job, nationality

db_path = 'MainData.db'
conn = sqlite3.connect(db_path)
cursor = conn.cursor()

# CREATE TABLE QUERY
create_biodata_table_sql = '''
CREATE TABLE IF NOT EXISTS biodata (
    NIK TEXT NOT NULL PRIMARY KEY,
    nama TEXT NOT NULL,
    tempat_lahir TEXT NOT NULL,
    tanggal_lahir TEXT NOT NULL,
    jenis_kelamin TEXT NOT NULL CHECK(jenis_kelamin IN ('Laki-Laki', 'Perempuan')),
    golongan_darah TEXT NOT NULL,
    alamat TEXT NOT NULL,
    agama TEXT NOT NULL,
    status_perkawinan TEXT NOT NULL CHECK(status_perkawinan IN ('Belum Menikah', 'Menikah', 'Cerai')),
    pekerjaan TEXT NOT NULL,
    kewarganegaraan TEXT NOT NULL
);
'''
cursor.execute(create_biodata_table_sql)

# ambil semua nama dari tabel sidik_jari
cursor.execute('SELECT DISTINCT nama FROM sidik_jari LIMIT 600')
names = cursor.fetchall()
if len(names) < 600:
    raise ValueError("Not enough unique names in the sidik_jari table to match the required count.")

niks = generate_unique_niks(600)
biodata_entries = []

for i in range(600):
    original_name = names[i][0]
    weird_name = weird_name_generator(original_name)
    city = generate_random_city()
    birthdate = generate_random_birthdate()
    gender, blood_type, religion, marital_status, job, nationality = generate_random_attributes()
    address = f"Jl. {weird_name} No. {random.randint(1, 100)}"
    
    biodata_entries.append((niks[i], weird_name, city, birthdate, gender, blood_type, address, religion, marital_status, job, nationality))

insert_biodata_sql = '''
INSERT INTO biodata (NIK, Nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan)
VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
'''

cursor.executemany(insert_biodata_sql, biodata_entries)

conn.commit()
conn.close()

print("Data inserted successfully.")
