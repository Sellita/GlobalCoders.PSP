namespace GlobalCoders.PSP.BackendApi.Base.Class;

public class CaseInSensitiveDictionary<TValue>() : Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
