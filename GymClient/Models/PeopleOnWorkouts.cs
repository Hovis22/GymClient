namespace GymClient.Models
{
    public class PeopleOnWorkouts
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

		public int ScheduledId { get; set; }

       public PeopleOnWorkouts(int? client,int sch)
        {
            ClientId = client;
			ScheduledId = sch;
        }
	}
}
