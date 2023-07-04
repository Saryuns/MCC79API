namespace API.Utilities.Handler;

public class GenerateNik
{
    public static string NIK(string? lastNik = null)
    {
        if (lastNik is null)
        {
            return "111111";
        }

        var generateNik = Convert.ToInt32(lastNik) + 1;

        return generateNik.ToString();
    }
}