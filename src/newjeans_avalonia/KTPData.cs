namespace newjeans_avalonia
{
    public class KTPData
    {
        public string NIK { get; set; }
        public string name { get; set; }
        public string birth_place { get; set; }
        public string birth_date { get; set; }
        public string gender { get; set; }
        public string blood_type { get; set; }
        public string address { get; set; }
        public string religion { get; set; }
        public string marriage_status { get; set; }
        public string job { get; set; }
        public string citizenhip { get; set; }

        public KTPData(string NIK, string name, string birth_place, string birth_date, string gender, string blood_type, string address, string religion, string marriage_status, string job, string citizenhip)
        {
            this.NIK = NIK;
            this.name = name;
            this.birth_place = birth_place;
            this.birth_date = birth_date;
            this.gender = gender;
            this.blood_type = blood_type;
            this.address = address;
            this.religion = religion;
            this.marriage_status = marriage_status;
            this.job = job;
            this.citizenhip = citizenhip;
        }
    }
}
