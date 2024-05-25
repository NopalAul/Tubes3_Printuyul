using System.Data.Common;

public class KTPData{
    public string NIK;
    public string name;
    public string birth_place;
    public string birth_date;
    public string gender;
    public string blood_type;
    public string address;
    public string religion;
    public string marriage_status;
    public string job;
    public string citizenhip;
    
    public KTPData(string NIK, string name, string birth_place, string birth_date, string gender, string blood_type, string address, string religion, string marriage_status, string job, string citizenhip) {
        this.NIK = NIK;
        this.name = name;
        this.birth_place = birth_place;
        this.birth_date = birth_date;
        this.gender = gender;
        this.religion = religion;
        this.blood_type = blood_type;
        this.address = address;
        this.marriage_status = marriage_status;
        this.job = job;
        this.citizenhip =citizenhip;
    }
}