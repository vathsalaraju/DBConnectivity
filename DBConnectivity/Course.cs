namespace DBConnectivity
{
    public abstract class Course
    {
        private string _id;
        private string _name;
        private string _duration;
        private float _fee;
        private int seatsAvailable;
        private bool isDegree; 

        public Course()
        {
            
        }
        public Course(string id, string name, string duration, float fee,int seatsAvailable,bool isDegree)
        {
            Id = id;
            Name = name;
            Duration = duration;
            Fee = fee;
            SeatsAvailable = seatsAvailable;
            IsDegree = isDegree;
            
        }

        public string Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Duration { get => _duration; set => _duration = value; }
        public float Fee { get => _fee; set => _fee = value; }
        public int SeatsAvailable { get => seatsAvailable; set => seatsAvailable = value; }
        public bool IsDegree { get => isDegree; set => isDegree = value; }

        public override string? ToString()
        {
            return "ID : "+ this.Id + "\tName : " + this.Name +"\tDuration : " + this.Duration + "\tFee : " +this.Fee; 
        }
        abstract public double calculateMonthlyFees();
        
    }
}