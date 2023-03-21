namespace FIREBASE.Models
{
    public class FirebaseNotificationResult
    {
        public Int64 multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FirebaseNotificationResultDetail> results { get; set; }
    }
}