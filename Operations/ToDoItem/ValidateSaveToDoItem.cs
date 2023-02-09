namespace DailyPlannerServices.Operations;

public class ValidateSaveToDoItem
{
    private Dictionary<string, object> payload;

    public Dictionary<string, List<string>> Errors { get; private set; }

    public ValidateSaveToDoItem(Dictionary<string, object> payload)
    {
        this.payload = payload;

        this.Errors = new Dictionary<string, List<string>>();
        Errors.Add("name", new List<string>());
        Errors.Add("categoryId", new List<string>());
        Errors.Add("date", new List<string>());
        Errors.Add("startTime", new List<string>());
        Errors.Add("endTime", new List<string>());
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

        // Category ID validation
        if (!payload.ContainsKey("categoryId"))
        {
            Errors["categoryId"].Add("category id is required");
        }
        else
        {
            try
            {
                int categoryId = int.Parse(payload["categoryId"].ToString());

                if (categoryId <= 0)
                {
                    Errors["categoryId"].Add("categoryId should be greater than 0");
                }
            }
            catch (Exception ex)
            {
                Errors["categoryId"].Add(ex.Message);
            }
        }

        // Date validation
        if (!payload.ContainsKey("date"))
        {
            Errors["date"].Add("date is required");
        }
        else
        {
            try
            {
                DateTime date = DateTime.ParseExact(payload["date"].ToString(), "yyyy-MM-dd", null);

            }
            catch (Exception ex)
            {
                Errors["date"].Add(ex.Message);
            }
        }

        // Time validation
        DateTime startTime = new DateTime();
        DateTime endTime = new DateTime();

        if (!payload.ContainsKey("startTime"))
        {
            Errors["startTime"].Add("start time is required");
        }
        else
        {
            try
            {
                startTime = DateTime.ParseExact(payload["startTime"].ToString(), "HH:mm", null);
            }
            catch (Exception ex)
            {
                Errors["startTime"].Add(ex.Message);
            }
        }

        if (!payload.ContainsKey("endTime"))
        {
            Errors["endTime"].Add("end time is required");
        }
        else
        {
            try
            {
                endTime = DateTime.ParseExact(payload["endTime"].ToString(), "HH:mm", null);
            }
            catch (Exception ex)
            {
                Errors["endTime"].Add(ex.Message);
            }
        }

        if (payload.ContainsKey("startTime") && payload.ContainsKey("endTime") 
            && Errors["startTime"].Count == 0 && Errors["endTime"].Count == 0) {
            if (endTime < startTime) {
                Errors["endTime"].Add("End Time must be greater than start time");
            }
        }
    }
}