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
        Errors.Add("category_id", new List<string>());
        Errors.Add("date", new List<string>());
        Errors.Add("start_time", new List<string>());
        Errors.Add("end_time", new List<string>());
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
        if (!payload.ContainsKey("category_id"))
        {
            Errors["category_id"].Add("category id is required");
        }
        else
        {
            try
            {
                int categoryId = int.Parse(payload["category_id"].ToString());

                if (categoryId <= 0)
                {
                    Errors["category_id"].Add("category_id should be greater than 0");
                }
            }
            catch (Exception ex)
            {
                Errors["category_id"].Add(ex.Message);
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

        if (!payload.ContainsKey("start_time"))
        {
            Errors["start_time"].Add("start time is required");
        }
        else
        {
            try
            {
                startTime = DateTime.ParseExact(payload["start_time"].ToString(), "HH:mm", null);
            }
            catch (Exception ex)
            {
                Errors["start_time"].Add(ex.Message);
            }
        }

        if (!payload.ContainsKey("end_time"))
        {
            Errors["end_time"].Add("end time is required");
        }
        else
        {
            try
            {
                endTime = DateTime.ParseExact(payload["end_time"].ToString(), "HH:mm", null);
            }
            catch (Exception ex)
            {
                Errors["end_time"].Add(ex.Message);
            }
        }

        if (payload.ContainsKey("start_time") && payload.ContainsKey("end_time") 
            && Errors["start_time"].Count == 0 && Errors["end_time"].Count == 0) {
            if (endTime < startTime) {
                Errors["end_time"].Add("End Time must be greater than start time");
            }
        }
    }
}