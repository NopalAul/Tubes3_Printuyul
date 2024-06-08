public class FingerprintData {
    public string name;
    public string path;

    public FingerprintData(string name, string path) {
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