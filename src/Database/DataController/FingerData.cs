namespace DataController {
    public class FingerprintData {
        public int id;
        public string name;
        public string path;

        public FingerprintData(string name, string path) {
            id = -1;
            this.name = name;
            this.path = path;
        }

        public FingerprintData(int id, string name, string path) {
            this.id = id;
            this.name = name;
            this.path = path;
        }

        public string getName(){
            return this.name;
        }

        public string getPath(){
            return this.path;
        }

        public void setName(string name) {
            this.name = name;
        }

        public void setPath(string path) {
            this.path = path;
        }
    }
}