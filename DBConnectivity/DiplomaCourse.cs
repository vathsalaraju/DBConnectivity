namespace DBConnectivity
{
    public class DiplomaCourse:Course
    {   
        public enum Type {
            PROFESSIONAL,
            ACADEMIC
        }
        public Type type;
        public DiplomaCourse()
        {
            
        }
        public DiplomaCourse(string id, string name, string duration, float fee,int seatsAvailable,bool isDegree, Type type) : base(id, name, duration, fee,seatsAvailable, isDegree)
        {
            this.type = type;
            this.IsDegree = false;
        }

        public override double calculateMonthlyFees()
        {
            if(type == Type.PROFESSIONAL)
            {
                return (double) ((0.1*this.Fee)+this.Fee);
            }
            else
            return (double) ((0.05*this.Fee)+this.Fee);
        }

    }
}