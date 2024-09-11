namespace MarinaData
{
    public static class CustomerManager
    {
        /// <summary>
        /// User is authenticated based on credentials and a customer returned if exists or null if not.
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <param name="password">Password as string</param>
        /// <returns>A user object or null.</returns>
        /// <remarks>
        /// Add additional for the docs for this application--for developers.
        /// </remarks>
        public static Customer Authenticate(InlandMarinaContext db, string username, string password)
        {
            var user = db.Customers.SingleOrDefault(usr => usr.Username == username
                                                    && usr.Password == password);
            return user; //this will either be null or an object
        }

    }
}
