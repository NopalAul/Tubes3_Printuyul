import csv
import sqlite3

def main():
    csv_file_path = "encrypted_biodata.csv"
    db_path = "EncryptedData.db"

    biodata_entries = read_csv(csv_file_path)
    insert_into_database(biodata_entries, db_path)

    print("Data inserted successfully.")

def read_csv(file_path):
    rows = []
    with open(file_path, newline='') as csvfile:
        reader = csv.reader(csvfile)
        next(reader)
        for row in reader:
            rows.append(row)
    return rows

def insert_into_database(biodata_entries, db_path):
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    cursor.execute('DROP TABLE IF EXISTS biodata')

    # tidak ada constraint soalnya dia masukin hasil encryption (random)
    create_table_sql = '''
    CREATE TABLE IF NOT EXISTS biodata (
        NIK TEXT PRIMARY KEY,
        nama TEXT NOT NULL,
        tempat_lahir TEXT NOT NULL,
        tanggal_lahir TEXT NOT NULL,
        jenis_kelamin TEXT NOT NULL,
        golongan_darah TEXT NOT NULL,
        alamat TEXT NOT NULL,
        agama TEXT NOT NULL,
        status_perkawinan TEXT NOT NULL,
        pekerjaan TEXT NOT NULL,
        kewarganegaraan TEXT NOT NULL
    )'''
    cursor.execute(create_table_sql)

    insert_sql = '''
    INSERT INTO biodata (NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan)
    VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)'''

    # buat cek apakah ada value yang lebih dari 11
    check_for_extra_values(biodata_entries)

    cursor.executemany(insert_sql, biodata_entries)

    conn.commit()
    conn.close()

def check_for_extra_values(biodata_entries):
    for i, entry in enumerate(biodata_entries, start=1):
        if len(entry) > 11:
            print(f"Row {i}: {entry}")

if __name__ == "__main__":
    main()