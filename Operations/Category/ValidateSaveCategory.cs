namespace DailyPlannerServices.Operations;

public class ValidateSaveCategory
{
    private Dictionary<string, object> payload;

    public Dictionary<string, List<string>> Errors { get; private set; }

    public ValidateSaveCategory(Dictionary<string, object> payload)
    {
        this.payload = payload;

        this.Errors = new Dictionary<string, List<string>>();
        Errors.Add("name", new List<string>());
    }

    public bool HasErrors()
    {
        return Errors.Any(x => x.Value.Count > 0);
    }

    public bool HasNoErrors()
    {
        return !HasErrors();
    }

    public void Execute()
    {
        // Name validation
        if (!payload.ContainsKey("name"))
        {
            Errors["name"].Add("name is required");
        }
    }
}