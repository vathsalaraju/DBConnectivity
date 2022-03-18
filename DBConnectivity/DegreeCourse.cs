

namespace DBConnectivity
{
    public class DegreeCourse : Course
    {

        public enum Level {BACHELORS, MASTERS};
        public bool isPlacementAvailable;
        public Level level;

        public DegreeCourse()
        {

        }
        public DegreeCourse(string id, string name, string duration, float fee,int seatAvailable,bool isDegree, bool isPlacementAvailable, Level level) : base(id, name, duration, fee, seatAvailable,isDegree)
        {
            this.isPlacementAvailable = isPlacementAvailable;
            this.level = level;
            // this.IsDegree = true;
        }

        public override double calculateMonthlyFees()
        {
            if(isPlacementAvailable)
            {
                return (double) ((0.1*Fee)+Fee);
            }
            return (double)Fee;
        }
    }
}