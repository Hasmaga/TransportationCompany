namespace TransportationCompany.Enum
{
    // Create Error Code Enum 
    public class ErrorCode
    {
        public const string ACCOUNT_NOT_FOUND = "THIS_ACCOUNT_NOT_FOUND";
        public const string PASSENGER_ADD_ERROR = "ERROR_WHILE_ADDING_PASSENGER";
        public const string CHECK_PASSENGER_ERROR = "ERROR_WHILE_CHECKING_PASSENGER";
        public const string PASSENGER_HAD_EXIST = "PASSENGER_HAD_EXIST_IN_THE_SYSTEM";
        public const string PASSWORD_INCORRECT = "PASSWORD_NOT_RIGHT";
        public const string PASSENGER_NOT_ACTIVE = "PASSENGER_NOT_ACTIVE";
        public const string BOOKING_TRIP_ERROR = "CANNOT_BOOKING_TRIP";
        public const string USER_NOT_FOUND = "USER_NOT_FOUND";
        public const string BOOKING_NOT_FOUND = "BOOKING_NOT_FOUND";
        public const string BOOKING_CANCEL_ERROR = "BOOKING_CANCEL_ERROR";
        public const string NOT_AUTHORIZED = "NOT_AUTHORIZED";
    }
}
