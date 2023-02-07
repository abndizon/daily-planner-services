using System.Text.RegularExpressions;

namespace DailyPlannerServices.Operations;

public class ValidateSaveUser
{
    private Dictionary<string, object> payload;

    public Dictionary<string, List<string>> Errors { get; private set; }

    public ValidateSaveUser(Dictionary<string, object> payload)
    {
        this.payload = payload;

        this.Errors = new Dictionary<string, List<string>>();
        Errors.Add("firstName", new List<string>());
        Errors.Add("lastName", new List<string>());
        Errors.Add("emailAddress", new List<string>());
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
        Regex nameRx = new Regex(@"^[a-z ,.'-]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // First Name validation
        if (!payload.ContainsKey("firstName"))
        {
            Errors["firstName"].Add("First name is required");
        }
        else
        {
            if (nameRx.Matches(payload["firstName"].ToString()).Count == 0)
            {
                Errors["firstName"].Add("Not a valid name");
            }
        }

        // Last Name validation
        if (!payload.ContainsKey("lastName"))
        {
            Errors["lastName"].Add("Last name is required");
        }
        else
        {
            if (nameRx.Matches(payload["lastName"].ToString()).Count == 0)
            {
                Errors["lastName"].Add("Not a valid name");
            }
        }

        // Email Address
        if (!payload.ContainsKey("emailAddress"))
        {
            Errors["emailAddress"].Add("Email address is required");
        }
        else
        {
            Regex emailRx = new Regex(@"^[\w\-\.]+@([\w\-]+\.)+[\w\-]{2,4}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (emailRx.Matches(payload["emailAddress"].ToString()).Count == 0)
            {
                Errors["emailAddress"].Add("Not a valid email address");
            }
        }
    }
}