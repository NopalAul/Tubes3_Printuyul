import csv
import sqlite3

def main():
    csv_file_path = "decrypted_sidik_jari.csv"
    db_path = "EncryptedData.db"

    sidik_jari_entries = read_csv(csv_file_path)
    insert_into_database(sidik_jari_entries, db_path)

    print("Data inserted successfully.")

def read_csv(file_path):
    rows = []
    with open(file_path, newline='') as csvfile:
        reader = csv.reader(csvfile)
        next(reader)
        for row in reader:
            rows.append(row)
    return rows

def insert_into_database(sidik_jari_entries, db_path):
    conn = sqlite3.connect(db_path)
    cursor = conn.cursor()

    cursor.execute('DROP TABLE IF EXISTS sidik_jari')

    create_table_sql = '''
    CREATE TABLE IF NOT EXISTS sidik_jari (
        berkas_citra TEXT NOT NULL,
        nama TEXT NOT NULL
    )'''
    cursor.execute(create_table_sql)

    insert_sql = '''
    INSERT INTO sidik_jari (berkas_citra, nama)
    VALUES (?, ?)'''

    check_for_extra_values(sidik_jari_entries)

    cursor.executemany(insert_sql, sidik_jari_entries)

    conn.commit()
    conn.close()

def check_for_extra_values(sidik_jari_entries):
    for i, entry in enumerate(sidik_jari_entries, start=1):
        if len(entry) > 2:
            print(f"Row {i}: {entry}")

if __name__ == "__main__":
    main()
