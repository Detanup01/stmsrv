namespace Steam3Kit.Types
{
    /// <summary>
    /// Represents an identifier of a network task known as a job.
    /// </summary>
    public class JobID : GlobalID
    {
        /// <summary>
        /// Represents an invalid JobID.
        /// </summary>
        public static readonly JobID Invalid = new JobID();


        /// <summary>
        /// Initializes a new instance of the <see cref="JobID"/> class.
        /// </summary>
        public JobID()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JobID"/> class.
        /// </summary>
        /// <param name="jobId">The Job ID to initialize this instance with.</param>
        public JobID(ulong jobId)
            : base(jobId)
        {
        }


        /// <summary>
        /// Performs an implicit conversion from <see cref="SteamKit2.JobID"/> to <see cref="System.UInt64"/>.
        /// </summary>
        /// <param name="jobId">The Job ID.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ulong(JobID jobId)
        {
            if (jobId == null)
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            return jobId.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt64"/> to <see cref="SteamKit2.JobID"/>.
        /// </summary>
        /// <param name="jobId">The Job ID.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator JobID(ulong jobId)
        {
            return new JobID(jobId);
        }
    }
}
