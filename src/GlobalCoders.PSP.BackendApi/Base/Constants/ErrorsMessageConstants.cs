namespace GlobalCoders.PSP.BackendApi.Base.Constants;

public static class ErrorsMessageConstants
{
    public const string OneOrMoreValidationErrors = "One or more validation errors occurred.";

    public const string EmailValidator = "EmailValidator";
    public const string GreaterThanOrEqualValidator = "GreaterThanOrEqualValidator";
    public const string GreaterThanValidator = "GreaterThanValidator";
    public const string LengthValidator = "LengthValidator";
    public const string MinimumLengthValidator = "MinimumLengthValidator";
    public const string MaximumLengthValidator = "MaximumLengthValidator";
    public const string LessThanOrEqualValidator = "LessThanOrEqualValidator";
    public const string LessThanValidator = "LessThanValidator";
    public const string NotEmptyValidator = "NotEmptyValidator";
    public const string NotEqualValidator = "NotEqualValidator";
    public const string NotNullValidator = "NotNullValidator";
    public const string PredicateValidator = "PredicateValidator";
    public const string AsyncPredicateValidator = "AsyncPredicateValidator";
    public const string RegularExpressionValidator = "RegularExpressionValidator";
    public const string EqualValidator = "EqualValidator";
    public const string ExactLengthValidator = "ExactLengthValidator";
    public const string InclusiveBetweenValidator = "InclusiveBetweenValidator";
    public const string ExclusiveBetweenValidator = "ExclusiveBetweenValidator";
    public const string CreditCardValidator = "CreditCardValidator";
    public const string ScalePrecisionValidator = "ScalePrecisionValidator";
    public const string EmptyValidator = "EmptyValidator";
    public const string NullValidator = "NullValidator";
    public const string EnumValidator = "EnumValidator";
    public const string LengthSimple = "Length_Simple";
    public const string MinimumLengthSimple = "MinimumLengthSimple";
    public const string MaximumLengthSimple = "MaximumLengthSimple";
    public const string ExactLengthSimple = "ExactLengthSimple";
    public const string InclusiveBetweenSimple = "InclusiveBetweenSimple";

    public const string InvalidRoles = "InvalidRoles";

    public const string FailedDeleteFileItemFromFileStorage = "Failed to delete file from file storage";

    public const string SystemError = "An error occurred while processing the request";



    //Authors
    public const string AuthorDuplicateError = "An Author with the same details already exists";
    public const string AuthorFailSaveError = "Sorry, we couldn't save author. Please try again";
    public const string AuthorNotFound = "An Author is not found";

    //HashTags
    public const string HashTagNotFound = "A HashTag is not found";
    public const string HashTagDuplicateError = "A HashTag with the same details already exists";
    public const string HashTagFailSaveError = "Sorry, we couldn't save hashTag. Please try again";
}
